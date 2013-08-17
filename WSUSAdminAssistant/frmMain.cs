using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;
using Microsoft.UpdateServices.Administration;
using Microsoft.UpdateServices.Administration.Internal;

namespace WSUSAdminAssistant
{
    public partial class frmMain : Form
    {
        private clsConfig cfg = new clsConfig();
        private clsWSUS wsus = new clsWSUS();
        private DateTime lastupdate = Convert.ToDateTime("1970-01-01 00:00:00");
        private DateTime lastupdaterun = Convert.ToDateTime("1970-01-01 00:00:00");

        private bool forceUpdate = true;
        private bool cancelNow = false;

        public frmMain()
        {
            InitializeComponent();
        }

        private void timUpdateData_Tick(object sender, EventArgs e)
        {
            // Disable timer until it's been processed fully
            timUpdateData.Enabled = false;

            // Determine which tab is selected and call it's update procedure
            if (tabAdminType.SelectedTab.Name == tabUnapprovedUpdates.Name) UpdateUnapprovedUpdates();
            if (tabAdminType.SelectedTab.Name == tabEndpointFaults.Name) UpdateEndpointFaults();
            if (tabAdminType.SelectedTab.Name == tabSuperceded.Name) UpdateSupercededUpdates();
            if (tabAdminType.SelectedTab.Name == tabWSUSNotCommunicating.Name) UpdateWSUSNotCommunicating();
            if (tabAdminType.SelectedTab.Name == tabServerRestarts.Name) UpdateServerReboots();
            
            // On return, ensure the "working" dialog is not showing and re-enable the timer
            timUpdateData.Enabled = true;
            gbxWorking.Visible = false;
        }

