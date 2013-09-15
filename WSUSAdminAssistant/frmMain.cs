using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Microsoft.UpdateServices.Administration;
using Microsoft.UpdateServices.Administration.Internal;

namespace WSUSAdminAssistant
{
    public partial class frmMain : Form
    {
        // Initialise configuration data
        private clsConfig cfg = new clsConfig();
        private clsWSUS wsus;

        private DateTime lastupdate = Convert.ToDateTime("1970-01-01 00:00:00");
        private DateTime lastupdaterun = Convert.ToDateTime("1970-01-01 00:00:00");

        // List of group columns added to grdUnapproved
        private List<DataGridViewColumn> GroupColumns = new List<DataGridViewColumn>();

        // Flags to let update procedures know if we've forced an update, or cancelled an operation
        private bool forceUpdate = true;
        private bool cancelNow = false;

        // Background workers
        BackgroundWorker wrkPinger = new BackgroundWorker();
        BackgroundWorker wrkSUSID = new BackgroundWorker();

        // User initiated task collection
        TaskCollection tasks;

        public frmMain()
        {
            InitializeComponent();

            // Initialise variables
            wsus = cfg.wsus;
        }

        private void timUpdateData_Tick(object sender, EventArgs e)
        {
            // Disable timer until it's been processed fully
            timUpdateData.Enabled = false;

            // Determine which tab is selected and call it's update procedure
            if (tabAdminType.SelectedTab.Name == tabUnapproved.Name) UpdateUnapproved();
            if (tabAdminType.SelectedTab.Name == tabEndpointFaults.Name) UpdateEndpointFaults();
            if (tabAdminType.SelectedTab.Name == tabSuperceded.Name) UpdateSupercededUpdates();
            if (tabAdminType.SelectedTab.Name == tabWSUSNotCommunicating.Name) UpdateWSUSNotCommunicating();
            if (tabAdminType.SelectedTab.Name == tabServerRestarts.Name) UpdateServerReboots();
            
            // On return, ensure the "working" dialog is not showing and re-enable the timer
            timUpdateData.Enabled = true;
            gbxWorking.Visible = false;
        }

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
        private const int WM_SETREDRAW = 11;

        private static void SuspendDrawing(Control parent)
        {
            parent.SuspendLayout();
            SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
        }

        private static void ResumeDrawing(Control parent)
        {
            parent.ResumeLayout();
            SendMessage(parent.Handle, WM_SETREDRAW, true, 0);
        }

        private void UpdateEndpointFaults()
        {
            // Update list when forced or after 2 minutes
            if (forceUpdate || DateTime.Now.Subtract(lastupdaterun).TotalSeconds > 120)
            {
                // Reset forced update flag and update timestamp
                forceUpdate = false;
                lastupdaterun = DateTime.Now;

                if (CheckDBConnection())
                {
                    // Show the update window
                    gbxWorking.Visible = true;
                    this.Refresh();

                    // Tag all rows as not having been updated...
                    foreach (DataGridViewRow r in grdEndpoints.Rows)
                    {
                        r.Cells["epUpdate"].Value = "N";
                    }

                    // Check which update types should be displayed and update those that need it.
                    if (butApproved.Checked) EndpointUpdateApproved();
                    if (butUpdateErrors.Checked) EndpointUpdateErrors();
                    if (butNotCommunicating.Checked) EndpointUpdateNotCommunicating();
                    if (butUnassigned.Checked) EndpointUpdateUngroupedComputers();
                    if (butDefaultSusID.Checked) EndpointUpdateDefaultSusID();
                    if (butGroupRules.Checked) EndpointIncorrectGroupUpdate();

                    
                    // Remove any row that hasn't been updated
                    bool removed = true;
                    
                    while (removed)
                    {
                        removed = false;

                        foreach (DataGridViewRow r in grdEndpoints.Rows)
                        {
                            if (r.Cells["epUpdate"].Value.ToString() == "N")
                            {
                                grdEndpoints.Rows.Remove(r);
                                removed = true;
                            }
                        }
                    }
                }
            }

            // Sort the datagrid
            grdEndpoints.Sort(grdEndpoints.Columns["epIP"], ListSortDirection.Ascending);

            // Alternate the row's background colour to make viewing easier - only inverting when a new PC is found
            string prevrow = "zzzzz";
            bool reverse = false;

            foreach (DataGridViewRow r in grdEndpoints.Rows)
            {
                if (r.Cells["epIP"].Value.ToString() != prevrow)
                {
                    reverse = !reverse;
                    prevrow = r.Cells["epIP"].Value.ToString();
                }

                if (reverse)
                    r.DefaultCellStyle.BackColor = Color.Empty;
                else
                    r.DefaultCellStyle.BackColor = Color.LightGray;
            }

            // Since this timer is active, ticks should occur every half a second...
            forceUpdate = false;
            timUpdateData.Interval = 500;
        }

        private void PingWorker(object sender, DoWorkEventArgs e)
        {
            // Overall loop that sleeps for 10 seconds while a form isn't active, or while all machines have been pinged in the last 30 seconds
            do
            {
                // Internal loop that keeps pinging machines whilst there are machines there to ping
                int rn;

                do
                {
                    // Find least recently pinged machine
                    rn = -1;
                    long lp = DateTime.Now.Ticks;

                    foreach (DataGridViewRow r in grdEndpoints.Rows)
                    {
                        // Has this machine ever been pinged?
                        if (r.Cells["epPingUpdated"].Value == null || r.Cells["epPingUpdated"].Value.ToString() == "")
                        {
                            // Nope, let's do this one...
                            rn = r.Index;
                            break;
                        }

                        // Yes.  Has it been pinged more than 30 seconds ago and longer ago than the last one found?
                        if (long.Parse(r.Cells["epPingUpdated"].Value.ToString()) < lp && (DateTime.Now.Ticks - long.Parse(r.Cells["epPingUpdated"].Value.ToString())) > (30 * TimeSpan.TicksPerSecond))
                        {
                            // Yep, make a note of it and keep going...
                            lp = long.Parse(r.Cells["epPingUpdated"].Value.ToString());
                            rn = r.Index;
                        }
                    }

                    // If a row was found, ping it.
                    if (rn != -1)
                        DoPing(grdEndpoints.Rows[rn]);

                    // Only continue immediately pinging if we found a row
                }
                while (rn != -1);

                // Snooze for 10 seconds before starting again.
                Thread.Sleep(10000);
            }
            while (true);

            // This thread should only exit when it is killed by the form being closed.
        }

