using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.XtraGrid;
using System.Data;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;

namespace UILogic.Print
{
    public class PrinterHelper
    {
        public static void Print(string title, string timeStr, GridControl gridControl1)
        {
            PrintComponent_XtraForm printView = PrintComponent_XtraForm.CreateReport();
            var ds = new DataSet();
            var dt = gridControl1.DataSource as DataTable;
            var gridView1 = gridControl1.MainView as GridView;
            if (dt == null)
                return;
            var dtTemp = dt.Copy();
            foreach (DevExpress.XtraGrid.Columns.GridColumn column in gridView1.Columns)
            {
                if (dtTemp.Columns.Contains(column.FieldName))
                    dtTemp.Columns[column.FieldName].ColumnName = column.Caption;
            }
            if (dtTemp.DataSet == null)
                ds.Tables.Add(dtTemp);
            else
                ds = dtTemp.DataSet;
            var report = new AutoGenerateColumn_XtraReport();
            report.CustomSizeList = new Dictionary<string, float>();
            report.Init(ds);
            printView.Init(report, ds, title, "编制单位：", "报表日期：" + timeStr,
                           "单位名称：", "打印人：", "",
                           String.Format("打印时间: {0:g}", DateTime.Now), true);
            report.PrintingSystem.PageSettings.Landscape = true;
            printView.Show();
        }

        public static void Print(string title, GridControl gridControl1)
        {
            var ps = new PrintingSystem();
            var link = new PrintableComponentLink(ps);
            link.PaperKind = System.Drawing.Printing.PaperKind.A4Rotated;
            ps.Links.Add(link);
            link.Component = gridControl1;
            var phf = (PageHeaderFooter)link.PageHeaderFooter;
            phf.Header.Content.Clear();
            phf.Header.Content.AddRange(new[] { "", title, "" });
            phf.Header.Font = new Font("微软雅黑", 22, FontStyle.Bold);
            phf.Header.LineAlignment = BrickAlignment.Center;
            phf.Footer.Content.Clear();
            phf.Footer.Content.AddRange(new[] { "", "", String.Format("打印时间: {0:g}", DateTime.Now) });
            phf.Footer.Font = new Font("微软雅黑", 16, FontStyle.Regular);
            link.ShowPreview(UserLookAndFeel.Default);
        }

        public static void ExportXls(GridControl gridControl1, string title)
        {
            var ps = new PrintingSystem();
            var link = new PrintableComponentLink(ps);
            link.PaperKind = System.Drawing.Printing.PaperKind.A4Rotated;
            ps.Links.Add(link);
            link.Component = gridControl1;
            var phf = (PageHeaderFooter)link.PageHeaderFooter;
            phf.Header.Content.Clear();
            phf.Header.Content.AddRange(new[] { "", title, "" });
            phf.Header.Font = new Font("微软雅黑", 22, FontStyle.Bold);
            phf.Header.LineAlignment = BrickAlignment.Center;
            phf.Footer.Content.Clear();
            phf.Footer.Content.AddRange(new[] { "", "", String.Format("打印时间: {0:g}", DateTime.Now) });
            phf.Footer.Font = new Font("微软雅黑", 16, FontStyle.Regular);
            link.ShowRibbonPreview(UserLookAndFeel.Default);
            var dialog = new SaveFileDialog { Filter = @"Excel文件|*.xls" };
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                ps.ExportToXls(dialog.FileName);
            }
        }

    }
}
