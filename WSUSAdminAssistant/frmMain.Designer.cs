namespace WSUSAdminAssistant
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.timUpdateData = new System.Windows.Forms.Timer(this.components);
            this.gbxWorking = new System.Windows.Forms.GroupBox();
            this.picReloading = new System.Windows.Forms.PictureBox();
            this.lblReload = new System.Windows.Forms.Label();
            this.tabRefresh = new System.Windows.Forms.TabPage();
            this.tabSuperceded = new System.Windows.Forms.TabPage();
            this.grdSupercededUpdates = new System.Windows.Forms.DataGridView();
            this.suUpdateName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.suUpdateID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.suSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tlsSuperceded = new System.Windows.Forms.ToolStrip();
            this.lblUpdateCount = new System.Windows.Forms.ToolStripButton();
            this.butDeclineSelected = new System.Windows.Forms.ToolStripButton();
            this.butSelectNone = new System.Windows.Forms.ToolStripButton();
            this.butSelectAll = new System.Windows.Forms.ToolStripButton();
            this.tabServerRestarts = new System.Windows.Forms.TabPage();
            this.lstServers = new System.Windows.Forms.ListBox();
            this.tabWSUSNotCommunicating = new System.Windows.Forms.TabPage();
            this.grdWSUSNotCommunicting = new System.Windows.Forms.DataGridView();
            this.wnuServerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wnuLastSync = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wnuLastRollup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabEndpointFaults = new System.Windows.Forms.TabPage();
            this.splEndpoint = new System.Windows.Forms.SplitContainer();
            this.grdEndpoints = new System.Windows.Forms.DataGridView();
            this.epName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.epUpdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.epIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.epApprovedUpdates = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.epUpdateErrors = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.epComputerGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.epFault = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.epLastServerContact = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.epLastStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.epPing = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.epPingUpdated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlsEndpoint = new System.Windows.Forms.ToolStrip();
            this.butApproved = new System.Windows.Forms.ToolStripButton();
            this.butUpdateErrors = new System.Windows.Forms.ToolStripButton();
            this.butNotCommunicating = new System.Windows.Forms.ToolStripButton();
            this.butUnassigned = new System.Windows.Forms.ToolStripButton();
            this.butDefaultSusID = new System.Windows.Forms.ToolStripButton();
            this.butGroupRules = new System.Windows.Forms.ToolStripButton();
            this.grdTasks = new System.Windows.Forms.DataGridView();
            this.tskID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tskStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tskIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tskCommand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tskOutput = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabUnapprovedUpdates = new System.Windows.Forms.TabPage();
            this.grdUpdates = new System.Windows.Forms.DataGridView();
            this.UpdateName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uaUpdateID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Updated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SortOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.T = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.A = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.B = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ServerT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChemistT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChemistA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChemistB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChemistServerT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Testing = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlsFilterUpdates = new System.Windows.Forms.ToolStrip();
            this.tlmFilterUpdates = new System.Windows.Forms.ToolStripDropDownButton();
            this.tlsNoFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tliXP = new System.Windows.Forms.ToolStripMenuItem();
            this.tliVista = new System.Windows.Forms.ToolStripMenuItem();
            this.tli7 = new System.Windows.Forms.ToolStripMenuItem();
            this.tli8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.tli2003 = new System.Windows.Forms.ToolStripMenuItem();
            this.tli2008 = new System.Windows.Forms.ToolStripMenuItem();
            this.tli2008r2 = new System.Windows.Forms.ToolStripMenuItem();
            this.tli2012 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.tliOffice2003 = new System.Windows.Forms.ToolStripMenuItem();
            this.tliOffice2007 = new System.Windows.Forms.ToolStripMenuItem();
            this.tliOffice2010 = new System.Windows.Forms.ToolStripMenuItem();
            this.tliOffice2013 = new System.Windows.Forms.ToolStripMenuItem();
            this.tlsUpdateCount = new System.Windows.Forms.ToolStripButton();
            this.butApproveUpdates = new System.Windows.Forms.ToolStripButton();
            this.tlmSelections = new System.Windows.Forms.ToolStripDropDownButton();
            this.clearSelectionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.groupAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AselectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AdeselectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BselectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BdeselectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SselectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SdeselectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.chemistGroupAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CAselectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CAdeselectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chemistGroupBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CBselectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CBdeselectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.butDeclineUnapproved = new System.Windows.Forms.ToolStripButton();
            this.butCancelApprove = new System.Windows.Forms.ToolStripButton();
            this.tabAdminType = new System.Windows.Forms.TabControl();
            this.tabHome = new System.Windows.Forms.TabPage();
            this.lblWSUSStatus = new System.Windows.Forms.Label();
            this.lblSQLStatus = new System.Windows.Forms.Label();
            this.lblWSUSConnection = new System.Windows.Forms.Label();
            this.lblSQLConnection = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuWSUSServer = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuComputerGroupRules = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuGroupApprovalRules = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCredentials = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDefaultSusIDList = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuPreferences = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUtilities = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSUSWatcher = new System.Windows.Forms.ToolStripMenuItem();
            this.cmEndpoint = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.epDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.epGPUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.epGPUpdateForce = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.epResetSusID = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuResetAuth = new System.Windows.Forms.ToolStripMenuItem();
            this.epDetectNow = new System.Windows.Forms.ToolStripMenuItem();
            this.epReportNow = new System.Windows.Forms.ToolStripMenuItem();
            this.timTasks = new System.Windows.Forms.Timer(this.components);
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.gbxWorking.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picReloading)).BeginInit();
            this.tabSuperceded.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSupercededUpdates)).BeginInit();
            this.tlsSuperceded.SuspendLayout();
            this.tabServerRestarts.SuspendLayout();
            this.tabWSUSNotCommunicating.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdWSUSNotCommunicting)).BeginInit();
            this.tabEndpointFaults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splEndpoint)).BeginInit();
            this.splEndpoint.Panel1.SuspendLayout();
            this.splEndpoint.Panel2.SuspendLayout();
            this.splEndpoint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdEndpoints)).BeginInit();
            this.tlsEndpoint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdTasks)).BeginInit();
            this.tabUnapprovedUpdates.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdUpdates)).BeginInit();
            this.tlsFilterUpdates.SuspendLayout();
            this.tabAdminType.SuspendLayout();
            this.tabHome.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.cmEndpoint.SuspendLayout();
            this.SuspendLayout();
            // 
            // timUpdateData
            // 
            this.timUpdateData.Interval = 500;
            this.timUpdateData.Tick += new System.EventHandler(this.timUpdateData_Tick);
            // 
            // gbxWorking
            // 
            this.gbxWorking.Controls.Add(this.picReloading);
            this.gbxWorking.Controls.Add(this.lblReload);
            this.gbxWorking.Location = new System.Drawing.Point(68, 169);
            this.gbxWorking.Name = "gbxWorking";
            this.gbxWorking.Size = new System.Drawing.Size(240, 102);
            this.gbxWorking.TabIndex = 2;
            this.gbxWorking.TabStop = false;
            this.gbxWorking.Text = "Working...";
            this.gbxWorking.Visible = false;
            // 
            // picReloading
            // 
            this.picReloading.Image = ((System.Drawing.Image)(resources.GetObject("picReloading.Image")));
            this.picReloading.Location = new System.Drawing.Point(18, 29);
            this.picReloading.Name = "picReloading";
            this.picReloading.Size = new System.Drawing.Size(48, 50);
            this.picReloading.TabIndex = 1;
            this.picReloading.TabStop = false;
            // 
            // lblReload
            // 
            this.lblReload.AutoSize = true;
            this.lblReload.Location = new System.Drawing.Point(72, 46);
            this.lblReload.Name = "lblReload";
            this.lblReload.Size = new System.Drawing.Size(149, 13);
            this.lblReload.TabIndex = 0;
            this.lblReload.Text = "Please wait... reloading data...";
            // 
            // tabRefresh
            // 
            this.tabRefresh.Location = new System.Drawing.Point(4, 22);
            this.tabRefresh.Name = "tabRefresh";
            this.tabRefresh.Padding = new System.Windows.Forms.Padding(3);
            this.tabRefresh.Size = new System.Drawing.Size(1038, 459);
            this.tabRefresh.TabIndex = 2;
            this.tabRefresh.Text = "Refresh";
            this.tabRefresh.UseVisualStyleBackColor = true;
            // 
            // tabSuperceded
            // 
            this.tabSuperceded.Controls.Add(this.grdSupercededUpdates);
            this.tabSuperceded.Controls.Add(this.tlsSuperceded);
            this.tabSuperceded.Location = new System.Drawing.Point(4, 22);
            this.tabSuperceded.Name = "tabSuperceded";
            this.tabSuperceded.Padding = new System.Windows.Forms.Padding(3);
            this.tabSuperceded.Size = new System.Drawing.Size(1038, 459);
            this.tabSuperceded.TabIndex = 5;
            this.tabSuperceded.Text = "Superceded Updates";
            this.tabSuperceded.UseVisualStyleBackColor = true;
            // 
            // grdSupercededUpdates
            // 
            this.grdSupercededUpdates.AllowUserToAddRows = false;
            this.grdSupercededUpdates.AllowUserToDeleteRows = false;
            this.grdSupercededUpdates.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.grdSupercededUpdates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdSupercededUpdates.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.suUpdateName,
            this.suUpdateID,
            this.suSelect});
            this.grdSupercededUpdates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSupercededUpdates.Location = new System.Drawing.Point(3, 28);
            this.grdSupercededUpdates.Name = "grdSupercededUpdates";
            this.grdSupercededUpdates.Size = new System.Drawing.Size(1032, 428);
            this.grdSupercededUpdates.TabIndex = 0;
            // 
            // suUpdateName
            // 
            this.suUpdateName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.suUpdateName.HeaderText = "Update Name";
            this.suUpdateName.Name = "suUpdateName";
            this.suUpdateName.ReadOnly = true;
            // 
            // suUpdateID
            // 
            this.suUpdateID.HeaderText = "Update ID";
            this.suUpdateID.Name = "suUpdateID";
            this.suUpdateID.ReadOnly = true;
            this.suUpdateID.Visible = false;
            this.suUpdateID.Width = 81;
            // 
            // suSelect
            // 
            this.suSelect.HeaderText = "Select";
            this.suSelect.Name = "suSelect";
            this.suSelect.Width = 43;
            // 
            // tlsSuperceded
            // 
            this.tlsSuperceded.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblUpdateCount,
            this.butDeclineSelected,
            this.butSelectNone,
            this.butSelectAll});
            this.tlsSuperceded.Location = new System.Drawing.Point(3, 3);
            this.tlsSuperceded.Name = "tlsSuperceded";
            this.tlsSuperceded.Size = new System.Drawing.Size(1032, 25);
            this.tlsSuperceded.TabIndex = 1;
            this.tlsSuperceded.Text = "toolStrip1";
            // 
            // lblUpdateCount
            // 
            this.lblUpdateCount.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.lblUpdateCount.Image = ((System.Drawing.Image)(resources.GetObject("lblUpdateCount.Image")));
            this.lblUpdateCount.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.lblUpdateCount.Name = "lblUpdateCount";
            this.lblUpdateCount.Size = new System.Drawing.Size(23, 22);
            // 
            // butDeclineSelected
            // 
            this.butDeclineSelected.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.butDeclineSelected.Image = ((System.Drawing.Image)(resources.GetObject("butDeclineSelected.Image")));
            this.butDeclineSelected.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.butDeclineSelected.Name = "butDeclineSelected";
            this.butDeclineSelected.Size = new System.Drawing.Size(159, 22);
            this.butDeclineSelected.Text = "Decline Selected Updates";
            this.butDeclineSelected.Click += new System.EventHandler(this.butDeclineSelected_Click);
            // 
            // butSelectNone
            // 
            this.butSelectNone.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.butSelectNone.Image = ((System.Drawing.Image)(resources.GetObject("butSelectNone.Image")));
            this.butSelectNone.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.butSelectNone.Name = "butSelectNone";
            this.butSelectNone.Size = new System.Drawing.Size(90, 22);
            this.butSelectNone.Text = "Select None";
            this.butSelectNone.Click += new System.EventHandler(this.butSelectNone_Click);
            // 
            // butSelectAll
            // 
            this.butSelectAll.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.butSelectAll.Image = ((System.Drawing.Image)(resources.GetObject("butSelectAll.Image")));
            this.butSelectAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.butSelectAll.Name = "butSelectAll";
            this.butSelectAll.Size = new System.Drawing.Size(75, 22);
            this.butSelectAll.Text = "Select All";
            this.butSelectAll.Click += new System.EventHandler(this.butSelectAll_Click);
            // 
            // tabServerRestarts
            // 
            this.tabServerRestarts.Controls.Add(this.lstServers);
            this.tabServerRestarts.Location = new System.Drawing.Point(4, 22);
            this.tabServerRestarts.Name = "tabServerRestarts";
            this.tabServerRestarts.Padding = new System.Windows.Forms.Padding(3);
            this.tabServerRestarts.Size = new System.Drawing.Size(1038, 459);
            this.tabServerRestarts.TabIndex = 3;
            this.tabServerRestarts.Text = "Servers Requiring Restarts";
            this.tabServerRestarts.UseVisualStyleBackColor = true;
            // 
            // lstServers
            // 
            this.lstServers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstServers.FormattingEnabled = true;
            this.lstServers.Location = new System.Drawing.Point(3, 3);
            this.lstServers.Name = "lstServers";
            this.lstServers.Size = new System.Drawing.Size(1032, 453);
            this.lstServers.TabIndex = 0;
            // 
            // tabWSUSNotCommunicating
            // 
            this.tabWSUSNotCommunicating.Controls.Add(this.grdWSUSNotCommunicting);
            this.tabWSUSNotCommunicating.Location = new System.Drawing.Point(4, 22);
            this.tabWSUSNotCommunicating.Name = "tabWSUSNotCommunicating";
            this.tabWSUSNotCommunicating.Size = new System.Drawing.Size(1038, 459);
            this.tabWSUSNotCommunicating.TabIndex = 7;
            this.tabWSUSNotCommunicating.Text = "WSUS servers not communicating";
            this.tabWSUSNotCommunicating.UseVisualStyleBackColor = true;
            // 
            // grdWSUSNotCommunicting
            // 
            this.grdWSUSNotCommunicting.AllowUserToAddRows = false;
            this.grdWSUSNotCommunicting.AllowUserToDeleteRows = false;
            this.grdWSUSNotCommunicting.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdWSUSNotCommunicting.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.wnuServerName,
            this.wnuLastSync,
            this.wnuLastRollup});
            this.grdWSUSNotCommunicting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdWSUSNotCommunicting.Location = new System.Drawing.Point(0, 0);
            this.grdWSUSNotCommunicting.Name = "grdWSUSNotCommunicting";
            this.grdWSUSNotCommunicting.ReadOnly = true;
            this.grdWSUSNotCommunicting.Size = new System.Drawing.Size(1038, 459);
            this.grdWSUSNotCommunicting.TabIndex = 2;
            // 
            // wnuServerName
            // 
            this.wnuServerName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.wnuServerName.HeaderText = "Server Name";
            this.wnuServerName.Name = "wnuServerName";
            this.wnuServerName.ReadOnly = true;
            // 
            // wnuLastSync
            // 
            this.wnuLastSync.HeaderText = "Last Sync";
            this.wnuLastSync.Name = "wnuLastSync";
            this.wnuLastSync.ReadOnly = true;
            this.wnuLastSync.Width = 150;
            // 
            // wnuLastRollup
            // 
            this.wnuLastRollup.HeaderText = "Last Rollup Time";
            this.wnuLastRollup.Name = "wnuLastRollup";
            this.wnuLastRollup.ReadOnly = true;
            this.wnuLastRollup.Width = 150;
            // 
            // tabEndpointFaults
            // 
            this.tabEndpointFaults.Controls.Add(this.splEndpoint);
            this.tabEndpointFaults.Location = new System.Drawing.Point(4, 22);
            this.tabEndpointFaults.Name = "tabEndpointFaults";
            this.tabEndpointFaults.Size = new System.Drawing.Size(1038, 459);
            this.tabEndpointFaults.TabIndex = 9;
            this.tabEndpointFaults.Text = "Endpoint Faults";
            this.tabEndpointFaults.UseVisualStyleBackColor = true;
            // 
            // splEndpoint
            // 
            this.splEndpoint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splEndpoint.Location = new System.Drawing.Point(0, 0);
            this.splEndpoint.Name = "splEndpoint";
            this.splEndpoint.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splEndpoint.Panel1
            // 
            this.splEndpoint.Panel1.Controls.Add(this.grdEndpoints);
            this.splEndpoint.Panel1.Controls.Add(this.tlsEndpoint);
            // 
            // splEndpoint.Panel2
            // 
            this.splEndpoint.Panel2.Controls.Add(this.grdTasks);
            this.splEndpoint.Size = new System.Drawing.Size(1038, 459);
            this.splEndpoint.SplitterDistance = 353;
            this.splEndpoint.TabIndex = 3;
            // 
            // grdEndpoints
            // 
            this.grdEndpoints.AllowUserToAddRows = false;
            this.grdEndpoints.AllowUserToDeleteRows = false;
            this.grdEndpoints.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdEndpoints.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.epName,
            this.epUpdate,
            this.epIP,
            this.epApprovedUpdates,
            this.epUpdateErrors,
            this.epComputerGroup,
            this.epFault,
            this.epLastServerContact,
            this.epLastStatus,
            this.epPing,
            this.epPingUpdated});
            this.grdEndpoints.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdEndpoints.Location = new System.Drawing.Point(0, 25);
            this.grdEndpoints.MultiSelect = false;
            this.grdEndpoints.Name = "grdEndpoints";
            this.grdEndpoints.ReadOnly = true;
            this.grdEndpoints.Size = new System.Drawing.Size(1038, 328);
            this.grdEndpoints.TabIndex = 4;
            this.grdEndpoints.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grdEndpoints_CellMouseClick);
            this.grdEndpoints.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.grdEndpoints_SortCompare);
            // 
            // epName
            // 
            this.epName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.epName.HeaderText = "PC Name";
            this.epName.Name = "epName";
            this.epName.ReadOnly = true;
            // 
            // epUpdate
            // 
            this.epUpdate.HeaderText = "Update";
            this.epUpdate.Name = "epUpdate";
            this.epUpdate.ReadOnly = true;
            this.epUpdate.Visible = false;
            // 
            // epIP
            // 
            this.epIP.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.epIP.HeaderText = "IP Address";
            this.epIP.Name = "epIP";
            this.epIP.ReadOnly = true;
            this.epIP.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // epApprovedUpdates
            // 
            this.epApprovedUpdates.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.epApprovedUpdates.HeaderText = "Approved Updates";
            this.epApprovedUpdates.Name = "epApprovedUpdates";
            this.epApprovedUpdates.ReadOnly = true;
            this.epApprovedUpdates.Width = 70;
            // 
            // epUpdateErrors
            // 
            this.epUpdateErrors.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.epUpdateErrors.HeaderText = "Updates With Errors";
            this.epUpdateErrors.Name = "epUpdateErrors";
            this.epUpdateErrors.ReadOnly = true;
            this.epUpdateErrors.Width = 92;
            // 
            // epComputerGroup
            // 
            this.epComputerGroup.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.epComputerGroup.HeaderText = "Computer Group";
            this.epComputerGroup.Name = "epComputerGroup";
            this.epComputerGroup.ReadOnly = true;
            // 
            // epFault
            // 
            this.epFault.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.epFault.HeaderText = "Endpoint Fault";
            this.epFault.Name = "epFault";
            this.epFault.ReadOnly = true;
            this.epFault.Width = 92;
            // 
            // epLastServerContact
            // 
            this.epLastServerContact.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.epLastServerContact.HeaderText = "Last Server Contact";
            this.epLastServerContact.Name = "epLastServerContact";
            this.epLastServerContact.ReadOnly = true;
            this.epLastServerContact.Width = 115;
            // 
            // epLastStatus
            // 
            this.epLastStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.epLastStatus.HeaderText = "Last Status";
            this.epLastStatus.Name = "epLastStatus";
            this.epLastStatus.ReadOnly = true;
            this.epLastStatus.Width = 78;
            // 
            // epPing
            // 
            this.epPing.HeaderText = "Ping";
            this.epPing.Name = "epPing";
            this.epPing.ReadOnly = true;
            // 
            // epPingUpdated
            // 
            this.epPingUpdated.HeaderText = "Updated";
            this.epPingUpdated.Name = "epPingUpdated";
            this.epPingUpdated.ReadOnly = true;
            this.epPingUpdated.Visible = false;
            // 
            // tlsEndpoint
            // 
            this.tlsEndpoint.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tlsEndpoint.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.butApproved,
            this.butUpdateErrors,
            this.butNotCommunicating,
            this.butUnassigned,
            this.butDefaultSusID,
            this.butGroupRules});
            this.tlsEndpoint.Location = new System.Drawing.Point(0, 0);
            this.tlsEndpoint.Name = "tlsEndpoint";
            this.tlsEndpoint.Size = new System.Drawing.Size(1038, 25);
            this.tlsEndpoint.TabIndex = 3;
            // 
            // butApproved
            // 
            this.butApproved.Checked = true;
            this.butApproved.CheckState = System.Windows.Forms.CheckState.Checked;
            this.butApproved.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.butApproved.Name = "butApproved";
            this.butApproved.Size = new System.Drawing.Size(187, 22);
            this.butApproved.Text = "Approved but Unapplied Updates";
            this.butApproved.Click += new System.EventHandler(this.butApproved_Click);
            // 
            // butUpdateErrors
            // 
            this.butUpdateErrors.Checked = true;
            this.butUpdateErrors.CheckState = System.Windows.Forms.CheckState.Checked;
            this.butUpdateErrors.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.butUpdateErrors.Name = "butUpdateErrors";
            this.butUpdateErrors.Size = new System.Drawing.Size(113, 22);
            this.butUpdateErrors.Text = "Updates with Errors";
            this.butUpdateErrors.Click += new System.EventHandler(this.butUpdateErrors_Click);
            // 
            // butNotCommunicating
            // 
            this.butNotCommunicating.Checked = true;
            this.butNotCommunicating.CheckState = System.Windows.Forms.CheckState.Checked;
            this.butNotCommunicating.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.butNotCommunicating.Name = "butNotCommunicating";
            this.butNotCommunicating.Size = new System.Drawing.Size(121, 22);
            this.butNotCommunicating.Text = "Not Communicating";
            this.butNotCommunicating.Click += new System.EventHandler(this.butNotCommunicating_Click);
            // 
            // butUnassigned
            // 
            this.butUnassigned.Checked = true;
            this.butUnassigned.CheckState = System.Windows.Forms.CheckState.Checked;
            this.butUnassigned.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.butUnassigned.Name = "butUnassigned";
            this.butUnassigned.Size = new System.Drawing.Size(141, 22);
            this.butUnassigned.Text = "Not Assigned to a Group";
            this.butUnassigned.Click += new System.EventHandler(this.butUnassigned_Click);
            // 
            // butDefaultSusID
            // 
            this.butDefaultSusID.Checked = true;
            this.butDefaultSusID.CheckState = System.Windows.Forms.CheckState.Checked;
            this.butDefaultSusID.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.butDefaultSusID.Name = "butDefaultSusID";
            this.butDefaultSusID.Size = new System.Drawing.Size(86, 22);
            this.butDefaultSusID.Text = "Default SUS ID";
            this.butDefaultSusID.Click += new System.EventHandler(this.butDefaultSusID_Click);
            // 
            // butGroupRules
            // 
            this.butGroupRules.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.butGroupRules.Name = "butGroupRules";
            this.butGroupRules.Size = new System.Drawing.Size(143, 22);
            this.butGroupRules.Text = "PCs not in Correct Group";
            this.butGroupRules.Click += new System.EventHandler(this.butGroupRules_Click);
            // 
            // grdTasks
            // 
            this.grdTasks.AllowUserToAddRows = false;
            this.grdTasks.AllowUserToDeleteRows = false;
            this.grdTasks.AllowUserToResizeColumns = false;
            this.grdTasks.AllowUserToResizeRows = false;
            this.grdTasks.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.grdTasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdTasks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tskID,
            this.tskStatus,
            this.tskIP,
            this.tskCommand,
            this.tskOutput});
            this.grdTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdTasks.Location = new System.Drawing.Point(0, 0);
            this.grdTasks.Name = "grdTasks";
            this.grdTasks.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.grdTasks.Size = new System.Drawing.Size(1038, 102);
            this.grdTasks.TabIndex = 0;
            // 
            // tskID
            // 
            this.tskID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.tskID.DataPropertyName = "TaskID";
            this.tskID.HeaderText = "Task ID";
            this.tskID.Name = "tskID";
            this.tskID.ReadOnly = true;
            this.tskID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.tskID.Width = 51;
            // 
            // tskStatus
            // 
            this.tskStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.tskStatus.DataPropertyName = "CurrentStatus";
            this.tskStatus.HeaderText = "Status";
            this.tskStatus.Name = "tskStatus";
            this.tskStatus.ReadOnly = true;
            this.tskStatus.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.tskStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.tskStatus.Width = 43;
            // 
            // tskIP
            // 
            this.tskIP.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.tskIP.DataPropertyName = "IPAddress";
            this.tskIP.HeaderText = "IP Address";
            this.tskIP.Name = "tskIP";
            this.tskIP.ReadOnly = true;
            this.tskIP.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.tskIP.Width = 64;
            // 
            // tskCommand
            // 
            this.tskCommand.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.tskCommand.DataPropertyName = "Command";
            this.tskCommand.HeaderText = "Command";
            this.tskCommand.Name = "tskCommand";
            this.tskCommand.ReadOnly = true;
            this.tskCommand.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.tskCommand.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.tskCommand.Width = 60;
            // 
            // tskOutput
            // 
            this.tskOutput.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.tskOutput.HeaderText = "Output";
            this.tskOutput.Name = "tskOutput";
            this.tskOutput.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // tabUnapprovedUpdates
            // 
            this.tabUnapprovedUpdates.Controls.Add(this.grdUpdates);
            this.tabUnapprovedUpdates.Controls.Add(this.tlsFilterUpdates);
            this.tabUnapprovedUpdates.Location = new System.Drawing.Point(4, 22);
            this.tabUnapprovedUpdates.Name = "tabUnapprovedUpdates";
            this.tabUnapprovedUpdates.Padding = new System.Windows.Forms.Padding(3);
            this.tabUnapprovedUpdates.Size = new System.Drawing.Size(1038, 459);
            this.tabUnapprovedUpdates.TabIndex = 0;
            this.tabUnapprovedUpdates.Text = "Unapproved updates";
            this.tabUnapprovedUpdates.UseVisualStyleBackColor = true;
            // 
            // grdUpdates
            // 
            this.grdUpdates.AllowUserToAddRows = false;
            this.grdUpdates.AllowUserToDeleteRows = false;
            this.grdUpdates.AllowUserToResizeColumns = false;
            this.grdUpdates.AllowUserToResizeRows = false;
            this.grdUpdates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdUpdates.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.UpdateName,
            this.uaUpdateID,
            this.Updated,
            this.Description,
            this.SortOrder,
            this.KB,
            this.T,
            this.A,
            this.B,
            this.ServerT,
            this.ChemistT,
            this.ChemistA,
            this.ChemistB,
            this.ChemistServerT,
            this.Testing});
            this.grdUpdates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdUpdates.Location = new System.Drawing.Point(3, 28);
            this.grdUpdates.Name = "grdUpdates";
            this.grdUpdates.ReadOnly = true;
            this.grdUpdates.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.grdUpdates.Size = new System.Drawing.Size(1032, 428);
            this.grdUpdates.TabIndex = 3;
            this.grdUpdates.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grdUpdates_CellMouseClick);
            // 
            // UpdateName
            // 
            this.UpdateName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.UpdateName.FillWeight = 50F;
            this.UpdateName.HeaderText = "Update Name";
            this.UpdateName.Name = "UpdateName";
            this.UpdateName.ReadOnly = true;
            this.UpdateName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.UpdateName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.UpdateName.Width = 71;
            // 
            // uaUpdateID
            // 
            this.uaUpdateID.HeaderText = "Update ID";
            this.uaUpdateID.Name = "uaUpdateID";
            this.uaUpdateID.ReadOnly = true;
            this.uaUpdateID.Visible = false;
            // 
            // Updated
            // 
            this.Updated.HeaderText = "Updated";
            this.Updated.Name = "Updated";
            this.Updated.ReadOnly = true;
            this.Updated.Visible = false;
            // 
            // Description
            // 
            this.Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            // 
            // SortOrder
            // 
            this.SortOrder.HeaderText = "SortOrder";
            this.SortOrder.Name = "SortOrder";
            this.SortOrder.ReadOnly = true;
            this.SortOrder.Visible = false;
            // 
            // KB
            // 
            this.KB.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.KB.HeaderText = "KB Article";
            this.KB.Name = "KB";
            this.KB.ReadOnly = true;
            this.KB.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.KB.Width = 72;
            // 
            // T
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.T.DefaultCellStyle = dataGridViewCellStyle1;
            this.T.HeaderText = "Group T";
            this.T.Name = "T";
            this.T.ReadOnly = true;
            this.T.Width = 55;
            // 
            // A
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.A.DefaultCellStyle = dataGridViewCellStyle2;
            this.A.HeaderText = "Group A";
            this.A.Name = "A";
            this.A.ReadOnly = true;
            this.A.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.A.Width = 55;
            // 
            // B
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.B.DefaultCellStyle = dataGridViewCellStyle3;
            this.B.HeaderText = "Group B";
            this.B.Name = "B";
            this.B.ReadOnly = true;
            this.B.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.B.Width = 55;
            // 
            // ServerT
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ServerT.DefaultCellStyle = dataGridViewCellStyle4;
            this.ServerT.HeaderText = "Servers T";
            this.ServerT.Name = "ServerT";
            this.ServerT.ReadOnly = true;
            this.ServerT.Width = 60;
            // 
            // ChemistT
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ChemistT.DefaultCellStyle = dataGridViewCellStyle5;
            this.ChemistT.HeaderText = "Chemist T";
            this.ChemistT.Name = "ChemistT";
            this.ChemistT.ReadOnly = true;
            this.ChemistT.Width = 60;
            // 
            // ChemistA
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ChemistA.DefaultCellStyle = dataGridViewCellStyle6;
            this.ChemistA.HeaderText = "Chemist A";
            this.ChemistA.Name = "ChemistA";
            this.ChemistA.ReadOnly = true;
            this.ChemistA.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ChemistA.Width = 60;
            // 
            // ChemistB
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ChemistB.DefaultCellStyle = dataGridViewCellStyle7;
            this.ChemistB.HeaderText = "Chemist B";
            this.ChemistB.Name = "ChemistB";
            this.ChemistB.ReadOnly = true;
            this.ChemistB.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ChemistB.Width = 60;
            // 
            // ChemistServerT
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ChemistServerT.DefaultCellStyle = dataGridViewCellStyle8;
            this.ChemistServerT.HeaderText = "Chemist Servers T";
            this.ChemistServerT.Name = "ChemistServerT";
            this.ChemistServerT.ReadOnly = true;
            this.ChemistServerT.Width = 60;
            // 
            // Testing
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Testing.DefaultCellStyle = dataGridViewCellStyle9;
            this.Testing.HeaderText = "Testing";
            this.Testing.Name = "Testing";
            this.Testing.ReadOnly = true;
            this.Testing.Width = 55;
            // 
            // tlsFilterUpdates
            // 
            this.tlsFilterUpdates.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlmFilterUpdates,
            this.tlsUpdateCount,
            this.butApproveUpdates,
            this.tlmSelections,
            this.butDeclineUnapproved,
            this.butCancelApprove});
            this.tlsFilterUpdates.Location = new System.Drawing.Point(3, 3);
            this.tlsFilterUpdates.Name = "tlsFilterUpdates";
            this.tlsFilterUpdates.Size = new System.Drawing.Size(1032, 25);
            this.tlsFilterUpdates.TabIndex = 4;
            this.tlsFilterUpdates.Text = "Filter Updates";
            // 
            // tlmFilterUpdates
            // 
            this.tlmFilterUpdates.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tlmFilterUpdates.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlsNoFilter,
            this.toolStripMenuItem1,
            this.tliXP,
            this.tliVista,
            this.tli7,
            this.tli8,
            this.toolStripMenuItem2,
            this.tli2003,
            this.tli2008,
            this.tli2008r2,
            this.tli2012,
            this.toolStripMenuItem3,
            this.tliOffice2003,
            this.tliOffice2007,
            this.tliOffice2010,
            this.tliOffice2013});
            this.tlmFilterUpdates.Image = ((System.Drawing.Image)(resources.GetObject("tlmFilterUpdates.Image")));
            this.tlmFilterUpdates.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlmFilterUpdates.Name = "tlmFilterUpdates";
            this.tlmFilterUpdates.Size = new System.Drawing.Size(92, 22);
            this.tlmFilterUpdates.Text = "Filter Updates";
            // 
            // tlsNoFilter
            // 
            this.tlsNoFilter.Name = "tlsNoFilter";
            this.tlsNoFilter.Size = new System.Drawing.Size(151, 22);
            this.tlsNoFilter.Text = "No Filtering";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(148, 6);
            // 
            // tliXP
            // 
            this.tliXP.Name = "tliXP";
            this.tliXP.Size = new System.Drawing.Size(151, 22);
            this.tliXP.Text = "Windows XP";
            // 
            // tliVista
            // 
            this.tliVista.Name = "tliVista";
            this.tliVista.Size = new System.Drawing.Size(151, 22);
            this.tliVista.Text = "Windows Vista";
            // 
            // tli7
            // 
            this.tli7.Name = "tli7";
            this.tli7.Size = new System.Drawing.Size(151, 22);
            this.tli7.Text = "Windows 7";
            // 
            // tli8
            // 
            this.tli8.Name = "tli8";
            this.tli8.Size = new System.Drawing.Size(151, 22);
            this.tli8.Text = "Windows 8";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(148, 6);
            // 
            // tli2003
            // 
            this.tli2003.Name = "tli2003";
            this.tli2003.Size = new System.Drawing.Size(151, 22);
            this.tli2003.Text = "Server 2003";
            // 
            // tli2008
            // 
            this.tli2008.Name = "tli2008";
            this.tli2008.Size = new System.Drawing.Size(151, 22);
            this.tli2008.Text = "Server 2008";
            // 
            // tli2008r2
            // 
            this.tli2008r2.Name = "tli2008r2";
            this.tli2008r2.Size = new System.Drawing.Size(151, 22);
            this.tli2008r2.Text = "Server 2008 R2";
            // 
            // tli2012
            // 
            this.tli2012.Name = "tli2012";
            this.tli2012.Size = new System.Drawing.Size(151, 22);
            this.tli2012.Text = "Server 2012";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(148, 6);
            // 
            // tliOffice2003
            // 
            this.tliOffice2003.Name = "tliOffice2003";
            this.tliOffice2003.Size = new System.Drawing.Size(151, 22);
            this.tliOffice2003.Text = "Office 2003";
            // 
            // tliOffice2007
            // 
            this.tliOffice2007.Name = "tliOffice2007";
            this.tliOffice2007.Size = new System.Drawing.Size(151, 22);
            this.tliOffice2007.Text = "Office 2007";
            // 
            // tliOffice2010
            // 
            this.tliOffice2010.Name = "tliOffice2010";
            this.tliOffice2010.Size = new System.Drawing.Size(151, 22);
            this.tliOffice2010.Text = "Office 2010";
            // 
            // tliOffice2013
            // 
            this.tliOffice2013.Name = "tliOffice2013";
            this.tliOffice2013.Size = new System.Drawing.Size(151, 22);
            this.tliOffice2013.Text = "Office 2013";
            // 
            // tlsUpdateCount
            // 
            this.tlsUpdateCount.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tlsUpdateCount.Image = ((System.Drawing.Image)(resources.GetObject("tlsUpdateCount.Image")));
            this.tlsUpdateCount.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlsUpdateCount.Name = "tlsUpdateCount";
            this.tlsUpdateCount.Size = new System.Drawing.Size(23, 22);
            // 
            // butApproveUpdates
            // 
            this.butApproveUpdates.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.butApproveUpdates.Image = global::WSUSAdminAssistant.Properties.Resources.OK;
            this.butApproveUpdates.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.butApproveUpdates.Name = "butApproveUpdates";
            this.butApproveUpdates.Size = new System.Drawing.Size(165, 22);
            this.butApproveUpdates.Text = "Approve Selected Updates";
            this.butApproveUpdates.Click += new System.EventHandler(this.butApproveUpdates_Click);
            // 
            // tlmSelections
            // 
            this.tlmSelections.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tlmSelections.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearSelectionsToolStripMenuItem,
            this.toolStripSeparator2,
            this.toolStripSeparator1,
            this.groupAToolStripMenuItem,
            this.groupBToolStripMenuItem,
            this.groupSToolStripMenuItem,
            this.toolStripSeparator3,
            this.chemistGroupAToolStripMenuItem,
            this.chemistGroupBToolStripMenuItem});
            this.tlmSelections.Image = ((System.Drawing.Image)(resources.GetObject("tlmSelections.Image")));
            this.tlmSelections.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlmSelections.Name = "tlmSelections";
            this.tlmSelections.Size = new System.Drawing.Size(151, 22);
            this.tlmSelections.Text = "Select Updates in Groups";
            // 
            // clearSelectionsToolStripMenuItem
            // 
            this.clearSelectionsToolStripMenuItem.Name = "clearSelectionsToolStripMenuItem";
            this.clearSelectionsToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.clearSelectionsToolStripMenuItem.Text = "Clear Selections";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(162, 6);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(162, 6);
            // 
            // groupAToolStripMenuItem
            // 
            this.groupAToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AselectToolStripMenuItem,
            this.AdeselectToolStripMenuItem});
            this.groupAToolStripMenuItem.Name = "groupAToolStripMenuItem";
            this.groupAToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.groupAToolStripMenuItem.Text = "Group A";
            // 
            // AselectToolStripMenuItem
            // 
            this.AselectToolStripMenuItem.Name = "AselectToolStripMenuItem";
            this.AselectToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.AselectToolStripMenuItem.Text = "Select";
            // 
            // AdeselectToolStripMenuItem
            // 
            this.AdeselectToolStripMenuItem.Name = "AdeselectToolStripMenuItem";
            this.AdeselectToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.AdeselectToolStripMenuItem.Text = "Deselect";
            // 
            // groupBToolStripMenuItem
            // 
            this.groupBToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BselectToolStripMenuItem,
            this.BdeselectToolStripMenuItem});
            this.groupBToolStripMenuItem.Name = "groupBToolStripMenuItem";
            this.groupBToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.groupBToolStripMenuItem.Text = "Group B";
            // 
            // BselectToolStripMenuItem
            // 
            this.BselectToolStripMenuItem.Name = "BselectToolStripMenuItem";
            this.BselectToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.BselectToolStripMenuItem.Text = "Select";
            // 
            // BdeselectToolStripMenuItem
            // 
            this.BdeselectToolStripMenuItem.Name = "BdeselectToolStripMenuItem";
            this.BdeselectToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.BdeselectToolStripMenuItem.Text = "Deselect";
            // 
            // groupSToolStripMenuItem
            // 
            this.groupSToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SselectToolStripMenuItem,
            this.SdeselectToolStripMenuItem});
            this.groupSToolStripMenuItem.Name = "groupSToolStripMenuItem";
            this.groupSToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.groupSToolStripMenuItem.Text = "Group S";
            // 
            // SselectToolStripMenuItem
            // 
            this.SselectToolStripMenuItem.Name = "SselectToolStripMenuItem";
            this.SselectToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.SselectToolStripMenuItem.Text = "Select";
            // 
            // SdeselectToolStripMenuItem
            // 
            this.SdeselectToolStripMenuItem.Name = "SdeselectToolStripMenuItem";
            this.SdeselectToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.SdeselectToolStripMenuItem.Text = "Deselect";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(162, 6);
            // 
            // chemistGroupAToolStripMenuItem
            // 
            this.chemistGroupAToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CAselectToolStripMenuItem,
            this.CAdeselectToolStripMenuItem});
            this.chemistGroupAToolStripMenuItem.Name = "chemistGroupAToolStripMenuItem";
            this.chemistGroupAToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.chemistGroupAToolStripMenuItem.Text = "Chemist Group A";
            // 
            // CAselectToolStripMenuItem
            // 
            this.CAselectToolStripMenuItem.Name = "CAselectToolStripMenuItem";
            this.CAselectToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.CAselectToolStripMenuItem.Text = "Select";
            // 
            // CAdeselectToolStripMenuItem
            // 
            this.CAdeselectToolStripMenuItem.Name = "CAdeselectToolStripMenuItem";
            this.CAdeselectToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.CAdeselectToolStripMenuItem.Text = "Deselect";
            // 
            // chemistGroupBToolStripMenuItem
            // 
            this.chemistGroupBToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CBselectToolStripMenuItem,
            this.CBdeselectToolStripMenuItem});
            this.chemistGroupBToolStripMenuItem.Name = "chemistGroupBToolStripMenuItem";
            this.chemistGroupBToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.chemistGroupBToolStripMenuItem.Text = "Chemist Group B";
            // 
            // CBselectToolStripMenuItem
            // 
            this.CBselectToolStripMenuItem.Name = "CBselectToolStripMenuItem";
            this.CBselectToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.CBselectToolStripMenuItem.Text = "Select";
            // 
            // CBdeselectToolStripMenuItem
            // 
            this.CBdeselectToolStripMenuItem.Name = "CBdeselectToolStripMenuItem";
            this.CBdeselectToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.CBdeselectToolStripMenuItem.Text = "Deselect";
            // 
            // butDeclineUnapproved
            // 
            this.butDeclineUnapproved.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.butDeclineUnapproved.Image = global::WSUSAdminAssistant.Properties.Resources.Critical;
            this.butDeclineUnapproved.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.butDeclineUnapproved.Name = "butDeclineUnapproved";
            this.butDeclineUnapproved.Size = new System.Drawing.Size(159, 22);
            this.butDeclineUnapproved.Text = "Decline Selected Updates";
            this.butDeclineUnapproved.Click += new System.EventHandler(this.butDeclineUnapproved_Click);
            // 
            // butCancelApprove
            // 
            this.butCancelApprove.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.butCancelApprove.Image = global::WSUSAdminAssistant.Properties.Resources.NoAction;
            this.butCancelApprove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.butCancelApprove.Name = "butCancelApprove";
            this.butCancelApprove.Size = new System.Drawing.Size(119, 22);
            this.butCancelApprove.Text = "Cancel Approvals";
            this.butCancelApprove.Visible = false;
            this.butCancelApprove.Click += new System.EventHandler(this.butCancelApprove_Click);
            // 
            // tabAdminType
            // 
            this.tabAdminType.Controls.Add(this.tabHome);
            this.tabAdminType.Controls.Add(this.tabUnapprovedUpdates);
            this.tabAdminType.Controls.Add(this.tabEndpointFaults);
            this.tabAdminType.Controls.Add(this.tabWSUSNotCommunicating);
            this.tabAdminType.Controls.Add(this.tabServerRestarts);
            this.tabAdminType.Controls.Add(this.tabSuperceded);
            this.tabAdminType.Controls.Add(this.tabRefresh);
            this.tabAdminType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabAdminType.Location = new System.Drawing.Point(0, 0);
            this.tabAdminType.Name = "tabAdminType";
            this.tabAdminType.SelectedIndex = 0;
            this.tabAdminType.Size = new System.Drawing.Size(1046, 485);
            this.tabAdminType.TabIndex = 0;
            this.tabAdminType.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabAdminType_Selecting);
            // 
            // tabHome
            // 
            this.tabHome.Controls.Add(this.lblWSUSStatus);
            this.tabHome.Controls.Add(this.lblSQLStatus);
            this.tabHome.Controls.Add(this.lblWSUSConnection);
            this.tabHome.Controls.Add(this.lblSQLConnection);
            this.tabHome.Controls.Add(this.menuStrip1);
            this.tabHome.Location = new System.Drawing.Point(4, 22);
            this.tabHome.Name = "tabHome";
            this.tabHome.Size = new System.Drawing.Size(1038, 459);
            this.tabHome.TabIndex = 10;
            this.tabHome.Text = "Home";
            this.tabHome.UseVisualStyleBackColor = true;
            // 
            // lblWSUSStatus
            // 
            this.lblWSUSStatus.AutoSize = true;
            this.lblWSUSStatus.Location = new System.Drawing.Point(189, 99);
            this.lblWSUSStatus.Name = "lblWSUSStatus";
            this.lblWSUSStatus.Size = new System.Drawing.Size(0, 13);
            this.lblWSUSStatus.TabIndex = 4;
            // 
            // lblSQLStatus
            // 
            this.lblSQLStatus.AutoSize = true;
            this.lblSQLStatus.Location = new System.Drawing.Point(189, 82);
            this.lblSQLStatus.Name = "lblSQLStatus";
            this.lblSQLStatus.Size = new System.Drawing.Size(0, 13);
            this.lblSQLStatus.TabIndex = 3;
            // 
            // lblWSUSConnection
            // 
            this.lblWSUSConnection.AutoSize = true;
            this.lblWSUSConnection.Location = new System.Drawing.Point(24, 99);
            this.lblWSUSConnection.Name = "lblWSUSConnection";
            this.lblWSUSConnection.Size = new System.Drawing.Size(134, 13);
            this.lblWSUSConnection.TabIndex = 2;
            this.lblWSUSConnection.Text = "WSUS Server Connection:";
            // 
            // lblSQLConnection
            // 
            this.lblSQLConnection.AutoSize = true;
            this.lblSQLConnection.Location = new System.Drawing.Point(24, 82);
            this.lblSQLConnection.Name = "lblSQLConnection";
            this.lblSQLConnection.Size = new System.Drawing.Size(122, 13);
            this.lblSQLConnection.TabIndex = 1;
            this.lblSQLConnection.Text = "SQL Server Connection:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuOptions,
            this.mnuUtilities});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1038, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnuOptions
            // 
            this.mnuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuWSUSServer,
            this.mnuComputerGroupRules,
            this.mnuGroupApprovalRules,
            this.mnuCredentials,
            this.mnuDefaultSusIDList,
            this.toolStripMenuItem4,
            this.mnuPreferences});
            this.mnuOptions.Name = "mnuOptions";
            this.mnuOptions.Size = new System.Drawing.Size(61, 20);
            this.mnuOptions.Text = "&Options";
            // 
            // mnuWSUSServer
            // 
            this.mnuWSUSServer.Name = "mnuWSUSServer";
            this.mnuWSUSServer.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.mnuWSUSServer.Size = new System.Drawing.Size(288, 22);
            this.mnuWSUSServer.Text = "&WSUS Server";
            this.mnuWSUSServer.Click += new System.EventHandler(this.mnuWSUSServer_Click);
            // 
            // mnuComputerGroupRules
            // 
            this.mnuComputerGroupRules.Name = "mnuComputerGroupRules";
            this.mnuComputerGroupRules.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.mnuComputerGroupRules.Size = new System.Drawing.Size(288, 22);
            this.mnuComputerGroupRules.Text = "Computer &Group Rules";
            this.mnuComputerGroupRules.Click += new System.EventHandler(this.mnuComputerGroupRules_Click);
            // 
            // mnuGroupApprovalRules
            // 
            this.mnuGroupApprovalRules.Name = "mnuGroupApprovalRules";
            this.mnuGroupApprovalRules.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.mnuGroupApprovalRules.Size = new System.Drawing.Size(288, 22);
            this.mnuGroupApprovalRules.Text = "Computer Group &Approval Rules";
            this.mnuGroupApprovalRules.Click += new System.EventHandler(this.mnuGroupApprovalRules_Click);
            // 
            // mnuCredentials
            // 
            this.mnuCredentials.Name = "mnuCredentials";
            this.mnuCredentials.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.mnuCredentials.Size = new System.Drawing.Size(288, 22);
            this.mnuCredentials.Text = "Security &Credentials";
            this.mnuCredentials.Click += new System.EventHandler(this.mnuCredentials_Click);
            // 
            // mnuDefaultSusIDList
            // 
            this.mnuDefaultSusIDList.Name = "mnuDefaultSusIDList";
            this.mnuDefaultSusIDList.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.mnuDefaultSusIDList.Size = new System.Drawing.Size(288, 22);
            this.mnuDefaultSusIDList.Text = "Default &SUS ID List";
            this.mnuDefaultSusIDList.Click += new System.EventHandler(this.mnuDefaultSusIDList_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(285, 6);
            // 
            // mnuPreferences
            // 
            this.mnuPreferences.Name = "mnuPreferences";
            this.mnuPreferences.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.mnuPreferences.Size = new System.Drawing.Size(288, 22);
            this.mnuPreferences.Text = "&Preferences";
            this.mnuPreferences.Click += new System.EventHandler(this.mnuPreferences_Click);
            // 
            // mnuUtilities
            // 
            this.mnuUtilities.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSUSWatcher});
            this.mnuUtilities.Name = "mnuUtilities";
            this.mnuUtilities.Size = new System.Drawing.Size(58, 20);
            this.mnuUtilities.Text = "&Utilities";
            // 
            // mnuSUSWatcher
            // 
            this.mnuSUSWatcher.Name = "mnuSUSWatcher";
            this.mnuSUSWatcher.Size = new System.Drawing.Size(208, 22);
            this.mnuSUSWatcher.Text = "Duplicate SUS ID Watcher";
            this.mnuSUSWatcher.Click += new System.EventHandler(this.mnuSUSWatcher_Click);
            // 
            // cmEndpoint
            // 
            this.cmEndpoint.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.epDetails,
            this.toolStripMenuItem6,
            this.epGPUpdate,
            this.epGPUpdateForce,
            this.toolStripMenuItem5,
            this.epResetSusID,
            this.mnuResetAuth,
            this.epDetectNow,
            this.epReportNow});
            this.cmEndpoint.Name = "cmEndpoint";
            this.cmEndpoint.Size = new System.Drawing.Size(231, 170);
            // 
            // epDetails
            // 
            this.epDetails.Enabled = false;
            this.epDetails.Name = "epDetails";
            this.epDetails.Size = new System.Drawing.Size(230, 22);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(227, 6);
            // 
            // epGPUpdate
            // 
            this.epGPUpdate.Name = "epGPUpdate";
            this.epGPUpdate.Size = new System.Drawing.Size(230, 22);
            this.epGPUpdate.Text = "&Group Policy Update";
            this.epGPUpdate.Click += new System.EventHandler(this.epGPUpdate_Click);
            // 
            // epGPUpdateForce
            // 
            this.epGPUpdateForce.Name = "epGPUpdateForce";
            this.epGPUpdateForce.Size = new System.Drawing.Size(230, 22);
            this.epGPUpdateForce.Text = "Group Policy Update (&Forced)";
            this.epGPUpdateForce.Click += new System.EventHandler(this.epGPUpdateForce_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(227, 6);
            // 
            // epResetSusID
            // 
            this.epResetSusID.Name = "epResetSusID";
            this.epResetSusID.Size = new System.Drawing.Size(230, 22);
            this.epResetSusID.Text = "&Reset SUS ID";
            this.epResetSusID.Click += new System.EventHandler(this.epResetSusID_Click);
            // 
            // mnuResetAuth
            // 
            this.mnuResetAuth.Name = "mnuResetAuth";
            this.mnuResetAuth.Size = new System.Drawing.Size(230, 22);
            this.mnuResetAuth.Text = "Reset &Authorisation Token";
            this.mnuResetAuth.Click += new System.EventHandler(this.mnuResetAuth_Click);
            // 
            // epDetectNow
            // 
            this.epDetectNow.Name = "epDetectNow";
            this.epDetectNow.Size = new System.Drawing.Size(230, 22);
            this.epDetectNow.Text = "&Detect Updates Now";
            this.epDetectNow.Click += new System.EventHandler(this.epDetectNow_Click);
            // 
            // epReportNow
            // 
            this.epReportNow.Name = "epReportNow";
            this.epReportNow.Size = new System.Drawing.Size(230, 22);
            this.epReportNow.Text = "&Report Update Status Now";
            this.epReportNow.Click += new System.EventHandler(this.epReportNow_Click);
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.HeaderText = "Status";
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1046, 485);
            this.Controls.Add(this.gbxWorking);
            this.Controls.Add(this.tabAdminType);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Text = "WSUS Administration Assistant";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_Closing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.gbxWorking.ResumeLayout(false);
            this.gbxWorking.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picReloading)).EndInit();
            this.tabSuperceded.ResumeLayout(false);
            this.tabSuperceded.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSupercededUpdates)).EndInit();
            this.tlsSuperceded.ResumeLayout(false);
            this.tlsSuperceded.PerformLayout();
            this.tabServerRestarts.ResumeLayout(false);
            this.tabWSUSNotCommunicating.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdWSUSNotCommunicting)).EndInit();
            this.tabEndpointFaults.ResumeLayout(false);
            this.splEndpoint.Panel1.ResumeLayout(false);
            this.splEndpoint.Panel1.PerformLayout();
            this.splEndpoint.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splEndpoint)).EndInit();
            this.splEndpoint.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdEndpoints)).EndInit();
            this.tlsEndpoint.ResumeLayout(false);
            this.tlsEndpoint.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdTasks)).EndInit();
            this.tabUnapprovedUpdates.ResumeLayout(false);
            this.tabUnapprovedUpdates.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdUpdates)).EndInit();
            this.tlsFilterUpdates.ResumeLayout(false);
            this.tlsFilterUpdates.PerformLayout();
            this.tabAdminType.ResumeLayout(false);
            this.tabHome.ResumeLayout(false);
            this.tabHome.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.cmEndpoint.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timUpdateData;
        private System.Windows.Forms.GroupBox gbxWorking;
        private System.Windows.Forms.Label lblReload;
        private System.Windows.Forms.PictureBox picReloading;
        private System.Windows.Forms.TabPage tabRefresh;
        private System.Windows.Forms.TabPage tabSuperceded;
        private System.Windows.Forms.DataGridView grdSupercededUpdates;
        private System.Windows.Forms.DataGridViewTextBoxColumn suUpdateName;
        private System.Windows.Forms.DataGridViewTextBoxColumn suUpdateID;
        private System.Windows.Forms.DataGridViewCheckBoxColumn suSelect;
        private System.Windows.Forms.ToolStrip tlsSuperceded;
        private System.Windows.Forms.ToolStripButton lblUpdateCount;
        private System.Windows.Forms.ToolStripButton butDeclineSelected;
        private System.Windows.Forms.ToolStripButton butSelectNone;
        private System.Windows.Forms.ToolStripButton butSelectAll;
        private System.Windows.Forms.TabPage tabServerRestarts;
        private System.Windows.Forms.ListBox lstServers;
        private System.Windows.Forms.TabPage tabWSUSNotCommunicating;
        private System.Windows.Forms.DataGridView grdWSUSNotCommunicting;
        private System.Windows.Forms.DataGridViewTextBoxColumn wnuServerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn wnuLastSync;
        private System.Windows.Forms.DataGridViewTextBoxColumn wnuLastRollup;
        private System.Windows.Forms.TabPage tabEndpointFaults;
        private System.Windows.Forms.TabPage tabUnapprovedUpdates;
        private System.Windows.Forms.TabControl tabAdminType;
        private System.Windows.Forms.TabPage tabHome;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuOptions;
        private System.Windows.Forms.Label lblWSUSConnection;
        private System.Windows.Forms.Label lblSQLConnection;
        private System.Windows.Forms.Label lblWSUSStatus;
        private System.Windows.Forms.Label lblSQLStatus;
        private System.Windows.Forms.ToolStripMenuItem mnuComputerGroupRules;
        private System.Windows.Forms.ToolStripMenuItem mnuWSUSServer;
        private System.Windows.Forms.ToolStripMenuItem mnuDefaultSusIDList;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem mnuPreferences;
        private System.Windows.Forms.ToolStripMenuItem mnuCredentials;
        private System.Windows.Forms.ContextMenuStrip cmEndpoint;
        private System.Windows.Forms.ToolStripMenuItem epGPUpdate;
        private System.Windows.Forms.ToolStripMenuItem epGPUpdateForce;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem epResetSusID;
        private System.Windows.Forms.ToolStripMenuItem epDetectNow;
        private System.Windows.Forms.ToolStripMenuItem epReportNow;
        private System.Windows.Forms.ToolStripMenuItem mnuResetAuth;
        private System.Windows.Forms.ToolStripMenuItem epDetails;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem mnuGroupApprovalRules;
        private System.Windows.Forms.Timer timTasks;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.SplitContainer splEndpoint;
        private System.Windows.Forms.DataGridView grdEndpoints;
        private System.Windows.Forms.DataGridViewTextBoxColumn epName;
        private System.Windows.Forms.DataGridViewTextBoxColumn epUpdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn epIP;
        private System.Windows.Forms.DataGridViewTextBoxColumn epApprovedUpdates;
        private System.Windows.Forms.DataGridViewTextBoxColumn epUpdateErrors;
        private System.Windows.Forms.DataGridViewTextBoxColumn epComputerGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn epFault;
        private System.Windows.Forms.DataGridViewTextBoxColumn epLastServerContact;
        private System.Windows.Forms.DataGridViewTextBoxColumn epLastStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn epPing;
        private System.Windows.Forms.DataGridViewTextBoxColumn epPingUpdated;
        private System.Windows.Forms.ToolStrip tlsEndpoint;
        private System.Windows.Forms.ToolStripButton butApproved;
        private System.Windows.Forms.ToolStripButton butUpdateErrors;
        private System.Windows.Forms.ToolStripButton butNotCommunicating;
        private System.Windows.Forms.ToolStripButton butUnassigned;
        private System.Windows.Forms.ToolStripButton butDefaultSusID;
        private System.Windows.Forms.ToolStripButton butGroupRules;
        private System.Windows.Forms.DataGridView grdTasks;
        private System.Windows.Forms.DataGridView grdUpdates;
        private System.Windows.Forms.DataGridViewTextBoxColumn UpdateName;
        private System.Windows.Forms.DataGridViewTextBoxColumn uaUpdateID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Updated;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn SortOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn KB;
        private System.Windows.Forms.DataGridViewTextBoxColumn T;
        private System.Windows.Forms.DataGridViewTextBoxColumn A;
        private System.Windows.Forms.DataGridViewTextBoxColumn B;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServerT;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChemistT;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChemistA;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChemistB;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChemistServerT;
        private System.Windows.Forms.DataGridViewTextBoxColumn Testing;
        private System.Windows.Forms.ToolStrip tlsFilterUpdates;
        private System.Windows.Forms.ToolStripDropDownButton tlmFilterUpdates;
        private System.Windows.Forms.ToolStripMenuItem tlsNoFilter;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tliXP;
        private System.Windows.Forms.ToolStripMenuItem tliVista;
        private System.Windows.Forms.ToolStripMenuItem tli7;
        private System.Windows.Forms.ToolStripMenuItem tli8;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem tli2003;
        private System.Windows.Forms.ToolStripMenuItem tli2008;
        private System.Windows.Forms.ToolStripMenuItem tli2008r2;
        private System.Windows.Forms.ToolStripMenuItem tli2012;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem tliOffice2003;
        private System.Windows.Forms.ToolStripMenuItem tliOffice2007;
        private System.Windows.Forms.ToolStripMenuItem tliOffice2010;
        private System.Windows.Forms.ToolStripMenuItem tliOffice2013;
        private System.Windows.Forms.ToolStripButton tlsUpdateCount;
        private System.Windows.Forms.ToolStripButton butApproveUpdates;
        private System.Windows.Forms.ToolStripDropDownButton tlmSelections;
        private System.Windows.Forms.ToolStripMenuItem clearSelectionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem groupAToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AselectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AdeselectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem groupBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem BselectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem BdeselectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem groupSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SselectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SdeselectToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem chemistGroupAToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CAselectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CAdeselectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chemistGroupBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CBselectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CBdeselectToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton butDeclineUnapproved;
        private System.Windows.Forms.ToolStripButton butCancelApprove;
        private System.Windows.Forms.ToolStripMenuItem mnuUtilities;
        private System.Windows.Forms.ToolStripMenuItem mnuSUSWatcher;
        private System.Windows.Forms.DataGridViewTextBoxColumn tskID;
        private System.Windows.Forms.DataGridViewTextBoxColumn tskStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn tskIP;
        private System.Windows.Forms.DataGridViewTextBoxColumn tskCommand;
        private System.Windows.Forms.DataGridViewTextBoxColumn tskOutput;
    }
}