        private void DoPing(DataGridViewRow r)
        {
            try
            {
                Ping p = new System.Net.NetworkInformation.Ping();
                PingReply pr;
                string pingstatus;

                pr = p.Send(r.Cells["epIP"].Value.ToString(), 1000);

                if (pr.Status == IPStatus.Success)
                {
                    pingstatus = pr.RoundtripTime.ToString() + "ms";
                }
                else
                {
                    pingstatus = "No Response";
                }

                // Update all grid rows with this IP address
                foreach (DataGridViewRow gr in grdEndpoints.Rows)
                {
                    if (gr.Cells["epIP"].Value.ToString() == r.Cells["epIP"].Value.ToString())
                    {
                        gr.Cells["epPing"].Value = pingstatus;
                        gr.Cells["epPingUpdated"].Value = DateTime.Now.Ticks;
                    }
                }
            }
            catch (Exception e)
            {
                r.Cells[epPing.Index].Value = "Error";
                r.Cells[epPing.Index].ToolTipText = e.Message;
            }

            // Note time was last updated
            r.Cells["epPingUpdated"].Value = DateTime.Now.Ticks;
        }

        private bool CheckDBConnection()
        {
            // Placeholder function to ensure that if the DB connection fails, tabs are disabled and the user is returned to the home tab to see what the
            // error is
            if (wsus.CheckDBConnection())
                return true;
            else
            {
                // Problem - reset tabs and return result
                tabHome.Select();
                ShowTabs(false);

                return false;
            }
        }

