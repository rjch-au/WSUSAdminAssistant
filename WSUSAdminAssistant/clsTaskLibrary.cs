using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace WSUSAdminAssistant
{
    ////////////////////////////////////////////////////////////////////////////////////
    /// Define some easily read task conditions
    //
    enum TaskStatus
    {
        New,
        Queued,
        Pending,
        Running,
        TimedOut,
        Complete,
        Cancelled
    };

    ////////////////////////////////////////////////////////////////////////////////////
    // Class describing a single task
    //
    class Task
    {
        public Task()
        {
            // All tasks are new until either set to queued or a dependent task ID is supplied
            Status = TaskStatus.New;

            // Default timeout is 30 seconds
            TimeoutInterval = new TimeSpan(0, 0, 30);
        }

        private int _DependsOnTask;

        public int TaskID;
        
        public int DependsOnTaskID
        {
            get { return _DependsOnTask; }

            set 
            {
                _DependsOnTask = value;
                
                // If a task was new, it can be marked as pending now
                if (Status == TaskStatus.New) Status = TaskStatus.Pending;
            }
        }

        /// <summary>
        /// Sets the task that this task is dependant on
        /// </summary>
        public Task DependsOnTask
        {
            set { DependsOnTaskID = value.TaskID; }
        }

        public TaskStatus Status;
        public string CurrentStatus { get { return Status.ToString(); } }

        public DateTime TaskStarted;
        public DateTime TaskFinished;
        public TimeSpan TimeoutInterval;

        public string Computer;
        public IPAddress IP;
        public string IPAddress { get { return IP.ToString(); } }

        public clsConfig.SecurityCredential Credentials;
        public string Command;
        public string Output;

        /// <summary>
        /// Mark a task as ready for execution
        /// </summary>
        public void Ready()
        {
            Status = TaskStatus.Queued;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////
    // Class that contains and processes a list of tasks
    //
    class TaskCollection : System.Collections.CollectionBase
    {
        // Class initialisation
        public TaskCollection()
        {
            // Kick off the background worker
            wrkTaskManager.DoWork += wrkTaskManager_DoWork;
            wrkTaskManager.RunWorkerAsync();
        }

        // Private class properties and methods
        private BackgroundWorker wrkTaskManager = new BackgroundWorker();
        private int _taskidcounter = 0;
        private clsConfig cfg = new clsConfig();

        // Public properties and methods
        public Task this[int taskid]
        {
            get
            {
                // Find the task with the appropriate task ID
                foreach (Task t in this.List)
                    if (t.TaskID == taskid)
                        // Found the task - return it
                        return t;

                // Didn't find a task - return null
                return null;
            }

            set
            {
                // Locate the taskid in question
                for (int i = 0; i < this.List.Count; i++)
                {
                    Task t = (Task)this.List[i];

                    if (t.TaskID == taskid)
                    {
                        // Found it - update it and return
                        this.List[i] = value;
                        return;
                    }
                }

                // Couldn't find the task ID - throw exception

                throw new KeyNotFoundException();
            }
        }

        public Task AddTask(IPAddress ip, string computername, clsConfig.SecurityCredential credentials, string command)
        {
            // Has a valid PSExec path been supplied?
            if (cfg.PSExecPath == "")
                // No - throw exception
                throw new ConfigurationException("No valid path to PSExec has been set in Preferences.  Please set a path to PSExec in Helper Preferences.", "PSExec path not valid");

            // Did we get valid credentials?
            if (credentials == null)
            {
                // No - do we run with the current credentials?
                if (!cfg.RunWithLocalCreds)
                    // No - throw exception
                    throw new ConfigurationException("No credentials found for IP address " + ip.ToString() + " and running with local credentials disabled.  To run with local credentials, check \"Supply current credentials if no other security credentials found for IP address\" in General Preferences.",
                        "No local credentials found for remote PC");
            }

            // Build the task details
            Task t = new Task();

            t.TaskID = ++_taskidcounter;
            t.IP = ip;
            t.Credentials = credentials;
            t.Computer = computername;
            t.Command = command;

            // Add the task to the list and return it
            this.List.Add(t);
            return t;
        }

        // Events that can be triggered by task processing
        public delegate void TaskRunningEventHandler(object sender, EventArgs e);
        public event TaskRunningEventHandler TaskRun;

        protected virtual void OnRunning(Task task, EventArgs e)
        {
            if (TaskRun != null)
                TaskRun(task, e);
        }

        // Background worker that processes tasks as they become available
        private string output;

        private void wrkTaskManager_DoWork(object sender, DoWorkEventArgs e)
        {
            // Overall loop that sleeps for 1 second when there are no active tasks
            do
            {
                Thread.Sleep(1000);

                // If there are no tasks, we don't need to do anything...
                while (this.List.Count != 0)
                {
                    // Find the first completed, cancelled or task more than 5 minutes old and delete it
                    foreach (Task st in this.List)
                        if ((st.Status == TaskStatus.Complete || st.Status == TaskStatus.TimedOut || st.Status == TaskStatus.Cancelled) && DateTime.Now.Subtract(st.TaskFinished).TotalSeconds > 300)
                        {
                            this.List.Remove(st);
                            break;
                        }

                    // Get the first queued task
                    Task t = null;

                    foreach (Task st in this.List)
                    {
                        if (st.Status == TaskStatus.Running)
                            // Oops - we're already running one task - break before we start another one!
                            break;

                        if (st.Status == TaskStatus.Queued)
                        {
                            // Found one - note it and break
                            t = st;
                            break;
                        }
                    }

                    // Did we find a task?
                    if (t != null)
                    {
                        // Yes, build the task and run it
                        string param;

                        param = "\\\\" + t.IP.ToString() + " -e "; // Computer's IP address.  PSExec should not load account's profile (quicker startup)

                        // Add credentials only if we have some to add
                        if (t.Credentials != null)
                        {
                            if (t.Credentials.domain == null)
                                param += "-u " + t.Credentials.username;                                // Local user
                            else
                                param += "-u " + t.Credentials.domain + "\\" + t.Credentials.username;      // Domain user and password

                            param += " -p " + t.Credentials.password + " ";
                        }

                        // Add command to execute
                        param += t.Command;

                        // Trigger an event so other code can take action it may want to
                        t.Status = TaskStatus.Running;
                        OnRunning(t, EventArgs.Empty);

                        // Run PSExec
                        var psexec = new Process
                        {
                            StartInfo = new ProcessStartInfo
                            {
                                FileName = cfg.PSExecPath,
                                Arguments = param,
                                UseShellExecute = false,
                                RedirectStandardOutput = true,
                                RedirectStandardError = true,
                                CreateNoWindow = true
                            }
                        };

                        psexec.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
                        psexec.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);

                        output = "";

                        psexec.Start();
                        psexec.BeginOutputReadLine();
                        psexec.BeginErrorReadLine();

                        // Read output until the timeout has expired the process has terminated
                        t.TaskStarted = DateTime.Now;

                        do
                        {
                            // Is the publically available output any different to the building output?
                            if ((output != "" && t.Output == null) || (t.Output != null && output != t.Output))
                                // Yes, update the publically available output
                                t.Output = output;

                            // Attempt to flush all output then sleep for a second
                            psexec.CancelErrorRead();
                            psexec.CancelOutputRead();
                            psexec.BeginOutputReadLine();
                            psexec.BeginErrorReadLine();

                            Thread.Sleep(1000);
                        } while (DateTime.Now.Subtract(t.TaskStarted) < t.TimeoutInterval && !psexec.HasExited);

                        // Did task complete
                        if (psexec.HasExited)
                        {
                            // Yes, mark it as complete and release any dependent tasks
                            t.Status = TaskStatus.Complete;
                            ProcessDependentTasks(t.TaskID, TaskStatus.Complete);
                        }
                        else
                        {
                            // No, kill it and cancel all dependent tasks
                            psexec.Kill();

                            t.Status = TaskStatus.TimedOut;
                            ProcessDependentTasks(t.TaskID, TaskStatus.TimedOut);
                        }

                        // Note the time the task finished
                        t.TaskFinished = DateTime.Now;
                    }
                }
            } while (true);
        }

        private void ProcessDependentTasks(int taskid, TaskStatus status)
        {
            // Loop through all tasks, looking for tasks dependent on the supplied taskid
            foreach (Task t in this.List)
            {
                if (t.DependsOnTaskID == taskid)
                {
                    // Found it - what was the result of the supplied taskid?
                    switch (status)
                    {
                        case TaskStatus.Complete:
                            // Dependent tasks can be queued
                            t.Status = TaskStatus.Queued;
                            break;

                        case TaskStatus.TimedOut:
                            // All dependent tasks of both this any any task dependent on this must be cancelled
                            t.Status = TaskStatus.Cancelled;
                            t.Output = "Task cancelled due to the failure of task #" + taskid.ToString();
                            ProcessDependentTasks(t.TaskID, TaskStatus.Cancelled);
                            break;

                        case TaskStatus.Cancelled:
                            // All dependent tasks of both this any any task dependent on this must be cancelled
                            t.Status = TaskStatus.Cancelled;
                            t.Output = "Task cancelled due to the cancellation of task #" + taskid.ToString();
                            ProcessDependentTasks(t.TaskID, TaskStatus.Cancelled);
                            break;
                    }
                }
            }
        }

        private void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (outLine.Data == null) return;

            // Process each output line individually
            string[] lines = outLine.Data.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string l in lines)
            {
                // Does the text match the kind of output we get from PSExec?
                if (!Regex.IsMatch(l, "^PsExec v.*remotely$") &&
                    !Regex.IsMatch(l, "^Copyright.*Russinovich$") &&
                    !Regex.IsMatch(l, "^Sysinternals - www.sysinternals.com$") &&
                    !Regex.IsMatch(l, @"^Connecting .*\.\.\.$") &&
                    !Regex.IsMatch(l, @"^Starting .*\.\.\.$"))
                        // No, we can add it to the string
                        output += System.Environment.NewLine + l;
            }

            // Strip all double carriage returns
            while (output.Replace(System.Environment.NewLine + System.Environment.NewLine, System.Environment.NewLine) != output)
                output = output.Replace(System.Environment.NewLine + System.Environment.NewLine, System.Environment.NewLine);

            // Strip any leading or tailing carriage returns
            while (output != output.Trim('\r', '\n'))
                output = output.Trim('\r', '\n');
        }

            }
}
