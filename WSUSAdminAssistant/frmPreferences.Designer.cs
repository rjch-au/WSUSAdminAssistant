namespace WSUSAdminAssistant
{
    partial class frmPreferences
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPreferences));
            this.tabPreferences = new System.Windows.Forms.TabControl();
            this.tabHelpers = new System.Windows.Forms.TabPage();
            this.tlsPreferences = new System.Windows.Forms.ToolStrip();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.lblPSExec = new System.Windows.Forms.Label();
            this.txtPSExec = new System.Windows.Forms.TextBox();
            this.btnPSExec = new System.Windows.Forms.Button();
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.btnClose = new System.Windows.Forms.ToolStripButton();
            this.tabPreferences.SuspendLayout();
            this.tabHelpers.SuspendLayout();
            this.tlsPreferences.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPreferences
            // 
            this.tabPreferences.Controls.Add(this.tabHelpers);
            this.tabPreferences.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPreferences.Location = new System.Drawing.Point(0, 25);
            this.tabPreferences.Multiline = true;
            this.tabPreferences.Name = "tabPreferences";
            this.tabPreferences.SelectedIndex = 0;
            this.tabPreferences.Size = new System.Drawing.Size(631, 433);
            this.tabPreferences.TabIndex = 0;
            // 
            // tabHelpers
            // 
            this.tabHelpers.Controls.Add(this.btnPSExec);
            this.tabHelpers.Controls.Add(this.txtPSExec);
            this.tabHelpers.Controls.Add(this.lblPSExec);
            this.tabHelpers.Location = new System.Drawing.Point(4, 22);
            this.tabHelpers.Name = "tabHelpers";
            this.tabHelpers.Padding = new System.Windows.Forms.Padding(3);
            this.tabHelpers.Size = new System.Drawing.Size(623, 407);
            this.tabHelpers.TabIndex = 0;
            this.tabHelpers.Text = "Helper Applications";
            this.tabHelpers.UseVisualStyleBackColor = true;
            // 
            // tlsPreferences
            // 
            this.tlsPreferences.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSave,
            this.btnClose});
            this.tlsPreferences.Location = new System.Drawing.Point(0, 0);
            this.tlsPreferences.Name = "tlsPreferences";
            this.tlsPreferences.Size = new System.Drawing.Size(631, 25);
            this.tlsPreferences.TabIndex = 1;
            this.tlsPreferences.Text = "toolStrip1";
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(51, 22);
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblPSExec
            // 
            this.lblPSExec.AutoSize = true;
            this.lblPSExec.Location = new System.Drawing.Point(8, 10);
            this.lblPSExec.Name = "lblPSExec";
            this.lblPSExec.Size = new System.Drawing.Size(65, 13);
            this.lblPSExec.TabIndex = 0;
            this.lblPSExec.Text = "PSExec.exe";
            // 
            // txtPSExec
            // 
            this.txtPSExec.AcceptsReturn = true;
            this.txtPSExec.Location = new System.Drawing.Point(121, 7);
            this.txtPSExec.Name = "txtPSExec";
            this.txtPSExec.Size = new System.Drawing.Size(240, 20);
            this.txtPSExec.TabIndex = 1;
            // 
            // btnPSExec
            // 
            this.btnPSExec.Location = new System.Drawing.Point(367, 5);
            this.btnPSExec.Name = "btnPSExec";
            this.btnPSExec.Size = new System.Drawing.Size(24, 23);
            this.btnPSExec.TabIndex = 2;
            this.btnPSExec.Text = "...";
            this.btnPSExec.UseVisualStyleBackColor = true;
            this.btnPSExec.Click += new System.EventHandler(this.btnPSExec_Click);
            // 
            // dlgOpen
            // 
            this.dlgOpen.FileName = "openFileDialog1";
            // 
            // btnClose
            // 
            this.btnClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(40, 22);
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmPreferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(631, 458);
            this.Controls.Add(this.tabPreferences);
            this.Controls.Add(this.tlsPreferences);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPreferences";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Preferences";
            this.tabPreferences.ResumeLayout(false);
            this.tabHelpers.ResumeLayout(false);
            this.tabHelpers.PerformLayout();
            this.tlsPreferences.ResumeLayout(false);
            this.tlsPreferences.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabPreferences;
        private System.Windows.Forms.TabPage tabHelpers;
        private System.Windows.Forms.ToolStrip tlsPreferences;
        private System.Windows.Forms.Label lblPSExec;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.Button btnPSExec;
        private System.Windows.Forms.TextBox txtPSExec;
        private System.Windows.Forms.OpenFileDialog dlgOpen;
        private System.Windows.Forms.ToolStripButton btnClose;
    }
}