        private void UpdateUnapproved()
        {
            if (CheckDBConnection())
            {
                // Unapproved updates tab is selected - check when the last update was modified.
                DateTime clu = wsus.GetLastUpdated(lastupdate);

                // Update if the last update time doesn't agree the one previously recorded, if it's been more than 5 minutes since the last update or if we've forced an update
                if (clu != lastupdate && Math.Abs(clu.Subtract(lastupdaterun).Seconds) < 10 || DateTime.Now.Subtract(lastupdaterun).Minutes > 5 || forceUpdate)
                {
                    // Show the update window
                    gbxWorking.Visible = true;
                    this.Refresh();

                    // Note the time of the last update
                    lastupdate = clu;

                    // Get unapproved updates currently pending
                    clsWSUS.UnapprovedUpdates uuc = new clsWSUS.UnapprovedUpdates(cfg);
                    uuc.UpdateUnapprovedUpdates();

                    // Reset forced update flag
                    forceUpdate = false;

                    // Don't redraw the datagrid until we've finished updating it
                    SuspendDrawing(grdUnapproved);

                    // Have we ever created columns, or have rule collections changed since we last updated?
                    bool groupschanged = false;

                    if (GroupColumns.Count != uuc.Groups.Count)
                        // No group columns have been added, or the number of columns added differs to the number of groups
                        groupschanged = true;
                    else
                    {
                        // Loop through all groups and ensure that the columns match
                        for (int i = 0; i < uuc.Groups.Count; i++)
                        {
                            if (uuc.Groups[i].shortname != grdUnapproved.Columns[uaSortOrder.Index + i + 1].HeaderText)
                            {
                                // Header does not match
                                groupschanged = true;
                                break;
                            }
                        }
                    }

                    if (groupschanged)
                    {
                        // Remove old columns, but only if they've already been added
                        GroupColumns.Clear();

                        if (grdUnapproved.Columns.Count > uaSortOrder.Index)
                        {
                            for (int i = uaSortOrder.Index + 1; i < grdUnapproved.Columns.Count;  )
                                // Remove the column
                                grdUnapproved.Columns.RemoveAt(i);
                        }

                        // Add new columns
                        foreach (clsConfig.GroupUpdateRule ur in uuc.Groups)
                        {
                            // Add column
                            DataGridViewColumn c = new DataGridViewTextBoxColumn();
                            c.Name = "uag" + ur.shortname.Replace(' ', '_');
                            c.HeaderText = ur.shortname;

                            // Format the column appropriately
                            c.Width = 55;
                            c.Resizable = DataGridViewTriState.False;
                            c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                            // Store the computer group object in the header - handy for when approving or unapproving updates
                            c.Tag = ur.computergroup;
                            
                            GroupColumns.Add(c);
                        }

                        // Add columns to grdUnapproved
                        grdUnapproved.Columns.AddRange(GroupColumns.ToArray());
                    }

                    // Loop through all rows and mark them as not updated
                    foreach (DataGridViewRow r in grdUnapproved.Rows)
                        r.Tag = "N";

                    DateTime lastbreath = DateTime.Now;

                    // Loop through each unapproved update and update the datagrid accordingly
                    foreach (clsWSUS.UnapprovedUpdate uu in uuc)
                    {
                        // Have we taken a breath recently to allow other things to happen?
                        if (DateTime.Now.Subtract(lastbreath).TotalMilliseconds > 100)
                        {
                            // Nope - take a breather.
                            Application.DoEvents();
                            lastbreath = DateTime.Now;
                        }

                        // Is this update approvable for any PC in any group?
                        if (uu.PCsRequiringUpdate > 0)
                        {
                            // Try to find an existing row, adding one if it's not found
                            DataGridViewRow r = FindUnapprovedUpdateRow(uu.UpdateID);

                            // Update the row
                            r.Cells[uaID.Index].Value = uu.UpdateID;
                            r.Cells[uaUpdateName.Index].Value = uu.Title;
                            r.Cells[uaDescription.Index].Value = uu.Description;
                            r.Cells[uaKB.Index].Value = uu.KBArticle;
                            r.Cells[uaSortOrder.Index].Value = uu.SortIndex;

                            // Tag the row as updated
                            r.Tag = "Y";

                            // Loop through each group, adding details
                            foreach (clsWSUS.PerGroupInformation gi in uu.Groups)
                            {
                                // Try to locate a cell
                                DataGridViewCell c = null;

                                // Loop through each cell, looking for the right column
                                for (int i = uaSortOrder.Index + 1; i < grdUnapproved.Columns.Count; i++)
                                {
                                    if (grdUnapproved.Columns[i].HeaderText == gi.GroupRule.shortname)
                                    {
                                        // Got it - note it and break
                                        c = r.Cells[i];
                                        break;
                                    }
                                }

                                if (c == null)
                                {
                                    // Couldn't find cell - print some debugging information
                                    Debug.WriteLine("Couldn't find DataGridViewCell for group {0} ({1})", gi.GroupRule.shortname, "uag" + gi.GroupRule.shortname.Replace(' ', '_'));
                                    break;
                                }

                                // Has the update been approved?
                                if (gi.Approved.HasValue)
                                {
                                    // Yes - cell value should be "Approved".  Tooltip should show when update was approved.
                                    c.Value = "Approved";
                                    c.ToolTipText = string.Format("Approved {0}", gi.Approved.Value.ToLocalTime().ToString("ddd dMMMyy h:mm:sstt"));
                                }
                                else
                                {
                                    // Is the update approvable?
                                    if (gi.UpdateApprovableNow)
                                    {
                                        // Yes, show the number of PCs requiring the update.  No tooltip required.
                                        c.Value = gi.PCs.ToString();
                                        c.ToolTipText = "";
                                    }
                                    else
                                    {
                                        // Will the update be approvable in the future?
                                        if (gi.Approvable.HasValue)
                                        {
                                            // Yes.  Cell can be blank, tooltip should indicate when the update will be installable.
                                            c.Value = "Waiting";
                                            c.ToolTipText = string.Format("Update will be approvable on {0}", gi.Approvable.Value.ToLocalTime().ToString("ddd dMMMyy h:mm:sstt"));
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // Loop through all rows, removing any that hasn't been updated
                    for (int i = 0; i < grdUnapproved.Rows.Count; )
                    {
                        // Has this row been updated?
                        if (grdUnapproved.Rows[i].Tag.ToString() == "N")
                            // No - delete it.
                            grdUnapproved.Rows.RemoveAt(i);
                        else
                            // Yes, look at the next row
                            i++;
                    }

                    // Sort datagrid
                    uaSortOrder.SortMode = DataGridViewColumnSortMode.Automatic;
                    grdUnapproved.Sort(uaSortOrder, ListSortDirection.Ascending);

                    // Ensure UpdateName column isn't too wide (a maximum of a quarter of the window's width)
                    grdUnapproved.Columns[uaUpdateName.Index].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    Application.DoEvents();

                    if (grdUnapproved.Columns[uaUpdateName.Index].Width > (this.Width / 4))
                    {
                        grdUnapproved.Columns[uaUpdateName.Index].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        grdUnapproved.Columns[uaUpdateName.Index].Width = this.Width / 4;
                    }

                    // Show total number of updates
                    lblUpdatesToApprove.Text = grdUnapproved.Rows.Count.ToString() + " update(s)";

                    // Since this timer is active, ticks should occur every 15 seconds...
                    timUpdateData.Interval = 15000;

                    // Re-enabling drawing of datagrid and note time of datagrid update
                    ResumeDrawing(grdUnapproved);
                    lastupdaterun = DateTime.Now;
                }
            }
        }

        private DataGridViewRow FindUnapprovedUpdateRow(string updateid)
        {
            // Loop through each row in the datagrid, looking for an appropriate row
            foreach (DataGridViewRow r in grdUnapproved.Rows)
                if (r.Cells[uaID.Index].Value != null && r.Cells[uaID.Index].Value.ToString() == updateid)
                    // Found one - return it
                    return r;

            // Didn't find a row - return a newly added row
            return grdUnapproved.Rows[grdUnapproved.Rows.Add()];
        }

        private void EndpointUpdateUngroupedComputers()
        {
            // Declare some variables
            int rn;

            if (CheckDBConnection())
            {
                // Update the data grid
                DataTable d = wsus.GetUnassignedComputers();

                foreach (DataRow dr in d.Rows)
                {
                    // Locate an existing matching row
                    rn = -1;

                    foreach (DataGridViewRow dgr in grdEndpoints.Rows)
                    {
                        if (dgr.Cells["epName"].Value.ToString() == dr["name"].ToString() && dgr.Cells["epFault"].Value.ToString() == "Not Assigned to a Group")
                        {
                            rn = dgr.Index;
                            break;
                        }
                    }

                    // If no row is located, create a new one and set the last updated time to blank
                    if (rn == -1) rn = grdEndpoints.Rows.Add();

                    DataGridViewRow r = grdEndpoints.Rows[rn];

                    r.Cells["epName"].Value = dr["name"].ToString();
                    r.Cells["epIP"].Value = dr["ipaddress"].ToString();
                    r.Cells["epFault"].Value = "Not Assigned to a Group";

                    r.Cells["epUpdate"].Value = "Y";
                }
            }
        }

        private void EndpointUpdateNotCommunicating()
        {
            int rn;

            // Get list of computers that hasn't updated in the last week
            ComputerTargetScope cs = new ComputerTargetScope();
            cs.ToLastSyncTime = DateTime.Now.AddDays(-7);
            cs.IncludeDownstreamComputerTargets = true;

            // Update grid
            foreach (IComputerTarget c in wsus.server.GetComputerTargets(cs))
            {
                // Look for an existing row
                rn = -1;

                foreach (DataGridViewRow fr in grdEndpoints.Rows)
                {
                    if (fr.Cells["epName"].Value.ToString() == c.FullDomainName && fr.Cells["epFault"].Value.ToString() == "Not Communicating")
                    {
                        rn = fr.Index;
                        break;
                    }
                }

                // If no existing row found, add a new one
                if (rn == -1) rn = grdEndpoints.Rows.Add();

                DataGridViewRow r = grdEndpoints.Rows[rn];

                r.Cells["epName"].Value = c.FullDomainName;
                r.Cells["epIP"].Value = c.IPAddress.ToString();
                r.Cells["epLastServerContact"].Value = c.LastSyncTime.ToString("dd-MMM-yyyy h:mm");
                r.Cells["epLastStatus"].Value = c.LastSyncResult;
                r.Cells["epFault"].Value = "Not Communicating";

                r.Cells["epUpdate"].Value = "Y";
            }
        }

        private void UpdateWSUSNotCommunicating()
        {
            // Reset forced update flag
            forceUpdate = false;

            // Show working box
            gbxWorking.Visible = true;
            this.Refresh();

            // Clear grid and populate with downstream server information
            grdWSUSNotCommunicting.Rows.Clear();

            foreach (IDownstreamServer s in wsus.server.GetDownstreamServers())
            {
                if (s.LastSyncTime < DateTime.Now.ToUniversalTime().AddHours(-24))
                {
                    int rn = grdWSUSNotCommunicting.Rows.Add();
                    DataGridViewRow r = grdWSUSNotCommunicting.Rows[rn];

                    r.Cells["wnuServerName"].Value = s.FullDomainName;
                    r.Cells["wnuLastSync"].Value = s.LastSyncTime.ToString("dd-MMM-yyyy h:mmtt");
                    r.Cells["wnuLastRollup"].Value = s.LastRollupTime.ToString("dd-MMM-yyyy h:mmtt");
                }
            }

            // Update time for this tab is 60 seconds
            timUpdateData.Interval = 60 * 60 * 1000;
        }

        private void AlternateRowColour(DataGridView g)
        {
            // Alternate the row's background colour to make viewing easier
            foreach (DataGridViewRow r in g.Rows)
            {
                if (r.Index % 2 == 0)
                    r.DefaultCellStyle.BackColor = Color.Empty;
                else
                    r.DefaultCellStyle.BackColor = Color.LightGray;
            }
        }

        private void EndpointUpdateDefaultSusID()
        {
            int rn;

            foreach (string susid in cfg.DefaultSusIDCollection)
            {
                try
                {
                    IComputerTarget c = wsus.server.GetComputerTarget(susid);

                    // Locate an existing matching row
                    rn = -1;

                    foreach (DataGridViewRow dgr in grdEndpoints.Rows)
                    {
                        if (dgr.Cells["epName"].Value.ToString() == c.FullDomainName.ToString() && dgr.Cells["epFault"].Value.ToString() == "Default SUS ID")
                        {
                            rn = dgr.Index;
                            break;
                        }

                    }

                    // If no row is located, create a new one
                    if (rn == -1) rn = grdEndpoints.Rows.Add();

                    DataGridViewRow r = grdEndpoints.Rows[rn];

                    // Fill in data grid
                    r.Cells["epName"].Value = c.FullDomainName.ToString();
                    r.Cells["epIP"].Value = c.IPAddress.ToString();
                    r.Cells["epLastServerContact"].Value = c.LastReportedStatusTime.ToString("dd-MMM-yyyy h:mm");
                    r.Cells["epLastStatus"].Value = c.LastSyncResult.ToString();
                    r.Cells["epFault"].Value = "Default SUS ID";

                    // Tag the row as updated
                    r.Cells["epUpdate"].Value = "Y";
                }
                catch (WsusObjectNotFoundException)
                {
                    // No PC found, no action required
                }
            }
        }

        private void EndpointIncorrectGroupUpdate()
        {
            int rn;
            
            // Get the group rules and sort by priority
            clsConfig.ComputerGroupRegexCollection rules = cfg.ComputerRegExList;
            rules.SortByPriority();

            if (CheckDBConnection())
            {
                // Update the data grid;
                DataTable t = wsus.GetComputerGroups();

                foreach (DataRow d in t.Rows)
                {
                    // Attempt to match the computer to a rule
                    clsConfig.ComputerGroupRegEx rx = rules.MatchPC(d["name"].ToString(), d["ipaddress"].ToString());

                    // Do we have a match?
                    if (rx != null)
                    {
                        // Yes - retreive the computer's group
                        Debug.WriteLine(DateTime.Now.ToString("h:mm:ss.ff ") + d["name"].ToString() + ": Retreiving group information");

                        // Does it match the computer group we have?
                        if (d["groupname"].ToString() != rx.ComputerGroup)
                        {
                            // No - add it.

                            // Locate an existing matching row
                            rn = -1;

                            foreach (DataGridViewRow dgr in grdEndpoints.Rows)
                            {
                                if (dgr.Cells["epName"].Value.ToString() == d["name"].ToString() && dgr.Cells["epFault"].Value.ToString() == "Incorrect Computer Group")
                                {
                                    rn = dgr.Index;
                                    break;
                                }
                            }

                            // If no row is located, create a new one
                            if (rn == -1) rn = grdEndpoints.Rows.Add();

                            DataGridViewRow r = grdEndpoints.Rows[rn];

                            r.Cells["epName"].Value = d["name"].ToString();
                            r.Cells["epIP"].Value = d["ipaddress"].ToString();
                            r.Cells["epComputerGroup"].Value = rx.ComputerGroup;
                            r.Cells["epComputerGroup"].ToolTipText = "Is currently in " + d["groupname"].ToString();
                            r.Cells["epFault"].Value = "Incorrect Computer Group";

                            // Tag the row as updated
                            r.Cells["epUpdate"].Value = "Y";
                        }
                    }
                }
            }
        }

        private void EndpointUpdateApproved()
        {
            int rn;

            if (CheckDBConnection())
            {
                // Retreive the list of approved updates that have not yet been applied.
                DataTable t = wsus.GetApprovedUpdates();

                foreach (DataRow d in t.Rows)
                {
                    // Locate an existing matching row
                    rn = -1;

                    foreach (DataGridViewRow dgr in grdEndpoints.Rows)
                    {
                        if (dgr.Cells["epName"].Value.ToString() == d["fulldomainname"].ToString() && dgr.Cells["epFault"].Value.ToString() == "Uninstalled Approved Updates")
                        {
                            rn = dgr.Index;
                            break;
                        }
                    }
                    
                    // If no row is located, create a new one
                    if (rn == -1) rn = grdEndpoints.Rows.Add();

                    DataGridViewRow r = grdEndpoints.Rows[rn];

                    // Fill in data grid
                    r.Cells["epName"].Value = d["fulldomainname"].ToString();
                    r.Cells["epIP"].Value = d["ipaddress"].ToString();
                    r.Cells["epApprovedUpdates"].Value = int.Parse(d["approvedupdates"].ToString());
                    r.Cells["epLastServerContact"].Value = DateTime.Parse(d["lastsynctime"].ToString()).ToString("dd-MMM-yyyy h:mm");
                    r.Cells["epFault"].Value = "Uninstalled Approved Updates";

                    // Tag the row as updated
                    r.Cells["epUpdate"].Value = "Y";
                }
            }
        }

        private void EndpointUpdateErrors()
        {
            int rn;

            if (CheckDBConnection())
            {
                // Update the data grid;
                DataTable t = wsus.GetUpdateErrors();

                foreach (DataRow d in t.Rows)
                {
                    // Locate an existing matching row
                    rn = -1;

                    foreach (DataGridViewRow dgr in grdEndpoints.Rows)
                    {
                        if (dgr.Cells["epName"].Value.ToString() == d["fulldomainname"].ToString() && dgr.Cells["epFault"].Value.ToString() == "Updates With Errors")
                        {
                            rn = dgr.Index;
                            break;
                        }
                    }

                    // If no row is located, create a new one and set the last updated time to blank
                    if (rn == -1) rn = grdEndpoints.Rows.Add();

                    DataGridViewRow r = grdEndpoints.Rows[rn];

                    // Fill in data grid
                    r.Cells["epName"].Value = d["fulldomainname"].ToString();
                    r.Cells["epIP"].Value = d["ipaddress"].ToString();
                    r.Cells["epUpdateErrors"].Value = int.Parse(d["updateerrors"].ToString());
                    r.Cells["epLastServerContact"].Value = DateTime.Parse(d["lastsynctime"].ToString()).ToString("dd-MMM-yyyy h:mm");
                    r.Cells["epFault"].Value = "Updates With Errors";

                    // Tag the row as updated
                    r.Cells["epUpdate"].Value = "Y";
                }
            }
        }

        private void UpdateSupercededUpdates()
        {
            // Show "refreshing" notification
            gbxWorking.Visible = true;
            this.Refresh();

            // Get list of updates and update IDs for superceded updates
            DataTable t = wsus.GetSupercededUpdates();

            // Empty the grid and refill it.
            grdSupercededUpdates.Rows.Clear();

            foreach (DataRow d in t.Rows)
            {
                DataGridViewRow r = grdSupercededUpdates.Rows[grdSupercededUpdates.Rows.Add()];

                r.Cells["suUpdateName"].Value = d["defaulttitle"].ToString();
                r.Cells["suUpdateID"].Value = d["updateid"].ToString();

                DataGridViewCheckBoxCell c = (DataGridViewCheckBoxCell)r.Cells["suSelect"];
                c.Value = false;
            }

            // Show number of updates
            lblUpdateCount.Text = grdSupercededUpdates.Rows.Count.ToString() + " update(s)";

            // Reset timer to trigger again only after 15 minutes.  Other activity will trigger updates.
            timUpdateData.Interval = 15 * 60 * 1000;
        }

        private void UpdateServerReboots()
        {
            if (DateTime.Now.Subtract(lastupdaterun).Minutes > 5 || forceUpdate)
            {
                // Show updating message
                gbxWorking.Visible = true;
                this.Refresh();

                // Reset last run time
                lastupdaterun = DateTime.Now;

                // Try to get a list of computers
                ComputerTargetScope cs = new ComputerTargetScope();
                cs.IncludedInstallationStates = UpdateInstallationStates.InstalledPendingReboot;
                cs.IncludeDownstreamComputerTargets = true;
                ComputerTargetCollection comp = wsus.server.GetComputerTargets(cs);

                // Clear the list of servers and update
                lstServers.Items.Clear();

                foreach (IComputerTarget c in comp)
                {
                    // Is this a Windows Server?
                    if (c.OSDescription.ToUpper().Contains("SERVER"))
                    {
                        // Yep - include it
                        lstServers.Items.Add(c.FullDomainName);
                    }
                }

                // Reset update interval to 5 minutes
                timUpdateData.Interval = 5 * 60 * 1000;
            }
        }

        private void ShowTabs(bool show)
        {
            foreach (TabPage t in tabAdminType.TabPages)
            {
                if (t.Name != tabHome.Name) ((Control)t).Enabled = show;
            }

            timUpdateData.Enabled = show;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // Let the user know something is happening
            Cursor.Current = Cursors.WaitCursor;

            // Link status items to WSUS class
            lblSQLStatus.Text = wsus.dbStatus;
            lblWSUSStatus.Text = wsus.wsusStatus;

            if (wsus.server != null)
                ShowTabs(true);
            else
                ShowTabs(false);

            // Return window to it's saved location and state
            this.Location = cfg.WindowLocation;
            this.Size = cfg.WindowSize;

            if (cfg.WindowState == FormWindowState.Maximized.ToString()) this.WindowState = FormWindowState.Maximized;
            if (cfg.WindowState == FormWindowState.Normal.ToString()) this.WindowState = FormWindowState.Normal;
            if (cfg.WindowState == FormWindowState.Minimized.ToString()) this.WindowState = FormWindowState.Minimized;


            // Turn on timer if both SQL and WSUS are connected
            if (lblSQLStatus.Text == "OK" && lblWSUSStatus.Text == "OK")
            {
                timUpdateData.Interval = 500;
                timUpdateData.Enabled = true;
            }

            // Restore Endpoint selections
            BitArray ba = new BitArray(new int[] { cfg.EndpointSelections });

            butCheckClick(butApproved, ba[0]);
            butCheckClick(butNotCommunicating, ba[1]);
            butCheckClick(butUnassigned, ba[2]);
            butCheckClick(butUpdateErrors, ba[3]);
            butCheckClick(butDefaultSusID, ba[4]);
            butCheckClick(butGroupRules, ba[5]);

            // Set up and start the background pinger
            wrkPinger.DoWork += new DoWorkEventHandler(PingWorker);
            wrkPinger.RunWorkerAsync();

            wrkSUSID.DoWork += new DoWorkEventHandler(wrkSUSID_DoWork);

            // Initialise task collection
            tasks = new TaskCollection(cfg);
            tasks.TaskRun += tasks_TaskRun;
            // Bind the task list to grdTasks
            grdTasks.AutoGenerateColumns = false;
            grdTasks.DataSource = tasks.Tasks;

            // Bind to datagrid and configure row properties
            tskID.DataPropertyName = "TaskID";
            tskStatus.DataPropertyName = "CurrentStatus";
            tskIP.DataPropertyName = "IPAddress";
            tskCommand.DataPropertyName = "Command";
            tskOutput.DataPropertyName = "Output";
            
            tskOutput.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            grdEndpoints.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            // Return cursor to normal
            Cursor.Current = Cursors.Arrow;
        }

        void tasks_TaskRun(object sender, EventArgs e)
        {
            Task t = (Task)sender;

            // Find active task and select it
            DataGridViewRow r = null;

            foreach (DataGridViewRow gr in grdTasks.Rows)
                if (gr.Cells[tskID.Index].Value != null && gr.Cells[tskID.Index].Value.ToString() == t.TaskID.ToString())
                    r = gr;

            // Did we find a row?
            if (r != null)
                // Yes - select it
                grdTasks.CurrentCell = r.Cells[tskID.Index];
        }

        void tasks_ListChanged(object sender, ListChangedEventArgs e)
        {
            Debug.WriteLine("Tasks list changed: Type {0},  Property {1}", e.ListChangedType.ToString(), e.PropertyDescriptor);
        }

        private void frmMain_Closing(object sender, FormClosingEventArgs e)
        {
            // Disable timer to ensure no further ticks happen
            timUpdateData.Enabled = false;

            // Save window location
            SaveWindowLocation();

            Application.Exit();
        }

        private void SaveWindowLocation()
        {
            // Save the window state and (if maximized) normalise window so it will save in the right location
            cfg.WindowState = this.WindowState.ToString();

            // Save the window size and location.
            if (this.WindowState == FormWindowState.Normal)
                cfg.WindowSize = this.Size;
            else
                cfg.WindowSize = this.RestoreBounds.Size;

            cfg.WindowLocation = this.Location;

            // Save Endpoint selections
            int eps = 0;

            if (butApproved.Checked) eps += 1;
            if (butNotCommunicating.Checked) eps += 2;
            if (butUnassigned.Checked) eps += 4;
            if (butUpdateErrors.Checked) eps += 8;
            if (butDefaultSusID.Checked) eps += 16;
            if (butGroupRules.Checked) eps += 32;

            cfg.EndpointSelections = eps;
        }

        private void tabAdminType_Selecting(object sender, TabControlCancelEventArgs e)
        {
            // If the refresh tab is selected, cancel the selection.  A refresh of the tab will take place because next we will...
            if (e.TabPage.Text.ToString() == "Refresh")
            {
                e.Cancel = true;
            }

            // Trigger an update on all tab changes
            ForceUpdate(100);
        }

        private void ForceUpdate(int delay)
        {
            forceUpdate = true;
            timUpdateData.Interval = delay;
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            gbxWorking.Left = (this.Width / 2) - (gbxWorking.Width / 2);
            gbxWorking.Top = (this.Height / 2) - (gbxWorking.Height / 2);
        }

        private void butSelectAll_Click(object sender, EventArgs e)
        {
            // Check all items
            foreach (DataGridViewRow r in grdSupercededUpdates.Rows)
            {
                DataGridViewCheckBoxCell c = (DataGridViewCheckBoxCell)r.Cells["suSelect"];
                c.Value = true;
            }
        }

        private void butSelectNone_Click(object sender, EventArgs e)
        {
            // Uncheck all items
            foreach (DataGridViewRow r in grdSupercededUpdates.Rows)
            {
                DataGridViewCheckBoxCell c = (DataGridViewCheckBoxCell)r.Cells["suSelect"];
                c.Value = false;
            }
        }

        private void butDeclineSelected_Click(object sender, EventArgs e)
        {
            // Loop through all updates, declining those that were selected
            for (int i = 0; i < grdSupercededUpdates.Rows.Count; )
            {
                DataGridViewRow r = grdSupercededUpdates.Rows[i];

                // Is update checked?
                if ((bool)r.Cells["suSelect"].Value == false)
                {
                    // No, skip to the next update
                    i++;
                }
                else
                {
                    // Yes - decline update
                    UpdateRevisionId ur = new UpdateRevisionId();
                    ur.UpdateId = new Guid(r.Cells["suUpdateID"].Value.ToString());
                    IUpdate u = wsus.server.GetUpdate(ur);

                    u.Decline();

                    grdSupercededUpdates.Rows.Remove(r);
                    this.Refresh();
                }
            }

            // Trigger update of dialog box
            timUpdateData.Interval = 100;
        }

        private void ShowCancelApproveButton(bool show)
        {
            if (show)
            {
                // We're supposed to show the cancel button, which also implies disabling the approve and decline buttons, disabling the timer and resetting the
                // Cancel Now flag
                btnUACancel.Visible = true;
                btnUAApprove.Enabled = false;
                btnUADecline.Enabled = false;

                timUpdateData.Enabled = false;
                cancelNow = false;
                this.Refresh();
            }
            else
            {
                // We're supposed to hide the cancel button, which also implies enabling the approve and decline buttons and the timer
                btnUACancel.Visible = false;
                btnUAApprove.Enabled = true;
                btnUADecline.Enabled = true;

                timUpdateData.Enabled = true;
                this.Refresh();
            }
        }

        private void butCancelApprove_Click(object sender, EventArgs e)
        {
            cancelNow = true;
        }

        private void butCheckClick(ToolStripButton but)
        {
            // Toggle button state
            if (but.Checked)
            {
                but.Checked = false;
                but.Image = Properties.Resources.BuilderDialog_delete;
            }
            else
            {
                but.Checked = true;
                but.Image = Properties.Resources.SuccessComplete;
            }

            EndpointShowHideColumns();
        }

        private void butCheckClick(ToolStripButton but, bool state)
        {
            if (state)
            {
                but.Checked = true;
                but.Image = Properties.Resources.SuccessComplete;
            }
            else
            {
                but.Checked = false;
                but.Image = Properties.Resources.BuilderDialog_delete;
            }

            EndpointShowHideColumns();
        }

        private void EndpointShowHideColumns()
        {
            // Show and hide columns as appropriate for selections
            epApprovedUpdates.Visible = butApproved.Checked;
            epUpdateErrors.Visible = butUpdateErrors.Checked;
            epLastStatus.Visible = butNotCommunicating.Checked;
            epComputerGroup.Visible = butGroupRules.Checked;

            // Trigger an update after a second to allow end-user to change other selections
            forceUpdate = true;
            timUpdateData.Interval = 1000;
        }

        private void butApproved_Click(object sender, EventArgs e)
        {
            butCheckClick(butApproved);
        }

        private void butNotCommunicating_Click(object sender, EventArgs e)
        {
            butCheckClick(butNotCommunicating);
        }

        private void butUnassigned_Click(object sender, EventArgs e)
        {
            butCheckClick(butUnassigned);
        }

        private void butUpdateErrors_Click(object sender, EventArgs e)
        {
            butCheckClick(butUpdateErrors);
        }

        private void grdEndpoints_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            // Create some useful row variables
            DataGridViewRow r1 = grdEndpoints.Rows[e.RowIndex1];
            DataGridViewRow r2 = grdEndpoints.Rows[e.RowIndex2];

            // Check ping results - anything other than a successful ping result should be equal
            bool p1;
            bool p2;

            if (r1.Cells["epPing"].Value == null || r1.Cells["epPing"].Value.ToString() == "" || r1.Cells["epPing"].Value.ToString() == "No Response")
                p1 = false;
            else
                p1 = true;

            if (r2.Cells["epPing"].Value == null || r2.Cells["epPing"].Value.ToString() == "" || r2.Cells["epPing"].Value.ToString() == "No Response")
                p2 = false;
            else
                p2 = true;

            // Are the two ping results the same
            if (p1 == p2)
            {
                // If the two IPs are the same, sort by fault
                if (r1.Cells["epIP"].Value.ToString() == r2.Cells["epIP"].Value.ToString())
                {
                    e.SortResult = System.String.Compare(r1.Cells["epFault"].Value.ToString(), r2.Cells["epFault"].Value.ToString());
                    e.Handled = true;
                }
                else
                {
                    e.SortResult = System.String.Compare(r1.Cells["epIP"].Value.ToString(), r2.Cells["epIP"].Value.ToString());
                    e.Handled = true;
                }
            }
            else if (!p1  && p2)
            {
                // Cell 2 should come first since only it has a successful ping
                e.SortResult = 1;
                e.Handled = true;
            }
            else if (p1 && !p2)
            {
                // Cell 1 should come first since only it has a successful ping
                e.SortResult = -1;
                e.Handled = true;
            }
        }

        private void butDefaultSusID_Click(object sender, EventArgs e)
        {
            butCheckClick(butDefaultSusID);
        }

        private void butGroupRules_Click(object sender, EventArgs e)
        {
            butCheckClick(butGroupRules);
        }

        private void mnuWSUSServer_Click(object sender, EventArgs e)
        {
            Form f = new frmWSUSConfig(cfg);
            f.ShowDialog();
        }

        private void mnuComputerGroupRules_Click(object sender, EventArgs e)
        {
            Form f = new frmComputerGroupRules(cfg);
            f.ShowDialog();
        }

        private void mnuDefaultSusIDList_Click(object sender, EventArgs e)
        {
            Form f = new frmDefaultSUS(cfg);
            f.Show();
        }

        private void mnuPreferences_Click(object sender, EventArgs e)
        {
            Form f = new frmPreferences(cfg);
            f.ShowDialog();
        }

        private void mnuCredentials_Click(object sender, EventArgs e)
        {
            Form f = new frmCredentials(cfg);
            f.ShowDialog();
        }

        private void mnuGroupApprovalRules_Click(object sender, EventArgs e)
        {
            Form f = new frmGroupUpdateRules(cfg, wsus);
            f.ShowDialog();
        }

        DataGridViewRow epcmRow;
        IPAddress epcmIPAddress;
        clsConfig.SecurityCredential epcmCreds;
        string epcmFullName;

        private void grdEndpoints_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Was this a right-click?
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                // Yes - save cell context, select cell and show pop-up menu
                epcmRow = grdEndpoints.Rows[e.RowIndex];
                epcmIPAddress = IPAddress.Parse(epcmRow.Cells["epIP"].Value.ToString());
                epcmFullName = epcmRow.Cells[epName.Index].Value.ToString();
                grdEndpoints.CurrentCell = grdEndpoints.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Look for credentials for this PC
                clsConfig.CredentialCollection cc = cfg.CredentialList;
                epcmCreds = cc[epcmIPAddress];

                // Update menu to show details of the selected PC
                epDetails.Text = epcmRow.Cells[epName.Index].Value.ToString() + " at " + epcmIPAddress.ToString();

                // Show pop-up menu
                cmEndpoint.Show(Cursor.Position);
            }
        }

        private void epGPUpdateForce_Click(object sender, EventArgs e)
        {
            PSExecCall(epcmIPAddress, epcmFullName, epcmCreds, "gpupdate /force");
        }

        private void epGPUpdate_Click(object sender, EventArgs e)
        {
            PSExecCall(epcmIPAddress, epcmFullName, epcmCreds, "gpupdate");
        }

        private void epDetectNow_Click(object sender, EventArgs e)
        {
            PSExecCall(epcmIPAddress, epcmFullName, epcmCreds, "wuauclt /detectnow");
        }

        private void epReportNow_Click(object sender, EventArgs e)
        {
            PSExecCall(epcmIPAddress, epcmFullName, epcmCreds, "wuauclt /reportnow");
        }

        private void epResetSusID_Click(object sender, EventArgs e)
        {
            try
            {
                // Stop the Windows Update service
                Task stop = tasks.AddTask(epcmIPAddress, epcmFullName, epcmCreds, "net stop wuauserv");
                stop.TimeoutInterval = new TimeSpan(0, 1, 0);
                stop.Status = TaskStatus.Queued;

                // Delete the three registry keys only if the service successfully stops
                Task reg = tasks.AddTask(epcmIPAddress, epcmFullName, epcmCreds, "REG DELETE \"HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\WindowsUpdate\" /v AccountDomainSid /f");
                reg.DependsOnTask = stop;

                reg = tasks.AddTask(epcmIPAddress, epcmFullName, epcmCreds, "REG DELETE \"HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\WindowsUpdate\" /v PingID /f");
                reg.DependsOnTask = stop;

                reg = tasks.AddTask(epcmIPAddress, epcmFullName, epcmCreds, "REG DELETE \"HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\WindowsUpdate\" /v SusClientId /f");
                reg.DependsOnTask = stop;

                Task start = tasks.AddTask(epcmIPAddress, epcmFullName, epcmCreds, "net start wuauserv");
                start.TimeoutInterval = new TimeSpan(0, 1, 0);
                start.Status = TaskStatus.Queued;

                Task wu = tasks.AddTask(epcmIPAddress, epcmFullName, epcmCreds, "wuauclt /resetauthorization");
                wu.DependsOnTask = start;

                wu = tasks.AddTask(epcmIPAddress, epcmFullName, epcmCreds, "wuauclt /detectnow");
                wu.DependsOnTask = start;
            }
            catch (ConfigurationException ex)
            {
                // Could not schedule task - inform user of reason why
                MessageBox.Show(ex.Message, ex.Summary, MessageBoxButtons.OK);
            }
        }

        private void mnuResetAuth_Click(object sender, EventArgs e)
        {
            PSExecCall(epcmIPAddress, epcmFullName, epcmCreds, "wuauclt /resetauthorization");
        }

        /// <summary>
        /// Scheule a simple task with no dependent tasks
        /// </summary>
        private void PSExecCall(IPAddress ip, string computername, clsConfig.SecurityCredential credentials, string command)
        {
            try
            {
                Task task = tasks.AddTask(ip, computername, credentials, command);
                task.Ready();
            }
            catch (ConfigurationException ex)
            {
                // Could not schedule task - inform user of reason why
                MessageBox.Show(ex.Message, ex.Summary, MessageBoxButtons.OK);
            }
        }

        private void mnuSUSWatcher_Click(object sender, EventArgs e)
        {
            if (wrkSUSID.IsBusy)
            {
                MessageBox.Show("SUS ID Watcher already running", "Can't start SUS ID Watcher", MessageBoxButtons.OK);
                return;
            }

            wrkSUSID.RunWorkerAsync();
        }

        private void wrkSUSID_DoWork(object sender, DoWorkEventArgs e)
        {
            Form f = new SUSWatcher.frmSUSWatch();

            f.ShowDialog();
        }

        private void grdUnapproved_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Are we getting some dumb values?  Ignore event if so.
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            // Create some variables to make processing a bit easier.
            DataGridViewRow r = grdUnapproved.Rows[e.RowIndex];
            DataGridViewCell c = r.Cells[e.ColumnIndex];

            // Work out default colours for this cell
            Color fore = SystemColors.ControlText;
            Color back;

            if (e.RowIndex % 2 == 0)
                // Lighter highlight for even rows
                back = SystemColors.Control;
            else
                // Darker highlight for odd rows
                back = DarkenColour(SystemColors.Control);

            // Is this one of the group columns?
            if (e.ColumnIndex > uaSortOrder.Index)
            {
                int PCs;
                // Are the contents of this cell numeric?
                if (c.Value != null && int.TryParse(c.Value.ToString(), out PCs))
                {
                    // Looks like it - this is a PC count.  Highlight the cell
                    if (e.RowIndex % 2 == 0)
                        // Lighter highlight for even rows
                        back = Color.LightGreen;
                    else
                        // Darker highlight for odd rows
                        back = DarkenColour(Color.LightGreen);
                }
                else
                    // No - the text colour should be light
                    fore = MidColour(SystemColors.ControlText, back);
            }

            // Set cell colour
            c.Style.ForeColor = fore;
            c.Style.BackColor = back;
        }

        private Color MidColour(Color a, Color b)
        {
            // Calculate the colour that's halfway between the two provided colours
            return Color.FromArgb((a.R + b.R) / 2, (a.G + b.G) / 2, (a.B + b.B) / 2);
        }

        private Color DarkenColour(Color c)
        {
            return Color.FromArgb((int)(c.R * 0.8), (int)(c.G * 0.8), (int)(c.B * 0.8));
        }

        private void btnUAApprove_Click(object sender, EventArgs e)
        {
            // Show cancel button
            ShowCancelApproveButton(true);

            // Loop through each selected cell and see what to approve
            foreach (DataGridViewCell c in grdUnapproved.SelectedCells)
            {
                // Break out of the loop now if the cancel flag has been set
                if (cancelNow) break;

                // Only allow the update to be approved if some PCs require it (and if this is an update column)
                int PCs;

                if (c.ColumnIndex > uaSortOrder.Index && int.TryParse(c.Value.ToString(), out PCs))
                {
                    // Ensure update is visible so end user can see what's going on...
                    grdUnapproved.CurrentCell = c;
                    this.Refresh();

                    // Get the appropriate update object
                    UpdateRevisionId ur = new UpdateRevisionId();
                    ur.UpdateId = new Guid(grdUnapproved.Rows[c.RowIndex].Cells[uaID.Index].Value.ToString());
                    IUpdate u = wsus.server.GetUpdate(ur);

                    // Grab computer group object from column header
                    IComputerTargetGroup tg = (IComputerTargetGroup)grdUnapproved.Columns[c.ColumnIndex].Tag;

                    // If a valid group was found, approve the update
                    if (tg != null)
                    {
                        bool canapprove = true;

                        // Does the update require a EULA approval?
                        if (u.RequiresLicenseAgreementAcceptance)
                        {
                            // Get license agreement, check to see if the license has been agreed to. and approve it if it hasn't
                            ILicenseAgreement eula = u.GetLicenseAgreement();
                            if (!eula.IsAccepted)
                            {
                                // EULA requires approval - display it to the user and request approval.
                                if (MessageBox.Show(eula.Text, string.Format("{0} requires end-user license acceptance", u.Title), MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                                    // EULA accepted.  Mark acceptance.
                                    u.AcceptLicenseAgreement();
                                else
                                    // EULA rejected - update can't be approved
                                    canapprove = false;
                            }
                        }

                        // If update can be approved, do it.
                        if (canapprove)
                        {
                            u.Approve(UpdateApprovalAction.Install, tg);

                            // Empty cell and unselect it so the end user knows the update has been approved
                            c.Value = "Approved";
                            c.Selected = false;
                            this.Refresh();
                        }

                        // Process outstanding events to allow end user to cancel approvals if they want
                        Application.DoEvents();
                    }
                }
            }

            // Hide the cancel button, enable the approve button and the timer
            ShowCancelApproveButton(false);

            // Trigger update of unapproved updates
            timUpdateData.Interval = 100;
            forceUpdate = true;
        }

        private void btnUACancel_Click(object sender, EventArgs e)
        {
            cancelNow = true;
        }

        private void btnUADecline_Click(object sender, EventArgs e)
        {
            // Warn user that this will decline updates for *all* groups, even if the update is already approved
            if (MessageBox.Show("Declining updates affects ALL groups, not just the selected group!" + Environment.NewLine +
                "Proceeding will decline this update for all groups, even if the update has already been approved for other groups." + Environment.NewLine + Environment.NewLine +
                "Do you wish to proceed?", "WARNING", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                // User declined to proceed - return without declining any updates
                return;
            }

            // Show the cancel button
            ShowCancelApproveButton(true);

            // Loop through each selected cell and see what to decline
            foreach (DataGridViewCell c in grdUnapproved.SelectedCells)
            {
                // Break out of the loop now if the cancel flag has been set
                if (cancelNow) break;

                // Only allow the update to be declined if some PCs require it.
                if (c.Value.ToString() != "")
                {
                    // Ensure update is visible so end user can see what's going on...
                    grdUnapproved.CurrentCell = c;
                    this.Refresh();

                    // Get the appropriate update object
                    UpdateRevisionId ur = new UpdateRevisionId();
                    ur.UpdateId = new Guid(grdUnapproved.Rows[c.RowIndex].Cells[uaID.Index].Value.ToString());
                    IUpdate u = wsus.server.GetUpdate(ur);

                    // Decline the update and update the cell so the end user knows what's going on.
                    u.Decline();
                    c.Value = "Declined";

                    // Process outstanding events to allow end user to cancel approvals if they want
                    Application.DoEvents();
                }
            }

            // Hide the cancel button, enable the approve button and the timer
            ShowCancelApproveButton(false);

            // Trigger update of unapproved updates
            timUpdateData.Interval = 100;
            forceUpdate = true;
        }

        private void grdUnapproved_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Create handy variables referring to this cell
            DataGridViewRow r = grdUnapproved.Rows[e.RowIndex];
            DataGridViewCell c = r.Cells[e.ColumnIndex];
            DataGridViewColumn gc = grdUnapproved.Columns[e.ColumnIndex];

            // Was this a left click?
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                // Left click - check to see if KB column was selected - if it was, open link in default browser
                if (gc.HeaderText == "KB Article")
                    Process.Start("http://support.microsoft.com/kb/" + c.Value.ToString());

                // If the column selected isn't an update row, deselect it
                if (e.ColumnIndex <= uaSortOrder.Index)
                    c.Selected = false;
            }
        }
    }
}