        private void UpdateEndpointFaults()
        {
            int rn;

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

            // Ping PCs for 1 second
            for (long i = DateTime.Now.Ticks; DateTime.Now.Ticks < i + TimeSpan.TicksPerSecond; )
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
                else
                    // ...otherwise break
                    break;
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
                r.Cells["epPing"].Value = "Error";
                r.Cells["epPing"].ToolTipText = e.Message;
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

        private void UpdateUnapprovedUpdates()
        {
            if (CheckDBConnection())
            {
                // Unapproved updates tab is selected... Check when the last update was modified...
                DateTime clu = wsus.GetLastUpdated(lastupdate);

                if (clu != lastupdate && Math.Abs(clu.Subtract(lastupdaterun).Seconds) < 10 || DateTime.Now.Subtract(lastupdaterun).Minutes > 5 || forceUpdate)
                {
                    // Last updated date and time has changed... let's update the data grid...
                    lastupdate = clu;

                    // No longer need to force an update
                    forceUpdate = false;

                    // Show the update window
                    gbxWorking.Visible = true;
                    this.Refresh();

                    // Tag all rows as not having been updated...
                    foreach (DataGridViewRow r in grdUpdates.Rows)
                    {
                        r.Cells["Updated"].Value = "N";
                    }

                    // Run SQL query to get list of updates
                    DataTable d = wsus.GetUnapprovedUpdates();

                    // Reset UpdateName column to automatic resizing.
                    grdUpdates.Columns["UpdateName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                    foreach (DataRow dr in d.Rows)
                    {
                        // Check to see what filtering is active and if it is, whether the update should be shown
                        bool showupdate = false;

                        if (tliXP.Checked || tliVista.Checked || tli7.Checked || tli8.Checked ||
                            tli2003.Checked || tli2008.Checked || tli2008r2.Checked || tli2012.Checked ||
                            tliOffice2003.Checked || tliOffice2007.Checked || tliOffice2010.Checked || tliOffice2013.Checked)
                        {
                            if (tliXP.Checked && Convert.ToInt32(dr["xp"]) > 0) showupdate = true;
                            if (tliVista.Checked && Convert.ToInt32(dr["v"]) > 0) showupdate = true;
                            if (tli7.Checked && Convert.ToInt32(dr["w7"]) > 0) showupdate = true;
                            if (tli8.Checked && Convert.ToInt32(dr["w8"]) > 0) showupdate = true;

                            if (tli2003.Checked && Convert.ToInt32(dr["s3"]) > 0) showupdate = true;
                            if (tli2008.Checked && Convert.ToInt32(dr["s8"]) > 0) showupdate = true;
                            if (tli2008r2.Checked && Convert.ToInt32(dr["s8r2"]) > 0) showupdate = true;
                            if (tli2012.Checked && Convert.ToInt32(dr["s12"]) > 0) showupdate = true;

                            if (tliOffice2003.Checked && Convert.ToInt32(dr["o3"]) > 0) showupdate = true;
                            if (tliOffice2007.Checked && Convert.ToInt32(dr["o7"]) > 0) showupdate = true;
                            if (tliOffice2010.Checked && Convert.ToInt32(dr["o10"]) > 0) showupdate = true;
                            if (tliOffice2013.Checked && Convert.ToInt32(dr["o13"]) > 0) showupdate = true;
                        }
                        else
                        {
                            showupdate = true;
                        }

                        if (showupdate)
                        {
                            int rn = -1;

                            // Locate an existing matching row
                            foreach (DataGridViewRow dgr in grdUpdates.Rows)
                            {
                                if (dgr.Tag.ToString() == dr["localupdateid"].ToString())
                                {
                                    rn = dgr.Index;
                                    break;
                                }
                            }

                            // If no row is located, create a new one.
                            if (rn == -1) rn = grdUpdates.Rows.Add();

                            DataGridViewRow r = grdUpdates.Rows[rn];

                            // Fill in data grid
                            r.Tag = dr["localupdateid"].ToString();
                            r.Cells["UpdateName"].Value = dr["defaulttitle"].ToString();
                            r.Cells["UpdateName"].ToolTipText = dr["defaulttitle"].ToString();               // Tool tip allows you to view the update when the text is too wide to fit into the cell.
                            r.Cells["Description"].Value = dr["defaultdescription"].ToString();
                            r.Cells["Description"].ToolTipText = dr["defaultdescription"].ToString();  // Tool tip allows you to view the update when the text is too wide to fit into the cell.
                            r.Cells["uaUpdateID"].Value = dr["UpdateID"].ToString();
                            r.Cells["KB"].Value = dr["knowledgebasearticle"].ToString();
                            r.Cells["KB"].ToolTipText = "Click to open knowledge base article in your default browser";

                            // Now determine which groups the updates should be rolled out to
                            DateTime arrived = Convert.ToDateTime(dr["arrivaldate"].ToString());

                            // Testing and T groups should always be shown
                            r.Cells["T"].Value = dr["T"].ToString();
                            r.Cells["ServerT"].Value = dr["Servers T"].ToString();
                            r.Cells["ChemistServerT"].Value = dr["Chemist Servers T"].ToString();
                            r.Cells["ChemistT"].Value = dr["Chemist T"].ToString();
                            r.Cells["Testing"].Value = dr["Testing"].ToString();

                            // Check if A groups should be included or not
                            IncludeAUpdates(arrived, r.Cells["A"], dr["T Approved"], dr["T"], dr["A"].ToString());
                            IncludeAUpdates(arrived, r.Cells["ChemistA"], dr["Chemist T Approved"], dr["Chemist T"], dr["Chemist A"].ToString());

                            // Check if B groups should be included or not
                            IncludeBUpdates(arrived, r.Cells["B"], dr["T Approved"], dr["A Approved"], dr["T"], dr["A"], dr["B"].ToString());
                            IncludeBUpdates(arrived, r.Cells["ChemistB"], dr["Chemist T Approved"], dr["Chemist A Approved"], dr["Chemist T"], dr["Chemist A"], dr["Chemist B"].ToString());

                            // Calculate and set the sort order string
                            string so = "";

                            if (int.Parse("0" + r.Cells["T"].Value.ToString()) > 0) so += "T"; else so += "Z";
                            if (int.Parse("0" + r.Cells["ChemistT"].Value.ToString()) > 0) so += "T"; else so += "Z";
                            if (int.Parse("0" + r.Cells["ServerT"].Value.ToString()) > 0) so += "T"; else so += "Z";
                            if (int.Parse("0" + r.Cells["ChemistServerT"].Value.ToString()) > 0) so += "T"; else so += "Z";
                            if (int.Parse("0" + r.Cells["A"].Value.ToString()) > 0) so += "A"; else so += "Z";
                            if (int.Parse("0" + r.Cells["ChemistA"].Value.ToString()) > 0) so += "A"; else so += "Z";
                            if (int.Parse("0" + r.Cells["B"].Value.ToString()) > 0) so += "B"; else so += "Z";
                            if (int.Parse("0" + r.Cells["ChemistB"].Value.ToString()) > 0) so += "B"; else so += "Z";
                            if (int.Parse("0" + r.Cells["Testing"].Value.ToString()) > 0) so += "T"; else so += "Z";

                            // Tag the row as updated, so long as the update can be approved for at least one group
                            if (so != new String('Z', so.Length)) r.Cells["Updated"].Value = "Y";

                            so += dr["defaulttitle"].ToString();

                            r.Cells["SortOrder"].Value = so;
                        }
                    }

                    // Sort the datagrid.
                    grdUpdates.Sort(grdUpdates.Columns["SortOrder"], ListSortDirection.Ascending);

                    // Remove any row that hasn't been updated
                    bool deletedany = false;

                    do
                    {
                        // Reset deleted flag
                        deletedany = false;

                        // Loop through each row and delete those rows that haven't been updated
                        foreach (DataGridViewRow r in grdUpdates.Rows)
                        {
                            if (r.Cells["Updated"].Value == null || r.Cells["Updated"].Value.ToString() == "N")
                            {
                                grdUpdates.Rows.Remove(r);  //Remove rows that haven't been updated
                                deletedany = true;          //Note that rows have been deleted
                            }
                        }
                        // Keep looping until no rows have been deleted
                    } while (deletedany == true);

                    // Alternate the row's background colour to make viewing easier
                    foreach (DataGridViewRow r in grdUpdates.Rows)
                    {
                        if (r.Index % 2 == 0)
                            r.DefaultCellStyle.BackColor = Color.Empty;
                        else
                            r.DefaultCellStyle.BackColor = Color.LightGray;
                    }
                }
            }

            // Ensure UpdateName column isn't too wide (a maximum of a quarter of the window's width)
            if (grdUpdates.Columns["UpdateName"].Width > (this.Width / 4))
            {
                grdUpdates.Columns["UpdateName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                grdUpdates.Columns["UpdateName"].Width = this.Width / 4;
            }

            // Show total number of updates
            tlsUpdateCount.Text = grdUpdates.Rows.Count.ToString() + " update(s)";

            // Since this timer is active, ticks should occur every 15 seconds...
            timUpdateData.Interval = 15000;

            // Make a note of the time that the update was last run
            lastupdaterun = DateTime.Now;
        }

        private void IncludeAUpdates(DateTime arrived, DataGridViewCell c, object TApproved, object T, string ACount)
        {
            // A groups should only be shown 7 days after being approved for the T group or 7 days after they arrived if no T PCs require the update
            c.Value = ""; // Default to not ready for approval

            // Has the update been approved for T PCs?
            if (TApproved != null && TApproved.ToString() != "")
            {
                // Yes, only allow it to be approved 7 days after the update has been approved for T PCs

                DateTime approved = Convert.ToDateTime(TApproved.ToString());
                if (approved < DateTime.Now.AddDays(-7)) c.Value = ACount;
            }
            else
            {
                if (T.ToString() == "")
                {
                    // If no T PCs require the update, allow it to be approved 7 days after the update arrived
                    if (DateTime.Now.AddDays(-7) > arrived) c.Value = ACount;
                }
            }
        }

        private void IncludeBUpdates(DateTime arrived, DataGridViewCell c, object TApproved, object AApproved, object T, object A, string BCount)
        {
            // B groups should only be shown 4 days after being approved for the A group or 14 days after they arrived if no T or A PCs require the update
            c.Value = ""; // Default to not ready for approval

            DateTime approved;

            // Has the update been approved for A PCs?
            if (AApproved != null && AApproved.ToString() != "")
            {
                // Yes, only allow it to be approved for B PCs 7 days after A PCs were approved

                approved = Convert.ToDateTime(AApproved.ToString());
                if (approved < DateTime.Now.AddDays(-7)) c.Value = BCount;
            }
            else
            {
                // No, has it been approved for T PCs?
                if (TApproved != null && TApproved.ToString() != "")
                {
                    // Yes, only allow it to be approved for B PCs 7 days after T PCs were approved, and only if it's not required by any A PCs
                    approved = Convert.ToDateTime(TApproved.ToString());
                    if (approved < DateTime.Now.AddDays(-7) && A == null) c.Value = BCount;
                }
                else
                {
                    // No, only allow it to be approved after 14 days if no T or A PCs require the update
                    if (T.ToString() == "" && A.ToString() == "")
                    {
                        // If no T or A PCs require the update, allow it to be approved 14 days after the update arrived
                        if (DateTime.Now.AddDays(-14) > arrived) c.Value = BCount;
                    }
                }
            }
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
            // Link status items to WSUS class
            lblSQLStatus.Text = wsus.dbStatus;
            lblWSUSStatus.Text = wsus.wsusStatus;

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

        private void tliXP_Click(object sender, EventArgs e)
        {
            tliXP.Checked = !tliXP.Checked;

            ForceUpdate(2500);
        }

        private void tliVista_Click(object sender, EventArgs e)
        {
            tliVista.Checked = !tliVista.Checked;

            ForceUpdate(2500);
        }

        private void tli7_Click(object sender, EventArgs e)
        {
            tli7.Checked = !tli7.Checked;

            ForceUpdate(2500);
        }

        private void tlsNoFilter_Click(object sender, EventArgs e)
        {
            tliXP.Checked = false;
            tliVista.Checked = false;
            tli7.Checked = false;
            tli8.Checked = false;

            tli2003.Checked = false;
            tli2008.Checked = false;
            tli2008r2.Checked = false;
            tli2012.Checked = false;

            tliOffice2003.Checked = false;
            tliOffice2007.Checked = false;
            tliOffice2010.Checked = false;
            tliOffice2013.Checked = false;

            ForceUpdate(100);
        }

        private void tliOffice2003_Click(object sender, EventArgs e)
        {
            tliOffice2003.Checked = !tliOffice2003.Checked;

            ForceUpdate(2500);
        }

        private void tliOffice2007_Click(object sender, EventArgs e)
        {
            tliOffice2007.Checked = !tliOffice2007.Checked;

            ForceUpdate(2500);
        }

        private void tliOffice2010_Click(object sender, EventArgs e)
        {
            tliOffice2010.Checked = !tliOffice2010.Checked;

            ForceUpdate(2500);
        }

        private void tliOffice2012_Click(object sender, EventArgs e)
        {
            tliOffice2013.Checked = !tliOffice2013.Checked;

            ForceUpdate(2500);
        }

        private void tli8_Click(object sender, EventArgs e)
        {
            tli8.Checked = !tli8.Checked;

            ForceUpdate(2500);
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
            // Note whether any updates were declined
            bool declined = true;

            while (declined)
            {
                declined = false;

                // Decline all updates for selected updates
                foreach (DataGridViewRow r in grdSupercededUpdates.Rows)
                {
                    if ((bool)r.Cells["suSelect"].Value == true)
                    {
                        // Decline update
                        UpdateRevisionId ur = new UpdateRevisionId();
                        ur.UpdateId = new Guid(r.Cells["suUpdateID"].Value.ToString());
                        IUpdate u = wsus.server.GetUpdate(ur);

                        u.Decline();

                        grdSupercededUpdates.Rows.Remove(r);
                        this.Refresh();

                        declined = true;
                    }
                }
                // Loop until no more updates have been declined
            }
            // Trigger update of dialog box
            timUpdateData.Interval = 100;
        }

        private void clearSelectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewCell c in grdUpdates.SelectedCells)
                c.Selected = false;
        }

        private void ShowCancelApproveButton(bool show)
        {
            if (show)
            {
                // We're supposed to show the cancel button, which also implies disabling the approve and decline buttons, disabling the timer and resetting the
                // Cancel Now flag
                butCancelApprove.Visible = true;
                butApproveUpdates.Enabled = false;
                butDeclineUnapproved.Enabled = false;

                timUpdateData.Enabled = false;
                cancelNow = false;
                this.Refresh();
            }
            else
            {
                // We're supposed to hide the cancel button, which also implies enabling the approve and decline buttons and the timer
                butCancelApprove.Visible = false;
                butApproveUpdates.Enabled = true;
                butDeclineUnapproved.Enabled = true;

                timUpdateData.Enabled = true;
                this.Refresh();
            }
        }

        private void butApproveUpdates_Click(object sender, EventArgs e)
        {
            // Show cancel button
            ShowCancelApproveButton(true);

            // Loop through each selected cell and see what to approve
            foreach (DataGridViewCell c in grdUpdates.SelectedCells)
            {
                // Break out of the loop now if the cancel flag has been set
                if (cancelNow) break;

                // Only allow the update to be approved if some PCs require it.
                if (c.Value.ToString() != "")
                {
                    // Ensure update is visible so end user can see what's going on...
                    grdUpdates.CurrentCell = c;
                    this.Refresh();

                    // Get the appropriate update object
                    UpdateRevisionId ur = new UpdateRevisionId();
                    ur.UpdateId = new Guid(grdUpdates.Rows[c.RowIndex].Cells["uaUpdateID"].Value.ToString());
                    IUpdate u = wsus.server.GetUpdate(ur);

                    // Figure out which computer group we're referring to
                    string group = grdUpdates.Columns[c.ColumnIndex].HeaderText.ToString();
                    IComputerTargetGroup tg = null;

                    foreach (IComputerTargetGroup ctg in wsus.server.GetComputerTargetGroups())
                    {
                        if (group == "Testing" && ctg.Name == "Testing") tg = ctg;

                        if (group == "Group T" && ctg.Name == "Workstations T") tg = ctg;
                        if (group == "Group A" && ctg.Name == "Workstations A") tg = ctg;
                        if (group == "Group B" && ctg.Name == "Workstations B") tg = ctg;

                        if (group == "Chemist T" && ctg.Name == "Minfos Workstations T") tg = ctg;
                        if (group == "Chemist A" && ctg.Name == "Minfos Workstations A") tg = ctg;
                        if (group == "Chemist B" && ctg.Name == "Minfos Workstations B") tg = ctg;

                        if (group == "Servers T" && ctg.Name == "Servers T") tg = ctg;

                        if (group == "Chemist Servers T" && ctg.Name == "Minfos Servers T") tg = ctg;
                    }

                    // If a valid group was found, approve the update
                    if (tg != null)
                    {
                        // Does the update require a EULA approval?
                        if (u.RequiresLicenseAgreementAcceptance)
                        {
                            // Get license agreement, check to see if the license has been agreed to and approve it if it hasn't
                            ILicenseAgreement eula = u.GetLicenseAgreement();
                            if (!eula.IsAccepted) u.AcceptLicenseAgreement();
                        }

                        // Approve update
                        u.Approve(UpdateApprovalAction.Install, tg);

                        // Empty cell and unselect it so the end user knows the update has been approved
                        c.Value = "Approved";
                        c.Selected = false;
                        this.Refresh();

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

        private void ChangeUpdateSelection(string group, bool selected)
        {
            // Scan each row to find updates for the appropriate group and select them
            foreach (DataGridViewRow r in grdUpdates.Rows)
            {
                if (r.Cells[group].Value.ToString() != "")
                    r.Cells[group].Selected = selected;
            }
        }

        private void AselectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeUpdateSelection("A", true);
        }

        private void AdeselectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeUpdateSelection("A", false);
        }

        private void BselectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeUpdateSelection("B", true);
        }

        private void BdeselectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeUpdateSelection("B", false);
        }

        private void SselectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeUpdateSelection("S", true);
        }

        private void SdeselectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeUpdateSelection("S", false);
        }

