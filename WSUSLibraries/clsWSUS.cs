using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.UpdateServices.Administration;
using Microsoft.UpdateServices.Administration.Internal;

namespace WSUSAdminAssistant
{
    public class clsWSUS
    {
        private clsConfig cfg;

        private SqlConnection sql = new SqlConnection();

        #region OldUpdatesQuery
        private SqlCommand cmdUnapproved = new SqlCommand(@"
            use SUSDB

            select c.targetid, fulldomainname, gi.targetgroupid, gi.name into #computers
            from tbcomputertarget c with (nolock)
            join tbtargetintargetgroup g with (nolock) on g.targetid = c.targetid
            join tbtargetgroup gi with (nolock) on gi.targetgroupid = g.targetgroupid

            create clustered index ix_targetid on #computers (targetid)

            /***********************************************************************************************************************************/

            select updateid, max(revisionnumber) rn into #updaterevs
            from PUBLIC_VIEWS.vUpdate
            group by updateid

            /***********************************************************************************************************************************/

            select uc.localupdateid, ud.updateid, defaulttitle, defaultdescription, knowledgebasearticle, fulldomainname as PC, ud.arrivaldate, c.name as pcgroup into #updates
            from tbupdatestatuspercomputer uc with (nolock)
            join #computers c on c.targetid = uc.targetid
            join tbupdate u with (nolock) on u.localupdateid = uc.localupdateid and ishidden = 0
            join PUBLIC_VIEWS.vUpdate ud on ud.updateid = u.updateid and isdeclined = 0
            join #updaterevs ur on ud.updateid = ur.updateid
            left outer join PUBLIC_VIEWS.vUpdateApproval ua on ua.updateid = u.updateid and ua.computertargetgroupid = c.targetgroupid
            where uc.summarizationstate = 2 and ua.updateapprovalid is null

            create clustered index ix_localupdateid on #updates (localupdateid)
            create index ix_updateid on #updates (updateid)
            create index ix_pcgroup on #updates (PC, pcgroup)

            /***********************************************************************************************************************************/

            select ua.updateid, ua.creationdate approvaldate, ctg.name groupname into #updateapprovals
            from PUBLIC_VIEWS.vUpdateApproval ua
            join PUBLIC_VIEWS.vComputerTargetGroup ctg on ua.computertargetgroupid = ctg.computertargetgroupid
            where ua.action = 'Install'

            create clustered index ix on #updateapprovals (updateid,groupname)
            /***********************************************************************************************************************************/

            select * into #vCategory
            from PUBLIC_VIEWS.vCategory

            create clustered index ix on #vcategory (categoryid, categorytype, defaulttitle)

            /***********************************************************************************************************************************/

            select u.updateid, count(xp.defaulttitle) xp, count(v.defaulttitle) v, count(w7.defaulttitle) w7, count(w8.defaulttitle) w8,
	            count(s3.defaulttitle) s3, count(s8.defaulttitle) s8, count(s82.defaulttitle) s82, count(s12.defaulttitle) s12,
	            count(o3.defaulttitle) o3, count(o7.defaulttitle) o7, count(o10.defaulttitle) o10, count(o13.defaulttitle) o13 into #updatecategory
            from #updates u
            join vwUpdateInCategory uic on u.updateid = uic.updateid
            left outer join #vCategory xp on xp.categorytype = 'Product' and xp.defaulttitle like 'Windows XP%' and uic.categoryupdateid = xp.categoryid
            left outer join #vCategory v on v.categorytype = 'Product' and v.defaulttitle like 'Windows Vista' and uic.categoryupdateid = v.categoryid
            left outer join #vCategory w7 on w7.categorytype = 'Product' and w7.defaulttitle like 'Windows 7' and uic.categoryupdateid = w7.categoryid
            left outer join #vCategory w8 on w8.categorytype = 'Product' and w8.defaulttitle like 'Windows 8' and uic.categoryupdateid = w8.categoryid
            left outer join #vCategory s3 on s3.categorytype = 'Product' and s3.defaulttitle like 'Windows Server 2003%' and uic.categoryupdateid = s3.categoryid
            left outer join #vCategory s8 on s8.categorytype = 'Product' and s8.defaulttitle like 'Windows Server 2008' and uic.categoryupdateid = s8.categoryid
            left outer join #vCategory s82 on s82.categorytype = 'Product' and s82.defaulttitle like 'Windows Server 2008 R2' and uic.categoryupdateid = s82.categoryid
            left outer join #vCategory s12 on s12.categorytype = 'Product' and s12.defaulttitle like 'Windows Server 2012' and uic.categoryupdateid = s12.categoryid
            left outer join #vCategory o3 on o3.categorytype = 'Product' and o3.defaulttitle like 'Office 2003' and uic.categoryupdateid = o3.categoryid
            left outer join #vCategory o7 on o7.categorytype = 'Product' and o7.defaulttitle like 'Office 2007' and uic.categoryupdateid = o7.categoryid
            left outer join #vCategory o10 on o10.categorytype = 'Product' and o10.defaulttitle like 'Office 2010' and uic.categoryupdateid = o10.categoryid
            left outer join #vCategory o13 on o13.categorytype = 'Product' and o13.defaulttitle like 'Office 2013' and uic.categoryupdateid = o13.categoryid
            group by u.updateid

            /***********************************************************************************************************************************/

            select localupdateid, count(*) as PCs into #test
            from #updates
            where pcgroup = 'Testing'
            group by localupdateid

            /***********************************************************************************************************************************/

            select localupdateid, count(*) as PCs into #t
            from #updates
            where pcgroup = 'Workstations T'
            group by localupdateid

            /***********************************************************************************************************************************/

            select u.localupdateid, count(*) as PCs into #a
            from #updates u
            where pcgroup = 'Workstations A'
            group by u.localupdateid

            /***********************************************************************************************************************************/

            select u.localupdateid, count(*) as PCs into #b
            from #updates u
            where pcgroup = 'Workstations B'
            group by u.localupdateid

            /***********************************************************************************************************************************/

            select localupdateid, count(*) as PCs into #st
            from #updates
            where pcgroup = 'Servers T'
            group by localupdateid

            /***********************************************************************************************************************************/

            select localupdateid, count(*) as PCs into #ct
            from #updates
            where pcgroup = 'Minfos Workstations T'
            group by localupdateid

            /***********************************************************************************************************************************/

            select u.localupdateid, count(*) as PCs into #ca
            from #updates u
            where pcgroup = 'Minfos Workstations A'
            group by u.localupdateid

            /***********************************************************************************************************************************/

            select u.localupdateid, count(*) as PCs into #cb
            from #updates u
            where pcgroup = 'Minfos Workstations B'
            group by u.localupdateid

            /***********************************************************************************************************************************/

            select localupdateid, count(*) as PCs into #cst
            from #updates
            where pcgroup = 'Minfos Servers T'
            group by localupdateid

            /***********************************************************************************************************************************/

            select u.localupdateid, u.updateid, u.defaulttitle, u.defaultdescription, knowledgebasearticle, arrivaldate, t.PCs as [T], tua.approvaldate [T Approved],
	            aua.approvaldate [A Approved], a.PCs as [A], b.PCs as [B], st.PCs as [Servers T], stua.approvaldate [Servers T Approved],
	            ct.PCs as [Chemist T], ctua.approvaldate [Chemist T Approved], ca.PCs as [Chemist A], caua.approvaldate [Chemist A Approved], cb.PCs as [Chemist B],
	            cst.PCs as [Chemist Servers T], cstua.approvaldate [Chemist Servers T Approved], test.PCs as [Testing],
	            uc.xp, uc.v, uc.w7, uc.w8, uc.s3, uc.s8, uc.s82, uc.s12, uc.o3, uc.o7, uc.o10, uc.o13
            from (
                    select distinct localupdateid, defaultdescription, updateid, defaulttitle, knowledgebasearticle, arrivaldate
                    from #updates
                ) u
            left outer join #t t on u.localupdateid = t.localupdateid
            left outer join #updateapprovals tua on tua.updateid = u.updateid and tua.groupname = 'Workstations T'
            left outer join #a a on u.localupdateid = a.localupdateid
            left outer join #updateapprovals aua on aua.updateid = u.updateid and aua.groupname = 'Workstations A'
            left outer join #b b on u.localupdateid = b.localupdateid
            left outer join #st st on u.localupdateid = st.localupdateid
            left outer join #updateapprovals stua on stua.updateid = u.updateid and stua.groupname = 'Servers T'
            left outer join #ct ct on u.localupdateid = ct.localupdateid
            left outer join #updateapprovals ctua on ctua.updateid = u.updateid and ctua.groupname = 'Minfos Workstations T'
            left outer join #ca ca on u.localupdateid = ca.localupdateid
            left outer join #updateapprovals caua on caua.updateid = u.updateid and caua.groupname = 'Minfos Workstations A'
            left outer join #cb cb on u.localupdateid = cb.localupdateid
            left outer join #cst cst on u.localupdateid = cst.localupdateid
            left outer join #test test on u.localupdateid = test.localupdateid
            left outer join #updateapprovals cstua on cstua.updateid = u.updateid and cstua.groupname = 'Minfos Servers T'
            left outer join #updatecategory uc on uc.updateid = u.updateid
            where t.PCs > 0 or a.PCs > 0 or b.PCs > 0 or ct.PCs > 0 or ca.PCs > 0 or cb.PCs > 0 or cst.PCs > 0 or test.PCs > 0

            /***********************************************************************************************************************************/

            drop table #computers, #updaterevs, #updates, #updateapprovals, #vCategory, #updatecategory, #test, #t, #a, #b, #st, #ct, #ca, #cb, #cst
        ");
        #endregion

        private SqlCommand cmdUnapprovedUpdates = new SqlCommand();
        private string strUnapprovedUpdates = @"
            use SUSDB

            select c.targetid, fulldomainname, gi.targetgroupid, gi.name as pcgroup into #computers
            from tbcomputertarget c with (nolock)
            join tbtargetintargetgroup g with (nolock) on g.targetid = c.targetid
            join tbtargetgroup gi with (nolock) on gi.targetgroupid = g.targetgroupid and gi.name in ({0});

            create clustered index ix_targetid on #computers (targetid)

            /***********************************************************************************************************************************/

            select updateid, max(revisionnumber) rn into #updaterevs
            from PUBLIC_VIEWS.vUpdate
            group by updateid

            /***********************************************************************************************************************************/

            select uc.localupdateid, ud.updateid, defaulttitle, defaultdescription, knowledgebasearticle, fulldomainname as PC, ud.arrivaldate, c.pcgroup into #updates
            from tbupdatestatuspercomputer uc with (nolock)
            join #computers c on c.targetid = uc.targetid
            join tbupdate u with (nolock) on u.localupdateid = uc.localupdateid and ishidden = 0
            join PUBLIC_VIEWS.vUpdate ud on ud.updateid = u.updateid and isdeclined = 0
            join #updaterevs ur on ud.updateid = ur.updateid
            left outer join PUBLIC_VIEWS.vUpdateApproval ua on ua.updateid = u.updateid and ua.computertargetgroupid = c.targetgroupid
            where uc.summarizationstate = 2 and ua.updateapprovalid is null

            create clustered index ix_localupdateid on #updates (localupdateid)
            create index ix_updateid on #updates (updateid)
            create index ix_pcgroup on #updates (pcgroup)

            /***********************************************************************************************************************************/

            select ua.updateid, ua.creationdate approvaldate, ctg.name groupname into #updateapprovals
            from PUBLIC_VIEWS.vUpdateApproval ua
            join PUBLIC_VIEWS.vComputerTargetGroup ctg on ua.computertargetgroupid = ctg.computertargetgroupid
            where ua.action = 'Install'

            create clustered index ix on #updateapprovals (updateid,groupname)

            /***********************************************************************************************************************************/

            select localupdateid, pcgroup, count(*) as PCs into #groups
            from #updates
            where pcgroup in ({0})
            group by localupdateid, pcgroup

            /***********************************************************************************************************************************/

            select u.localupdateid, u.updateid, u.defaulttitle, u.defaultdescription, knowledgebasearticle, arrivaldate, tg.name pcgroup, g.PCs, ua.approvaldate
            from (
                    select distinct localupdateid, defaultdescription, updateid, defaulttitle, knowledgebasearticle, arrivaldate
                    from #updates
                ) u
            left outer join tbtargetgroup tg on tg.name in ({0})
            left outer join #groups g on g.localupdateid = u.localupdateid and g.pcgroup = tg.name
            left outer join #updateapprovals ua on ua.updateid = u.updateid and ua.groupname = tg.name
            where g.PCs is not null or approvaldate is not null
            order by defaulttitle, approvaldate

            /***********************************************************************************************************************************/

            drop table #computers, #updaterevs, #updates, #updateapprovals, #groups
        ";

        private SqlCommand cmdApprovedUpdates = new SqlCommand(@"
            use susdb
            select fulldomainname, ipaddress, count(*) approvedupdates, max(lastsynctime) lastsynctime
            from tbupdatestatuspercomputer uc with (nolock)
            join tbcomputertarget c on c.targetid = uc.targetid
            join tbtargetintargetgroup g with (nolock) on g.targetid = c.targetid
            join tbupdate u with (nolock) on u.localupdateid = uc.localupdateid and ishidden = 0
            join PUBLIC_VIEWS.vUpdateApproval ua on ua.updateid = u.updateid and ua.computertargetgroupid = g.targetgroupid
            	and datediff(d, ua.creationdate, getdate()) > 5
            where uc.summarizationstate = 2
            group by fulldomainname, ipaddress
        ");

        private SqlCommand cmdUpdateErrors = new SqlCommand(@"
            use susdb
            select fulldomainname, ipaddress, count(*) updateerrors, max(lastsynctime) lastsynctime
            from tbupdatestatuspercomputer uc with (nolock)
            join tbcomputertarget c on c.targetid = uc.targetid
            join tbtargetintargetgroup g with (nolock) on g.targetid = c.targetid
            join tbupdate u with (nolock) on u.localupdateid = uc.localupdateid and ishidden = 0
            join PUBLIC_VIEWS.vUpdateApproval ua on ua.updateid = u.updateid and ua.computertargetgroupid = g.targetgroupid
            	and datediff(d, ua.creationdate, getdate()) > 5
            where uc.summarizationstate = 5
            group by fulldomainname, ipaddress
        ");

        private SqlCommand cmdLastUpdate = new SqlCommand(@"
            use susdb
            
            select max(deploymenttime) lastchange from tbDeployment;
        ");

        private SqlCommand cmdLastSync = new SqlCommand(@"
            use susdb
            select max(lastsynctime) lastsynctime from tbcomputertarget;
        ");

        private SqlCommand cmdUnassignedComputers = new SqlCommand(@"
            use susdb;

            select c.name, c.ipaddress
            from public_views.vcomputertarget c
            join public_views.vcomputergroupmembership cg on cg.computertargetid = c.computertargetid
            join public_views.vcomputertargetgroup g on g.computertargetgroupid = cg.computertargetgroupid and g.name = 'Unassigned Computers'
            order by c.ipaddress
        ");

        private SqlCommand cmdComputerGroups = new SqlCommand(@"
            use susdb

            select c.name, c.ipaddress, cg.name groupname
            from PUBLIC_VIEWS.vComputerTarget c
            join PUBLIC_VIEWS.vComputerGroupMembership cm on cm.computertargetid = c.computertargetid and cm.isexplicitmember = 1
            join PUBLIC_VIEWS.vComputerTargetGroup cg on cm.computertargetgroupid = cg.computertargetgroupid and cg.name not in ('Unassigned Computers', 'All Computers')
            order by c.name
        ");

        private SqlCommand cmdSupercededUpdates = new SqlCommand(@"
                use susdb

                select distinct u.updateid, u.defaulttitle
                from vwminimalupdate mu
                join public_views.vupdate u on u.updateid = mu.updateid
                join PUBLIC_VIEWS.vUpdateInstallationInfoBasic ui on ui.state = 2 and ui.updateid = mu.updateid
                where issuperseded = 1 and isdeclined = 0
                order by u.defaulttitle
            ");

        // String properties to pass status messages back to form
        public string dbStatus { get; set; }
        public string wsusStatus { get; set; }

        // Various WSUS variables, some with lazy initialisation

        public IUpdateServer server = null;

        private ComputerTargetGroupCollection _computergroups = null;
        private DateTime _cgupdated = DateTime.MinValue;

        public ComputerTargetGroupCollection ComputerGroups
        {
            get
            {
                // If we've never retreived computer groups, or they were updated less than 10 seconds ago, go get 'em
                if (_computergroups == null || DateTime.Now.Subtract(_cgupdated).TotalSeconds > 10)
                {
                    _computergroups = server.GetComputerTargetGroups();
                    _cgupdated = DateTime.Now;
                }

                return _computergroups;
            }
        }

        public bool CheckDBConnection()
        {
            // Check connection state to ensure it's open.
            if (sql.State == ConnectionState.Closed || sql.State == ConnectionState.Broken)
            {
                // It's not open - let's try to open it
                try
                {
                    sql.Open();

                    // If we got here, the connection is OK.
                    dbStatus = "OK";
                    return true;
                }
                catch (Exception ex)
                {
                    // That didn't work - return false and display error
                    dbStatus = "Error: " + ex.Message;
                    return false;
                }
            }
            else if (sql.State == ConnectionState.Open)
            {
                // Connection is OK.
                dbStatus = "OK";
                return true;
            }
            else
            {
                // Connection is in an unknown state - display message and return false
                dbStatus = "SQL server connection in an unknown state - " + sql.State.ToString();

                return false;
            }
        }

        public clsWSUS(clsConfig configobject)
        {
            cfg = configobject;

            // Initialise SQL query
            sql.ConnectionString = cfg.SQLConnectionString();

            if (CheckDBConnection())
            {
                // Set connection to SQL server for all queries
                cmdUnapproved.Connection = sql;
                cmdLastUpdate.Connection = sql;
                cmdApprovedUpdates.Connection = sql;
                cmdLastSync.Connection = sql;
                cmdUnassignedComputers.Connection = sql;
                cmdUpdateErrors.Connection = sql;
                cmdComputerGroups.Connection = sql;
            }

            // Connect to WSUS server
            try
            {
                server = AdminProxy.GetUpdateServer(cfg.WSUSServer, cfg.WSUSSecureConnection);
                wsusStatus = "OK";
            }
            catch (Exception ex)
            {
                wsusStatus = "Error: " + ex.Message;
            }
        }

        public DateTime GetLastUpdated(DateTime lastupdate)
        {
            // Retrieve and parse date last updated
            cmdLastUpdate.ExecuteNonQuery();

            SqlDataReader d = cmdLastUpdate.ExecuteReader();

            DateTime lu = lastupdate;

            while (d.Read())
            {
                lu = Convert.ToDateTime(d["lastchange"].ToString());
            }

            d.Close();

            return lu;
        }

        public DataTable GetApprovedUpdates()
        {
            // Run query and return results
            cmdApprovedUpdates.ExecuteNonQuery();

            SqlDataReader r = cmdApprovedUpdates.ExecuteReader();
            DataTable t = new DataTable();

            // Load the data table
            t.Load(r);
            r.Close();

            return t;
        }

        public DataTable GetUnapprovedUpdates()
        {
            // Run query and return results
            cmdUnapproved.ExecuteNonQuery();

            SqlDataReader r = cmdUnapproved.ExecuteReader();
            DataTable t = new DataTable();

            // Load the data table
            t.Load(r);
            r.Close();

            return t;
        }

        public DataTable GetUnapprovedUpdatesNew()
        {
            if (CheckDBConnection())
            {
                // Inject the right parameters into the Unapproved Updates query
                clsConfig.GroupUpdateRuleCollection ur = cfg.GroupUpdateRules;
                string[] parameters = new string[ur.Count];
                cmdUnapprovedUpdates.Parameters.Clear();

                // Build list of parameters
                for (int i = 0; i < ur.Count; i++)
                    parameters[i] = string.Format("@Group{0}", i);

                // Set SQL query
                cmdUnapprovedUpdates.CommandText = string.Format(strUnapprovedUpdates, string.Join(", ", parameters));

                // Build parameters
                for (int i = 0; i < ur.Count; i++)
                {
                    cmdUnapprovedUpdates.Parameters.Add(parameters[i], SqlDbType.NVarChar);
                    cmdUnapprovedUpdates.Parameters[parameters[i]].Value = ur[i].computergroup.Name;
                }

                // Associate with SQL query
                cmdUnapprovedUpdates.Connection = sql;

                // Run query and return results
                cmdUnapprovedUpdates.ExecuteNonQuery();

                SqlDataReader r = cmdUnapprovedUpdates.ExecuteReader();
                DataTable t = new DataTable();

                // Load the data table
                t.Load(r);
                r.Close();

                return t;
            }

            // DB Connection not OK - return a null table
            return null;
        }

        public DataTable GetUnassignedComputers()
        {
            // Run query and return results
            cmdUnassignedComputers.ExecuteNonQuery();

            SqlDataReader r = cmdUnassignedComputers.ExecuteReader();
            DataTable t = new DataTable();

            // Load the data table
            t.Load(r);
            r.Close();

            return t;
        }

        public DataTable GetComputerGroups()
        {
            // Run query and return results
            cmdComputerGroups.ExecuteNonQuery();

            SqlDataReader r = cmdComputerGroups.ExecuteReader();
            DataTable t = new DataTable();

            // Load the data table
            t.Load(r);
            r.Close();

            return t;
        }

        public DataTable GetUpdateErrors()
        {
            // Run query and return results
            cmdUpdateErrors.ExecuteNonQuery();

            SqlDataReader r = cmdUpdateErrors.ExecuteReader();
            DataTable t = new DataTable();

            // Load the data table
            t.Load(r);
            r.Close();

            return t;
        }

        public DataTable GetSupercededUpdates()
        {
            // Run query and return results
            cmdSupercededUpdates.Connection = sql;
            cmdSupercededUpdates.ExecuteNonQuery();

            SqlDataReader r = cmdSupercededUpdates.ExecuteReader();
            DataTable t = new DataTable();

            // Load the data table
            t.Load(r);
            r.Close();

            return t;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //  Class that returns the list of PCs requiring updates
        public class PerGroupInformation
        {
            public PerGroupInformation(clsConfig.GroupUpdateRule updaterule)
            {
                _grouprule = updaterule;
            }

            public IComputerTargetGroup Group { get { return _grouprule.computergroup; } }

            private clsConfig.GroupUpdateRule _grouprule;
            public clsConfig.GroupUpdateRule GroupRule { get { return _grouprule; } }

            public Nullable<DateTime> Approved = null;
            public Nullable<DateTime> Approvable = null;
            public Nullable<int> PCs = null;

            public bool UpdateApprovableNow
            {
                get
                {
                    if (Approvable.HasValue && DateTime.Now > Approvable)
                        // Update can be installed now
                        return true;
                    else
                        return false;
                }
            }
        }

        public class PerGroupCollection : System.Collections.CollectionBase
        {
            public PerGroupCollection(clsConfig.GroupUpdateRuleCollection groups)
            {
                // Loop through each group and add to the list and the dictionaries.  This is the only way items can be created.
                foreach (clsConfig.GroupUpdateRule ur in groups)
                    this.List.Add(new PerGroupInformation(ur));
            }

            // Locate item by index number
            public PerGroupInformation this[int index]
            {
                // Search and set items by index number
                get { return (PerGroupInformation)this.List[index]; }
                set { this.List[index] = value; }
            }

            // Locate item by group name
            public PerGroupInformation this[string group]
            {
                get
                {
                    // Loop through each item, looking for a group that matches
                    foreach (PerGroupInformation gi in this.List)
                        if (gi.Group.Name == group)
                            // Got one - return it
                            return gi;

                    // Couldn't find one - return null
                    return null;
                }

                set
                {
                    // Loop through each item, looking for a group that matches
                    for (int i = 0; i < this.List.Count; i++)
                    {
                        PerGroupInformation gi = (PerGroupInformation)this.List[i];

                        if (gi.Group.Name == group)
                        {
                            gi = value;
                            return;
                        }
                    }

                    // Couldn't find a group - throw an exception
                    throw new ArgumentException("Computer group " + group + " could not be found.");
                }
            }

            // Locate item by GUID
            public PerGroupInformation this[Guid groupid]
            {
                get
                {
                    // Loop through each item, looking for a group that matches
                    foreach (PerGroupInformation gi in this.List)
                        if (gi.Group.Id == groupid)
                            // Got one - return it
                            return gi;

                    // Couldn't find one - return null
                    return null;
                }

                set
                {
                    // Loop through each item, looking for a group that matches
                    for (int i = 0; i < this.List.Count; i++)
                    {
                        PerGroupInformation gi = (PerGroupInformation)this.List[i];

                        if (gi.Group.Id == groupid)
                        {
                            gi = value;
                            return;
                        }
                    }

                    // Couldn't find a group - throw an exception
                    throw new ArgumentException("Computer group ID " + groupid + " could not be found.");
                }
            }
        }

        public class UnapprovedUpdate
        {
            public UnapprovedUpdate(clsConfig.GroupUpdateRuleCollection groups, string updateid, string title, string description, string kbarticle, DateTime arrivaldate)
            {
                _groups = new PerGroupCollection(groups);
                _updateid = updateid;
                _title = title;
                _description = description;
                _kbarticle = kbarticle;
                _arrivaldate = arrivaldate;
            }

            private string _updateid;
            public string UpdateID { get { return _updateid; } }

            private string _title;
            public string Title { get { return _title; } }

            private string _description;
            public string Description { get { return _description; } }

            private string _kbarticle;
            public string KBArticle { get { return _kbarticle; } }

            private DateTime _arrivaldate;
            public DateTime ArrivalDate { get { return _arrivaldate; } }

            private PerGroupCollection _groups;
            public PerGroupCollection Groups { get { return _groups; } }

            public int PCsRequiringUpdate
            {
                get
                {
                    // Loop through all groups, and start counting PCs requiring the update where it is approvable now
                    int PCs = 0;

                    foreach (PerGroupInformation gi in _groups)
                        if (!gi.Approved.HasValue && gi.UpdateApprovableNow && gi.PCs.HasValue) PCs += gi.PCs.Value;

                    // Return PC count
                    return PCs;
                }
            }

            public string SortIndex
            {
                get
                {
                    List<PerGroupInformation> sortedbyweight = new List<PerGroupInformation>();

                    // Loop through each group, adding it to the list in the appropriate weighted order
                    foreach (PerGroupInformation i in _groups)
                    {
                        if (sortedbyweight.Count == 0)
                            // First item just gets added
                            sortedbyweight.Add(i);
                        else
                        {
                            // Assume we haven't added the item
                            bool added = false;

                            // Loop through each item, looking for a higher sort order than this item
                            for (int ix = 0; ix < sortedbyweight.Count; ix++)
                            {
                                if (sortedbyweight[ix].GroupRule.sortweight > i.GroupRule.sortweight)
                                {
                                    // Found one - insert it here
                                    sortedbyweight.Insert(ix, i);
                                    added = true;
                                    break;
                                }
                            }
                            
                            if (!added)
                                // Item wasn't added - therefore add it to the end of the list
                                sortedbyweight.Add(i);
                        }
                    }

                    string idx = "";

                    // Loop through the list, determining the appropriate sort order
                    foreach (PerGroupInformation i in sortedbyweight)
                    {
                        if (!i.PCs.HasValue || i.PCs == 0)
                            // If no PCs require this update (or it hasn't been initialized) it should be sorted lower in the list
                            idx += "Z";
                        else
                            // If PCs require this update, it should be sorted higher in the list
                            idx += "A";
                    }

                    return idx + _title;
                }
            }
        }

        public class UnapprovedUpdates : System.Collections.CollectionBase
        {
            private clsConfig.GroupUpdateRuleCollection groups;
            private Dictionary<string, int> _dict = new Dictionary<string, int>();
            private clsConfig cfg;

            // Class initialisation.  List of groups will be saved at creation time.
            public UnapprovedUpdates(clsConfig configobject)
            {
                // Store the config object for use
                cfg = configobject;

                // Store the groups to be managed, sorting them by display order
                groups = cfg.GroupUpdateRules;
                groups.SortByDisplayOrder();
            }

            public clsConfig.GroupUpdateRuleCollection Groups { get { return groups; } }

            /// <summary>
            /// Get an unapproved update item by index
            /// </summary>
            /// <param name="index">Index number of the unapproved update</param>
            public UnapprovedUpdate this[int index]
            {
                get { return (UnapprovedUpdate)this.List[index]; }
            }

            /// <summary>
            /// Get an unapproved update by GUID
            /// </summary>
            /// <param name="updatename"></param>
            /// <returns></returns>
            private UnapprovedUpdate this[string searchstring]
            {
                get
                {
                    // Does the key exist?
                    if (_dict.ContainsKey(searchstring))
                        // Found update, return it
                        return (UnapprovedUpdate)this.List[_dict[searchstring]];
                    else
                        // No update found, return null
                        return null;
                }
            }

            /// <summary>
            /// Add a new unapproved update to the list using the supplied details
            /// </summary>
            /// <param name="UpdateID">GUID of the update</param>
            /// <param name="UpdateName">Update Name</param>
            /// <param name="Description">Update Description</param>
            /// <param name="KBArticle">Knowledge Base Article</param>
            /// <returns>Object containing added update</returns>
            private UnapprovedUpdate Add(string UpdateID, string UpdateName, string Description, string KBArticle, DateTime ArrivalDate)
            {
                int idx = this.List.Add(new UnapprovedUpdate(groups, UpdateID, UpdateName, Description, KBArticle, ArrivalDate));
                _dict.Add(UpdateID, idx);
                //_dict.Add(UpdateName, idx);
                
                return (UnapprovedUpdate)this.List[idx];
            }

            private BackgroundWorker wrkUpdate = new BackgroundWorker();

            public void UpdateUnapprovedUpdates()
            {
                // Kick off background worker
                wrkUpdate.DoWork += wrkUpdate_DoWork;
                wrkUpdate.RunWorkerAsync();
                
                // Wait for completion
                while (wrkUpdate.IsBusy)
                    // Whilst background worker is running, let the system keep processing events.
                    Application.DoEvents();
            }

            void wrkUpdate_DoWork(object sender, DoWorkEventArgs e)
            {
                // Get list of unapproved updates
                DataTable uudt = cfg.wsus.GetUnapprovedUpdatesNew();

                // Empty collection and start populating based on results
                this.List.Clear();

                foreach (DataRow r in uudt.Rows)
                {
                    // Do we already know about this update?
                    UnapprovedUpdate uu = this[r["updateid"].ToString()];

                    if (uu == null)
                        // No we don't - add it.
                        uu = this.Add(r["updateid"].ToString(), r["defaulttitle"].ToString(), r["defaultdescription"].ToString(), r["knowledgebasearticle"].ToString(), (DateTime)r["arrivaldate"]);

                    // Find the group that this row refers to
                    PerGroupInformation gi = uu.Groups[r["pcgroup"].ToString()];

                    if (gi != null)
                    {
                        // Update the number of PCs requiring this update (null if none)
                        if (r["PCs"] == null || r["PCs"].ToString() == "")
                            gi.PCs = null;
                        else
                            gi.PCs = int.Parse(r["PCs"].ToString());

                        // Update the date this update was approved (null or empty if it hasn't)
                        if (r["approvaldate"] == null || r["approvaldate"].ToString() == "")
                            gi.Approved = null;
                        else
                            gi.Approved = DateTime.Parse(r["approvaldate"].ToString());
                    }
                }

                // Now that we have a full collection of results, find each child of "release day" and process it, followed by any children
                foreach (UnapprovedUpdate uu in this.List)
                {
                    // Loop through each group
                    foreach (PerGroupInformation gi in uu.Groups)
                    {
                        // Is this a child of "release day"?
                        if (gi.GroupRule.parentcomputergroup == null)
                            // Yes - process this rule and it's children
                            ProcessReleaseDayGroup(uu, gi);
                    }
                }
            }

            private void ProcessReleaseDayGroup(UnapprovedUpdate update, PerGroupInformation group)
            {
                // Calculate the date this update can be approved
                group.Approvable = update.ArrivalDate.Add(group.GroupRule.updateinterval);

                // Process child groups
                foreach (PerGroupInformation gi in update.Groups)
                {
                    if (gi.GroupRule.parentcomputergroup != null && gi.GroupRule.parentcomputergroup.Id == group.Group.Id)
                        // Found child - process this group
                        ProcessChildGroup(update, group, gi);
                }
            }

            private void ProcessChildGroup(UnapprovedUpdate update, PerGroupInformation parent, PerGroupInformation group)
            {
                // Has parent been approved?
                if (parent.Approved.HasValue)
                    // Yes - Update to be approved at parent.Approved + group.updateinterval
                    group.Approvable = parent.Approved.Value.Add(group.GroupRule.updateinterval);
                else
                    // No.  Is parent approvable at all?
                    if (!parent.Approvable.HasValue)
                        // Parent not approvable, therefore child not approvable
                        group.Approvable = null;
                    else
                        // Yes - do any parent PCs require the update?
                        if (parent.PCs.HasValue && parent.PCs > 0)
                            // Yes - this update may not be approved until it is approved for the parent group
                            group.Approvable = null;
                        else
                            // No - update to be approved at parent.Approvable + group.childupdateinterval
                            group.Approvable = parent.Approvable.Value.Add(group.GroupRule.childupdateinterval);
                // Recursively process child groups
                foreach (PerGroupInformation gi in update.Groups)
                {
                    if (gi.GroupRule.parentcomputergroup != null && gi.GroupRule.parentcomputergroup.Id == group.Group.Id)
                        // Found child - process this group
                        ProcessChildGroup(update, group, gi);
                }
            }
        }
    }
}
