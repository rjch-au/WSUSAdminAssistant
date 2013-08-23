using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.UpdateServices.Administration;

using WSUSAdminAssistant;
using WSUSAdminAssistant.clsConfig;
using WSUSAdminAssistant.clsWSUS;

namespace SUSWatcher
{
    public partial class frmMain : Form
    {
        private clsConfig cfg = new clsConfig();
        private clsWSUS wsus = new clsWSUS();

        private ComputerTargetCollection ctc = new ComputerTargetCollection();

        private class ComputerDetail
        {
            public IComputerTarget Computer;

            public Guid SusId
            {
                get { return Guid.Parse(this.Computer.Id); }
            }
        }

        private class ComputerCollection : System.Collections.CollectionBase
        {
            public ComputerDetail this[int index]
            {
                get { return (ComputerDetail)this.List[index]; }
                set { this.List[index] = value; }
            }

            public int Add(ComputerDetail add)
            {
                return this.List.Add(add);
            }

            public void Remove(int index)
            {
                // Check index is in the correct range
                if (index < 0 || index >= this.List.Count)
                    // Out of bounds - throw an exception
                    throw new IndexOutOfRangeException();

                this.List.RemoveAt(index);
            }
        }

        public void frmMain()
        {
            InitializeComponent();
        }

        private void tim_Tick(object sender, EventArgs e)
        {
            // If it's not time, decrement the progress bar and exit
            if (prg.Value > prg.Minimum)
            {
                prg.Value--;
                return;
            }

            prg.Value = prg.Maximum;
        }
    }
}
