using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Security.Cryptography;
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
            // Open the configuration storage registry
            reg = Registry.CurrentUser.OpenSubKey(regPath, true);

            if (reg == null)
                // If the registry key doesn't already exist, create it.
                reg = Registry.CurrentUser.CreateSubKey(regPath);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Useful private variables
        private string regPath = @"Software\\WSUSAdminAssistant";
        private RegistryKey reg;

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // WSUS Configuration methods
        public string WSUSServer
        {
            get 
            {
                object o = reg.GetValue("WSUSServer");

                if (o == null)
                {
                    return "localhost";
                }
                else
                {
                    return (string)o;
                }
            }

            set { reg.SetValue("WSUSServer", value, RegistryValueKind.String); }
        }

        public bool WSUSSecureConnection
        {
            get
            {
                object o = reg.GetValue("WSUSSecureConnection");

                if (o == null)
                {
                    return false;
                }
                else
                {
                    return Convert.ToBoolean((int)reg.GetValue("WSUSSecureConnection"));
                }
            }

            set { reg.SetValue("WSUSSecureConnection", Convert.ToInt32(value), RegistryValueKind.DWord); }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Database Configuration methods
        public string DBServer
        {
           get 
            {
                object o = reg.GetValue("DBServer");

                if (o == null)
                {
                    return "localhost";
                }
                else
                {
                    return (string)o;
                }
            }

            set { reg.SetValue("DBServer", value, RegistryValueKind.String); }
        }

        public string DBDatabase
        {
            get
            {
                object o = reg.GetValue("DBDatabase");

                if (o == null)
                {
                    return "SUSDB";
                }
                else
                {
                    return (string)o;
                }
            }

            set { reg.SetValue("DBDatabase", value, RegistryValueKind.String); }
        }

        public bool DBIntegratedAuth
        {
            get
            {
                object o = reg.GetValue("DBIntegratedAuth");

                if (o == null)
                {
                    return false;
                }
                else
                {
                    return Convert.ToBoolean((int)reg.GetValue("WSUSSecureConnection"));
                }
            }

            set { reg.SetValue("DBIntegratedAuth", Convert.ToInt32(value), RegistryValueKind.DWord); }
        }

        public string DBUsername
        {
            get
            {
                object o = reg.GetValue("DBUsername");

                if (o == null)
                {
                    return "";
                }
                else
                {
                    return (string)o;
                }
            }

            set { reg.SetValue("DBUsername", value, RegistryValueKind.String); }
        }

        public string DBPassword
        {
            get
            {
                // Try to retreive and decrypt encyrpted password
                string pwd;
                object o = reg.GetValue("DBPasswordEncrypted");

                if (o != null)
                {
                    // Try to decrypt password
                    try
                    {
                        pwd = Decrypt(o.ToString(), "dbpwd");
                    }
                    catch
                    {
                        // Decryption failed, return empty password
                        pwd = "";
                    }
                }
                else
                {
                    // If retrieving the encrypted password fails, try to use the old unencrypted password
                    pwd = (string)reg.GetValue("DBPassword");

                    if (pwd == null)
                    {
                        // Unencrypted password not available - use default
                        pwd = "";
                    }
                    else
                    {
                        // Encrypt password, save to registry and delete old key
                        try
                        {
                            reg.SetValue("DBPasswordEncrypted", Encrypt(pwd, "dbpwd"), RegistryValueKind.String);
                            reg.DeleteValue("DBPassword");
                        }
                        catch (Exception e)
                        {
                            // Converting password failed, do not delete old key
                            Console.WriteLine("Failed to encrypt password: " + e.Message);
                        }
                    }
                }

                return pwd;
            }

            set { reg.SetValue("DBPasswordEncrypted", Encrypt(value, "dbpwd"), RegistryValueKind.String); }
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
            get
            {
                object x = reg.GetValue("WindowLocationX");
                object y = reg.GetValue("WindowLocationY");

                if (x == null || y == null)
                {
                    return new System.Drawing.Point(0, 0);
                }
                else
                {
                    return new System.Drawing.Point((int)x, (int)y);
                }
            }

            set
            {
                reg.SetValue("WindowLocationX", value.X, RegistryValueKind.DWord);
                reg.SetValue("WindowLocationY", value.Y, RegistryValueKind.DWord);
            }
        }

        public System.Drawing.Size WindowSize
        {
            get
            {
                object x = reg.GetValue("WindowSizeWidth");
                object y = reg.GetValue("WindowSizeHeight");

                if (x == null || y == null)
                {
                    return new System.Drawing.Size(800, 600);
                }
                else
                {
                    return new System.Drawing.Size((int)x, (int)y);
                }
            }

            set
            {
                reg.SetValue("WindowSizeWidth", value.Width, RegistryValueKind.DWord);
                reg.SetValue("WindowSizeHeight", value.Height, RegistryValueKind.DWord);
            }
        }

        public string WindowState
        {
            get
            {
                object o = reg.GetValue("WindowState");

                if (o == null)
                {
                    return "";
                }
                else
                {
                    return (string)o;
                }
            }

            set { reg.SetValue("WindowState", value, RegistryValueKind.String); }
        }

        public int EndpointSelections
        {
            get
            {
                object o = reg.GetValue("EndpointSelections");

                if (o == null)
                {
                    return 255;
                }
                else
                {
                    return int.Parse(o.ToString());
                }
            }

            set { reg.SetValue("EndpointSelections", value, RegistryValueKind.DWord); }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // String encryption and decryption

        // This constant string is used as a "salt" value for the PasswordDeriveBytes function calls.
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private const string initVector = "WqFoWd5tnVSPjo37";

        // This constant is used to determine the keysize of the encryption algorithm.
        private const int keysize = 256;

        private static string Encrypt(string plainText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }

        private static string Decrypt(string cipherText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }
        
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Helper application methods

        private string FindOnPath(string executable)
        {
            var pathvar = Environment.GetEnvironmentVariable("PATH");

            foreach (var d in pathvar.Split(';'))
            {
                var fullpath = Path.Combine(d, executable);

                FileInfo fi = new FileInfo(fullpath.ToString());
                
                if (fi.Exists)
                    return fullpath;
            }

            // Not found - return nothing
            return "";
        }

        public string PSExecPath
        {
            get
            {
                object o = reg.GetValue("DBUsername");
                string path;

                if (o == null)
                {
                    path = FindOnPath("psexec.exe");
                }
                else
                {
                    path = (string)o;
                }

                return path;
            }

            set { reg.SetValue("PSExecPath", value, RegistryValueKind.String); }
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
                object o = reg.GetValue("SusIdXmlFile");
                string xmlpath ;

                if (o == null)
                {
                    xmlpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                }
                else
                {
                    xmlpath = (string)o;
                }

                xmlpath = Path.Combine(xmlpath, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + "\\DefaultSusIds.xml");

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

            set { reg.SetValue("SusIdXmlFile", value, RegistryValueKind.String); }
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
                object o = reg.GetValue("ComputerRegExXmlFile");
                string xmlpath;

                if (o == null)
                    xmlpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                else
                    xmlpath = (string)o;

                xmlpath = Path.Combine(xmlpath, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + "\\ComputerRegEx.xml");

                // Check if XML file exists
                if (File.Exists(xmlpath))
                    // It exists - return path
                    return xmlpath;
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

            set { reg.SetValue("ComputerRegExXMLFile", value, RegistryValueKind.String); }
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

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Security credential methods and classes

        public class SecurityCredential
        {
            [XmlElement] public string ip;
            [XmlElement] public byte netmask;
            [XmlElement] public string description;

            [XmlElement] public string domain;
            [XmlElement] public string username;
            [XmlElement] public string encryptedpassword;

            [XmlIgnore] public string password
            {
                get
                {
                    string pwd;

                    // Try to decrypt password
                    try
                    {
                        pwd = Decrypt(encryptedpassword, "cred");
                    }
                    catch
                    {
                        // Decryption failed - return empty password
                        pwd = "";
                    }

                    return pwd;
                }

                set { encryptedpassword = Encrypt(value, "cred"); }
            }
        }

        public class CredentialCollection : System.Collections.CollectionBase
        {
            public SecurityCredential this[int index]
            {
                get { return (SecurityCredential)this.List[index]; }
                set { this.List[index] = value; }
            }

            public SecurityCredential this[IPAddress ip]
            {
                get
                {
                    SecurityCredential cred = null;

                    //*** MORE WORK REQUIRED - locate credential by matching ip address to a subnet

                    return cred;
                }
            }

            public int Add(SecurityCredential add)
            {
                return this.List.Add(add);
            }

            public void Remove(int index)
            {
                // Check index is within boundaries
                if (index > this.Count - 1 || index < 0)
                    // Out of bounds
                    throw new IndexOutOfRangeException();
                else
                    this.List.RemoveAt(index);
            }
        }

        public string CredentialXmlFile
        {
            get
            {
                object o = reg.GetValue("CredentialXmlFile");
                string xmlpath;

                if (o == null)
                    xmlpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                else
                    xmlpath = (string)o;

                xmlpath = Path.Combine(xmlpath, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + "\\CredentialXmlFile.xml");

                // Check if XML file exists
                if (File.Exists(xmlpath))
                    // It exists - return path
                    return xmlpath;
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

            set { reg.SetValue("CredentialXmlFile", value, RegistryValueKind.String); }
        }

        private void WriteCredentialXML(CredentialCollection c, string file)
        {
            // Create XML file (overwriting it if it already exists)
            using (FileStream fs = new FileStream(file, FileMode.Create))
            {
                XmlSerializer xs = new XmlSerializer(typeof(CredentialCollection));
                xs.Serialize(fs, c);
            }
        }

        private CredentialCollection ReadCredentialXML(string file)
        {
            // Open the XML file and read the config.  Let the exceptions fly for other procedures to handle
            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                CredentialCollection c = new CredentialCollection();
                XmlSerializer xs = new XmlSerializer(typeof(CredentialCollection));
                c = (CredentialCollection)xs.Deserialize(fs);
                return c;
            }
        }

        public CredentialCollection CredentialList
        {
            get
            {
                CredentialCollection c;

                // Try and read the file
                try
                {
                    c = ReadCredentialXML(this.CredentialXmlFile);

                    // If we got here, it read correctly - return the collection
                    return c;
                }
                catch
                {
                    // Something went wrong - return an empty collection
                    return new CredentialCollection();
                }
            }

            set { WriteCredentialXML(value, this.CredentialXmlFile); }
        }
    }
}
