﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WSUSAdminAssistant
{
    public partial class frmPreferences : Form
    {
        private clsConfig cfg;

        public frmPreferences(clsConfig cfgobject)
        {
            InitializeComponent();

            cfg = cfgobject;

            // Populate form
            txtPSExec.Text = cfg.PSExecPath;
            txtComputerRegEx.Text = cfg.ComputerRegExXMLFile;
            txtCredentials.Text = cfg.CredentialXmlFile;
            txtDefaultSUS.Text = cfg.SusIdXmlFile;
            txtGroupUpdate.Text = cfg.GroupUpdateRulesXMLFile;

            chkLocalCreds.Checked = cfg.RunWithLocalCreds;
        }

        private void btnPSExec_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "PSExec|psexec.exe";
            of.FileName = "psexec.exe";
            of.DefaultExt = "exe";
            DialogResult res = of.ShowDialog();

            if (res == DialogResult.OK)
                txtPSExec.Text = of.FileName;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Update configuration
            cfg.PSExecPath = txtPSExec.Text;
            cfg.RunWithLocalCreds = chkLocalCreds.Checked;

            // Close form
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // Close form
            this.Close();
        }

        private void lnkPSExec_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Go to the PSExec page at Microsoft
            System.Diagnostics.Process.Start("http://technet.microsoft.com/en-au/sysinternals/bb897553.aspx");
        }

        private void btnComputerRegEx_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.InitialDirectory = Path.GetDirectoryName(cfg.ComputerRegExXMLFile);
            of.CheckFileExists = true;
            of.CheckPathExists = true;
            of.Filter = string.Format("XML Files|{0}", Path.GetFileName(cfg.ComputerRegExXMLFile));
            of.Title = "Select a location for the Computer Group Rules file";
            of.Multiselect = false;

            DialogResult result = of.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // Save location
                txtComputerRegEx.Text = of.FileName;
                cfg.ComputerRegExXMLFile = txtComputerRegEx.Text;
            }
        }

        private void btnCredentials_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.InitialDirectory = Path.GetDirectoryName(cfg.CredentialXmlFile);
            of.CheckFileExists = true;
            of.CheckPathExists = true;
            of.Filter = string.Format("XML Files|{0}", Path.GetFileName(cfg.CredentialXmlFile));
            of.Title = "Select a location for the Computer Group Rules file";
            of.Multiselect = false;

            DialogResult result = of.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // Save location
                txtCredentials.Text = of.FileName;
                cfg.CredentialXmlFile = txtCredentials.Text;
            }
        }

        private void btnDefaultSUS_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.InitialDirectory = Path.GetDirectoryName(cfg.SusIdXmlFile);
            of.CheckFileExists = true;
            of.CheckPathExists = true;
            of.Filter = string.Format("XML Files|{0}", Path.GetFileName(cfg.SusIdXmlFile));
            of.Title = "Select a location for the Computer Group Rules file";
            of.Multiselect = false;

            DialogResult result = of.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // Save location
                txtDefaultSUS.Text = of.FileName;
                cfg.SusIdXmlFile = txtDefaultSUS.Text;
            }
        }

        private void btnGroupUpdate_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.InitialDirectory = Path.GetDirectoryName(cfg.GroupUpdateRulesXMLFile);
            of.CheckFileExists = true;
            of.CheckPathExists = true;
            of.Filter = string.Format("XML Files|{0}", Path.GetFileName(cfg.GroupUpdateRulesXMLFile));
            of.Title = "Select a location for the Computer Group Rules file";
            of.Multiselect = false;

            DialogResult result = of.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // Save location
                txtGroupUpdate.Text = of.FileName;
                cfg.GroupUpdateRulesXMLFile = txtGroupUpdate.Text;
            }
        }
    }
}
