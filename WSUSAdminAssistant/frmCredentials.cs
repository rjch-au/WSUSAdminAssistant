using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace WSUSAdminAssistant
{
    public partial class frmCredentials : Form
    {
        public clsConfig cfg = new clsConfig();

        public frmCredentials()
        {
            InitializeComponent();
        }

        private void grdCredentials_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // If we're displaying the password column, reset the display to asterisks
            if (e.ColumnIndex == crPassword.Index && e.Value != null)
            {
                string v = e.Value.ToString();
                e.Value = v.Substring(0, 1) + new string('*', v.Length - 2) + v.Substring(v.Length - 1, 1);
            }
        }

        private void grdCredentials_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridViewRow r = grdCredentials.Rows[e.RowIndex];

            // Is this a new row?
            if (!r.IsNewRow)
                // No - check it
                ValidateRow(r);
        }

        private bool ValidateRow(DataGridViewRow r)
        {
            // Assume everything validates unless one of the following checks fails
            bool ok = true;
            foreach (DataGridViewCell c in r.Cells) c.ErrorText = "";

            // Check Network address is a valid IP address
            IPAddress ip;

            if (r.Cells["crNetwork"].Value == null || !IPAddress.TryParse(r.Cells["crNetwork"].Value.ToString(), out ip))
            {
                r.Cells["crNetwork"].ErrorText = "Invalid network address";
                ok = false;
            }
            else
            {
                // Check the netmask column is a valid number of bits
                byte netmask;
                DataGridViewCell c = r.Cells["crNetmask"];

                if (c.Value == null || !byte.TryParse(c.Value.ToString(), out netmask))
                {
                    c.ErrorText = "Invalid network mask (must be the number of bits - e.g. 24 not 255.255.255.0)";
                    ok = false;
                }
                else
                {
                    c.ErrorText = "";

                    // Check that the number of bits is correct for the type of IP address
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && netmask > 32) { c.ErrorText = "IPv4 netmasks must be 32 bits or less"; ok = false; }
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6 && netmask > 128) { c.ErrorText = "IPv6 netmasks must be 128 bits or less"; ok = false; }
                }
            }

            // Check that there is a username and password supplied
            if (r.Cells["crUser"].Value == null || r.Cells["crUser"].Value.ToString() == "") { r.Cells["crUser"].ErrorText = "Username must be supplied"; ok = false; }
            if (r.Cells["crPassword"].Value == null || r.Cells["crPassword"].Value.ToString() == "") { r.Cells["crPassword"].ErrorText = "Password must be supplied - empty network passwords not supported"; ok = false; }

            return ok;
        }

        private clsConfig.CredentialCollection CollateForm()
        {
            clsConfig.CredentialCollection cc = new clsConfig.CredentialCollection();

            // Loop through each row and (if it's valid) add it to the collection
            foreach (DataGridViewRow r in grdCredentials.Rows)
            {
                if (!r.IsNewRow && ValidateRow(r))
                {
                    clsConfig.SecurityCredential c = new clsConfig.SecurityCredential();

                    c.ip = IPAddress.Parse(r.Cells[crNetwork.Index].Value.ToString()).ToString();
                    c.netmask = byte.Parse(r.Cells[crNetmask.Index].Value.ToString());
                    c.description = r.Cells[crDescription.Index].Value.ToString();

                    if (r.Cells[crDomain.Index].Value == null)
                        c.domain = "";
                    else
                        c.domain = r.Cells[crDomain.Index].Value.ToString();

                    c.username = r.Cells[crUser.Index].Value.ToString();
                    c.password = r.Cells[crPassword.Index].Value.ToString();

                    cc.Add(c);
                }
            }

            return cc;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validate each row
            bool ok = true;

            foreach (DataGridViewRow r in grdCredentials.Rows)
                if (!r.IsNewRow && !ValidateRow(r)) { ok = false; break; }

            // Save without prompting if all rows are OK.  Prompt if not all rows are OK.
            if (ok || (!ok && MessageBox.Show("Not all rows are valid - rows with errors (highlighted) will not be saved if you continue.  Save security credentials?", "Not all rows are valid", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes))
            {
                clsConfig.CredentialCollection cc = CollateForm();

                cfg.CredentialList = cc;

                // Close form
                this.Close();
            }
        }

        private void frmCredentials_Load(object sender, EventArgs e)
        {
            // Populate grid
            clsConfig.CredentialCollection cc = cfg.CredentialList;

            foreach (clsConfig.SecurityCredential c in cc)
            {
                DataGridViewRow r = grdCredentials.Rows[grdCredentials.Rows.Add()];

                r.Cells["crNetwork"].Value = c.ip.ToString();
                r.Cells["crNetmask"].Value = c.netmask;
                r.Cells["crDescription"].Value = c.description;
                r.Cells["crDomain"].Value = c.domain;
                r.Cells["crUser"].Value = c.username;
                r.Cells["crPassword"].Value = c.password;
            }
        }
    }
}
