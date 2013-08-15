using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Microsoft.Win32;
using Microsoft.UpdateServices.Administration;

namespace WSUSAdminAssistant
{
    public class clsConfig
    {
        public clsConfig()
        {
            // Set application in registry.  This has the effect of creating the registry key should it not already exist
            Registry.SetValue(regKey, "Application", "WSUS Administration Assistant", RegistryValueKind.String);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Useful private variables
        private string regKey = @"HKEY_CURRENT_USER\Software\WSUSAdminAssistant";

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // WSUS Configuration methods
        public string WSUSServer
        {
            get { return (string)Registry.GetValue(regKey, "WSUSServer", (string)"localhost"); }
            set { Registry.SetValue(regKey, "WSUSServer", value, RegistryValueKind.String); }
        }

        public bool WSUSSecureConnection
        {
            get { return Convert.ToBoolean((int)Registry.GetValue(regKey, "WSUSSecureConnection", Convert.ToInt32(false))); }
            set { Registry.SetValue(regKey, "WSUSSecureConnection", Convert.ToInt32(value), RegistryValueKind.DWord); }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Database Configuration methods
        public string DBServer
        {
            get { return (string)Registry.GetValue(regKey, "DBServer", "localhost"); }
            set { Registry.SetValue(regKey, "DBServer", value, RegistryValueKind.String); }
        }

        public string DBDatabase
        {
            get { return (string)Registry.GetValue(regKey, "DBDatabase", "SUSDB"); }
            set { Registry.SetValue(regKey, "DBDatabase", value, RegistryValueKind.String); }
        }

        public bool DBIntegratedAuth
        {
            get { return Convert.ToBoolean((int)Registry.GetValue(regKey, "DBIntegratedAuth", Convert.ToInt32(true))); }
            set { Registry.SetValue(regKey, "DBIntegratedAuth", Convert.ToInt32(value), RegistryValueKind.DWord); }
        }

        public string DBUsername
        {
            get { return (string)Registry.GetValue(regKey, "DBUsername", ""); }
            set { Registry.SetValue(regKey, "DBUsername", value, RegistryValueKind.String); }
        }

        public string DBPassword
        {
            get { return (string)Registry.GetValue(regKey, "DBPassword", ""); }
            set { Registry.SetValue(regKey, "DBPassword", value, RegistryValueKind.String); }
        }

        public string SQLConnectionString()
        {
            if (this.DBIntegratedAuth)
                return "database=" + this.DBDatabase + ";" + "server=" + this.DBServer + ";Trusted_Connection=true";
            else
                return "database=" + this.DBDatabase + ";" + "server=" + this.DBServer + ";" + "User ID=" + this.DBUsername + ";Password=" + this.DBPassword;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // UI Settings methods
        public System.Drawing.Point WindowLocation
        {
            get { return new System.Drawing.Point((int)Registry.GetValue(regKey, "WindowLocationX", 0),(int)Registry.GetValue(regKey, "WindowLocationY", 0)); }
            set
            {
                Registry.SetValue(regKey, "WindowLocationX", value.X, RegistryValueKind.DWord);
                Registry.SetValue(regKey, "WindowLocationY", value.Y, RegistryValueKind.DWord);
            }
        }

        public System.Drawing.Size WindowSize
        {
            get { return new System.Drawing.Size((int)Registry.GetValue(regKey, "WindowSizeWidth", 640), (int)Registry.GetValue(regKey, "WindowSizeWidth", 480)); }
            set
            {
                Registry.SetValue(regKey, "WindowSizeWidth", value.Width, RegistryValueKind.DWord);
                Registry.SetValue(regKey, "WindowSizeHeight", value.Height, RegistryValueKind.DWord);
            }
        }

        public string WindowState
        {
            get { return (string)Registry.GetValue(regKey, "WindowState", ""); }
            set { Registry.SetValue(regKey, "WindowState", value, RegistryValueKind.String); }
        }

        public int EndpointSelections
        {
            get { return (int)Registry.GetValue(regKey, "EndpointSelections", 255); }
            set { Registry.SetValue(regKey, "EndpointSelections", value, RegistryValueKind.DWord); }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Default SUD ID methods
        public string[] DefaultSusIDCollection
        {
            get { return ReadSusIdXML(this.SusIdXmlFile); }
            set { WriteSusIdXML(value, this.SusIdXmlFile); }
        }

        public string SusIdXmlFile
        {
            get
            {
                string xmlpath = (string)Registry.GetValue(regKey, "SusIdXmlFile", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                xmlpath += "\\" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + "\\DefaultSusIds.xml";


                // Check if XML file exists
                if (File.Exists(xmlpath))
                {
                    // It exists - return path
                    return xmlpath;
                }
                else
                {
                    // It doesn't exist - try to create it
                    try
                    {
                        // Does the directory exist?
                        if (!Directory.Exists(Path.GetDirectoryName(xmlpath)))
                            // No it doesn't - try and create it
                            Directory.CreateDirectory(Path.GetDirectoryName(xmlpath));

                        WriteSusIdXML(new string[0], xmlpath);

                        // If we get here, it wrote successfully, so return the file
                        return xmlpath;
                    }
                    catch
                    {
                        // Create failed - return no path
                        return "";
                    }
                }
            }

            set { Registry.SetValue(regKey, "SusIdXmlFile", value, RegistryValueKind.String); }
        }

        private void WriteSusIdXML(string[] s, string file)
        {
            // Create XML file (overwriting it if it already exists)
            using (FileStream fs = new FileStream(file, FileMode.Create))
            {
                XmlSerializer xs = new XmlSerializer(typeof(string[]));
                xs.Serialize(fs, s);
            }
        }

        private string[] ReadSusIdXML(string file)
        {
            // Open the XML file and read the config.  Let the exceptions fly for other procedures to handle
            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                string[] s;
                XmlSerializer xs = new XmlSerializer(typeof(string[]));
                s = (string[])xs.Deserialize(fs);
                return s;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Computer Group Regex matching methods
        public class ComputerGroupRegEx
        {
            public int Priority;
            public string ComputerNameRegex;
            public string IPRegex;
            public string ComputerGroup;
            public string Comment;
            public bool Enabled;
        }

        public class ComputerGroupRegexCollection : System.Collections.CollectionBase
        {
            public ComputerGroupRegEx this[int index]
            {
                get { return (ComputerGroupRegEx) this.List[index]; }
                set { this.List[index] = value;}
            }

            public int Add(ComputerGroupRegEx add)
            {
                // Add item to list
                return this.List.Add(add);
            }

            public void Remove(int index)
            {
                // Ensure the index number falls within the correct range
                if (index > this.Count - 1 || index < 0)
                    // Out of bounds
                    throw new IndexOutOfRangeException();
                else
                    // Remove item from list
                    this.List.RemoveAt(index);
            }

            public void SortByPriority()
            {
                IComparer sorter = new PrioritySortHelper();
                InnerList.Sort(sorter);
            }

            public class PrioritySortHelper : System.Collections.IComparer
            {
                public int Compare(object x, object y)
                {
                    ComputerGroupRegEx r1 = (ComputerGroupRegEx)x;
                    ComputerGroupRegEx r2 = (ComputerGroupRegEx)y;

                    if (r1.Priority > r2.Priority) return 1;
                    if (r1.Priority < r2.Priority) return -1;

                    // Items are identical
                    return 0;
                }
            }

            public ComputerGroupRegEx MatchPC(string fulldomainname, string ip)
            {
                ComputerGroupRegEx rx = null;

                foreach (clsConfig.ComputerGroupRegEx r in this)
                {
                    // Assume the rules match unless one of the two regex expressions is non-empty and does not match or the rule is not enabled
                    bool matched = true;

                    if (!r.Enabled) matched = false;

                    if (r.ComputerNameRegex != null && r.ComputerNameRegex != "")
                    {
                        Match m = Regex.Match(fulldomainname, r.ComputerNameRegex);
                        if (!m.Success) matched = false;
                    }

                    if (r.IPRegex != null && r.IPRegex != "")
                    {
                        Match m = Regex.Match(ip, r.IPRegex);
                        if (!m.Success) matched = false;
                    }

                    if (matched)
                    {
                        // If we have a match, note it and break out
                        rx = r;
                        break;
                    }
                }

                // Return the rule that was matched (or null if no rule was matched)
                return rx;
            }
        }

        public string ComputerRegExXMLFile
        {
            get
            {
                string xmlpath = (string)Registry.GetValue(regKey, "ComputerRegExXMLFile", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                xmlpath += "\\" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + "\\ComputerRegEx.xml";


                // Check if XML file exists
                if (File.Exists(xmlpath))
                {
                    // It exists - return path
                    return xmlpath;
                }
                else
                {
                    // It doesn't exist - try to create it
                    try
                    {
                        // Does the directory exist?
                        if (!Directory.Exists(Path.GetDirectoryName(xmlpath)))
                            // No it doesn't - try and create it
                            Directory.CreateDirectory(Path.GetDirectoryName(xmlpath));

                        WriteComputerRegExXML(new ComputerGroupRegexCollection(), xmlpath);

                        // If we get here, it wrote successfully, so return the file
                        return xmlpath;
                    }
                    catch
                    {
                        // Create failed - return no path
                        return "";
                    }
                }
            }

            set { Registry.SetValue(regKey, "ComputerRegExXMLFile", value, RegistryValueKind.String); }
        }

        private void WriteComputerRegExXML(ComputerGroupRegexCollection c, string file)
        {
            // Create XML file (overwriting it if it already exists)
            using (FileStream fs = new FileStream(file, FileMode.Create))
            {
                XmlSerializer xs = new XmlSerializer(typeof(ComputerGroupRegexCollection));
                xs.Serialize(fs, c);
            }
        }

        private ComputerGroupRegexCollection ReadComputerRegExXML(string file)
        {
            // Open the XML file and read the config.  Let the exceptions fly for other procedures to handle
            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                ComputerGroupRegexCollection c = new ComputerGroupRegexCollection();
                XmlSerializer xs = new XmlSerializer(typeof(ComputerGroupRegexCollection));
                c = (ComputerGroupRegexCollection)xs.Deserialize(fs);
                return c;
            }
        }

        public ComputerGroupRegexCollection ComputerRegExList
        {
            get
            {
                ComputerGroupRegexCollection c;

                // Try and read the file
                try
                {
                    c = ReadComputerRegExXML(this.ComputerRegExXMLFile);

                    // If we got here, it read correctly - return the collection
                    return c;
                }
                catch
                {
                    // Something went wrong - return a null collection
                    return null;
                }
            }

            set { WriteComputerRegExXML(value, this.ComputerRegExXMLFile); }
        }
    }
}
