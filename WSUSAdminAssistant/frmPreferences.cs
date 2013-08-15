using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WSUSAdminAssistant
{
    public partial class frmPreferences : Form
    {
        private clsConfig cfg = new clsConfig();

        public frmPreferences()
        {
            InitializeComponent();

            // Populate form
            txtPSExec.Text = cfg.PSExecPath;
        }

        private void btnPSExec_Click(object sender, EventArgs e)
        {
            dlgOpen.Filter = "PSExec|psexec.exe";
            dlgOpen.FileName = "psexec.exe";
            dlgOpen.DefaultExt = "exe";
            DialogResult res = dlgOpen.ShowDialog();

            if (res == DialogResult.OK)
                txtPSExec.Text = dlgOpen.FileName;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Update configuration
            cfg.PSExecPath = txtPSExec.Text;

            // Close form
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // Close form
            this.Close();
        }
    }
}