        private void CAselectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeUpdateSelection("ChemistA", true);
        }

        private void CAdeselectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeUpdateSelection("ChemistA", false);
        }

        private void CBselectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeUpdateSelection("ChemistB", true);
        }

        private void CBdeselectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeUpdateSelection("ChemistB", false);
        }

        private void grdUpdates_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Create handy variables referring to this cell
            DataGridViewRow r = grdUpdates.Rows[e.RowIndex];
            DataGridViewCell c = r.Cells[e.ColumnIndex];
            DataGridViewColumn gc = grdUpdates.Columns[e.ColumnIndex];

            // Check to see if KB column was selected - if it was, open link in default browser
            if (gc.HeaderText == "KB Article")
            {
                Process.Start("http://support.microsoft.com/kb/" + c.Value.ToString());
            }

            // If the column selected is the UpdateName, Description or KB article, deselect the current cell
            if (gc.Name == "UpdateName" || gc.Name == "Description" || gc.Name == "KB")
                c.Selected = false;
        }

        private void butCancelApprove_Click(object sender, EventArgs e)
        {
            cancelNow = true;
        }

        private void butDeclineUnapproved_Click(object sender, EventArgs e)
        {
            // Show the cancel button
            ShowCancelApproveButton(true);

            // Loop through each selected cell and see what to decline
            foreach (DataGridViewCell c in grdUpdates.SelectedCells)
            {
                // Break out of the loop now if the cancel flag has been set
                if (cancelNow) break;

                // Only allow the update to be declined if some PCs require it.
                if (c.Value.ToString() != "")
                {
                    // Ensure update is visible so end user can see what's going on...
                    grdUpdates.CurrentCell = c;
                    this.Refresh();

                    // Get the appropriate update object
                    UpdateRevisionId ur = new UpdateRevisionId();
                    ur.UpdateId = new Guid(grdUpdates.Rows[c.RowIndex].Cells["uaUpdateID"].Value.ToString());
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
            Form f = new frmWSUSConfig();
            f.ShowDialog();
        }

        private void mnuComputerGroupRules_Click(object sender, EventArgs e)
        {
            Form f = new frmComputerGroupRules();
            f.ShowDialog();
        }

        private void mnuDefaultSusIDList_Click(object sender, EventArgs e)
        {
            Form f = new frmDefaultSUS();
            f.Show();
        }

        private void mnuPreferences_Click(object sender, EventArgs e)
        {
            Form f = new frmPreferences();
            f.ShowDialog();
        }

        private void mnuCredentials_Click(object sender, EventArgs e)
        {
            Form f = new frmCredentials();
            f.ShowDialog();
        }
    }
}
