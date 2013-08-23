namespace SUSWatcher
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
            this.prg = new System.Windows.Forms.ProgressBar();
            this.lst = new System.Windows.Forms.ListBox();
            this.tim = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // prg
            // 
            this.prg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.prg.Location = new System.Drawing.Point(0, 329);
            this.prg.Name = "prg";
            this.prg.Size = new System.Drawing.Size(486, 23);
            this.prg.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.prg.TabIndex = 0;
            // 
            // lst
            // 
            this.lst.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lst.FormattingEnabled = true;
            this.lst.Location = new System.Drawing.Point(0, 0);
            this.lst.Name = "lst";
            this.lst.Size = new System.Drawing.Size(486, 329);
            this.lst.TabIndex = 1;
            // 
            // tim
            // 
            this.tim.Interval = 1000;
            this.tim.Tick += new System.EventHandler(this.tim_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 352);
            this.Controls.Add(this.lst);
            this.Controls.Add(this.prg);
            this.Name = "frmMain";
            this.Text = "SUSWatch";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar prg;
        private System.Windows.Forms.ListBox lst;
        private System.Windows.Forms.Timer tim;
    }
}

