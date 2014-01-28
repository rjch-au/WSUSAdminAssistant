using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.UpdateServices.Administration;
using Microsoft.UpdateServices.Administration.Internal;

namespace WSUSAdminAssistant
{
    public partial class frmIgnoreComputerGroups : Form
    {
        private clsConfig cfg;
        private clsWSUS wsus;

        public frmIgnoreComputerGroups(clsConfig cfgobject)
        {
            cfg = cfgobject;
            wsus = cfg.wsus;

            InitializeComponent();
        }

        private List<IComputerTargetGroup> groups = new List<IComputerTargetGroup>();

        private void frmIgnoreComputerGroups_Load(object sender, EventArgs e)
        {
            // Create a clean list of computer groups and group IDs
            groups.Clear();

            foreach (IComputerTargetGroup g in wsus.ComputerGroups)
                groups.Add(g);
            
            // Sort it
            groups.OrderBy(x => x.Name);

            // Add each group name to the listbox
            foreach (IComputerTargetGroup g in groups)
            {
                int i = lstGroupsToIgnore.Items.Add(g.Name);

                // Is this group in the list of groups to exclude?
                if (cfg.IgnoreComputerGroupCollection.Any(g.Id.ToString().Contains))
                    // Yes - check the item in the listbox
                    lstGroupsToIgnore.SetItemChecked(i, true);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Create a list of computer group IDs to ignore
            List<string> ignore = new List<string>();

            // Loop through all selected items and add the corresponding group ID to the list
            foreach (string g in lstGroupsToIgnore.CheckedItems)
            {
                // Loop through all computer groups, looking for the matching name
                foreach (IComputerTargetGroup cg in wsus.ComputerGroups)
                {
                    // Does the name of this group match that of the selected item?
                    if (cg.Name == g)
                    {
                        // Yes, add it to the list and break
                        ignore.Add(cg.Id.ToString());
                        break;
                    }
                }
            }

            // Save the modified list
            cfg.IgnoreComputerGroupCollection = ignore.ToArray();

            // Close the form
            this.Close();
        }
    }
}
