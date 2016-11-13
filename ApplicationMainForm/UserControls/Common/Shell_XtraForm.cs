using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using UILogic;

namespace ApplicationMainForm.UserControls.Common
{
    public partial class Shell_XtraForm : DevExpress.XtraEditors.XtraForm
    {
        public int? ManualWidth
        {
            get;
            set;
        }

        public int? ManualHeight
        {
            get;
            set;
        }

        public Shell_XtraForm()
        {
            InitializeComponent();
        }

        public void ResizeForm()
        {
            if (ManualWidth != null && ManualHeight != null && ManualWidth > 0 && ManualHeight > 0)
                this.Size = new Size((int)ManualWidth, (int)ManualHeight);
        }

        public void LoadControl(XtraUserControl xtraUserControl)
        {
            this.panelControl1.LoadControl(xtraUserControl);
        }

        private void Shell_XtraForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (XtraExtension.Confirm(@"确认是否退出!") != DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

    }
}