using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.UpdateServices.Administration;

namespace WSUSAdminAssistant
{
    public partial class frmWSUSConfig : Form
    {
        private clsConfig cfg = new clsConfig();

        public frmWSUSConfig()
        {
            InitializeComponent();
        }

        private void frmWSUSConfig_Load(object sender, EventArgs e)
        {
            // Populate form with configuration data
            txtSQL.Text = cfg.DBServer;
            txtDB.Text = cfg.DBDatabase;
            txtUser.Text = cfg.DBUsername;
            txtPassword.Text = cfg.DBPassword;
            txtConfirm.Text = txtPassword.Text;
            txtWSUS.Text = cfg.WSUSServer;
            chkSecure.Checked = cfg.WSUSSecureConnection;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            PasswordsMatch();
        }

        private void txtConfirm_TextChanged(object sender, EventArgs e)
        {
            PasswordsMatch();
        }

        private void PasswordsMatch()
        {
            btnSave.Enabled = (txtPassword.Text == txtConfirm.Text);

            if (btnSave.Enabled)
            {
                txtPassword.BackColor = SystemColors.Window;
                txtConfirm.BackColor = SystemColors.Window;
            }
            else
            {
                txtPassword.BackColor = Color.Red;
                txtConfirm.BackColor = Color.Red;
            }
        }
            
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Try to connect to SQL database
            string sqlconnect;

            if (chkSecure.Checked)
                sqlconnect = "database=" + txtDB.Text + ";" + "server=" + txtSQL.Text + ";Trusted_Connection=true";
            else
                sqlconnect = "database=" + txtDB.Text + ";" + "server=" + txtSQL.Text + ";" + "User ID=" + txtUser.Text + ";Password=" + txtPassword.Text;

            SqlConnection sql = new SqlConnection(sqlconnect);

            try
            {
                sql.Open();
                sql.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Could not connect to SQL database", MessageBoxButtons.OK);
                return;
            }

            // SQL seems good, check WSUS
            try
            {
                IUpdateServer wsus = AdminProxy.GetUpdateServer(txtWSUS.Text, chkSecure.Checked);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Could not connect to WSUS server", MessageBoxButtons.OK);
                return;
            }

            // Both are good, write configuration and restart program.
            cfg.DBServer = txtWSUS.Text;
            cfg.DBDatabase = txtDB.Text;
            cfg.DBUsername = txtUser.Text;
            cfg.DBPassword = txtPassword.Text;
            cfg.DBIntegratedAuth = chkSecure.Checked;
            cfg.WSUSServer = txtWSUS.Text;
            cfg.WSUSSecureConnection = chkSecure.Checked;

            Application.Restart();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
