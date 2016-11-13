using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Utils;
using System.IO;
using System.Drawing;
using DevExpress.XtraTab;

namespace UILogic
{
    public static class XtraExtension
    {
        #region GridViewExtention
        public static void InvokeGridViewFocusedRowChanged(this GridView gridView)
        {
            if (gridView.RowCount > 0)
            {
                var index = gridView.FocusedRowHandle;
                gridView.FocusedRowHandle = -1;
                gridView.FocusedRowHandle = index;
            }
        }
        #endregion

        #region SuperToolTip
        public static SuperToolTip CreateToolTips(string name, string info)
        {
            return CreateToolTips(name, info, null);
        }

        public static SuperToolTip CreateToolTips(string name, string info, string picPath)
        {
            var superToolTip1 = new SuperToolTip { AllowHtmlText = DefaultBoolean.True };
            var toolTipTitleItem1 = new ToolTipTitleItem();
            var toolTipItem1 = new ToolTipItem();
            toolTipTitleItem1.Text = name;
            if (!string.IsNullOrEmpty(picPath))
            {
                if (File.Exists(Environment.CurrentDirectory + picPath))
                    toolTipItem1.Appearance.Image = Image.FromFile(Environment.CurrentDirectory + picPath);
                toolTipItem1.Appearance.Options.UseImage = true;
                if (File.Exists(Environment.CurrentDirectory + picPath))
                    toolTipItem1.Image = Image.FromFile(Environment.CurrentDirectory + picPath);
            }
            toolTipItem1.LeftIndent = 6;
            toolTipItem1.Text = info;
            superToolTip1.Items.Add(toolTipTitleItem1);
            superToolTip1.Items.Add(toolTipItem1);
            return superToolTip1;
        }
        #endregion

        #region MessageBox
        public static DialogResult ShowMessage(string msg)
        {
            return XtraMessageBox.Show(msg, "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static DialogResult Confirm(string msg)
        {
            return XtraMessageBox.Show(msg, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
        }

        public static DialogResult ShowAlarm(string msg)
        {
            return XtraMessageBox.Show(msg, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        #endregion

        #region PanelControlExtension

        /// <summary>
        /// 动态加载control扩展方法
        /// </summary>
        /// <param name="panelControl"></param>
        /// <param name="control"></param>
        public static void LoadControl(this PanelControl panelControl, XtraUserControl control)
        {

            panelControl.Controls.Clear();
            control.Dock = DockStyle.Fill;
            panelControl.Controls.Add(control);
        }

        /// <summary>
        /// 动态加载control扩展方法
        /// </summary>
        /// <param name="panelControl"></param>
        /// <param name="control"></param>
        public static void LoadControl(this PanelControl panelControl, UserControl control)
        {
            panelControl.Controls.Clear();
            control.Dock = DockStyle.Fill;
            panelControl.Controls.Add(control);
        }

        /// <summary>
        /// 动态加载control扩展方法
        /// </summary>
        /// <param name="panelControl"></param>
        /// <param name="control"></param>
        public static void LoadControl(this XtraTabPage panelControl, XtraUserControl control)
        {
            panelControl.Controls.Clear();
            control.Dock = DockStyle.Fill;
            panelControl.Controls.Add(control);
        }

        #endregion
    }
}
