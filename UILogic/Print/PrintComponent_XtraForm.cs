using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Parameters;
using System.Collections;
using DevExpress.XtraPrinting;

namespace UILogic.Print
{
    public partial class PrintComponent_XtraForm : DevExpress.XtraEditors.XtraForm
    {
        //public string Title;
        //public string LeftHeader;
        //public string MiddleHeader;
        //public string RightHeader;
        //public string LeftFooter;
        //public string MiddleFooter;
        //public string RightFooter;

        public static PrintComponent_XtraForm CreateReport()
        {
            var printView = new PrintComponent_XtraForm();
            return printView;
        }

        public PrintComponent_XtraForm()
        {
            InitializeComponent();
        }

        public void Init(XtraReport report, DataSet ds, string title, string leftHeader, string middleHeader, string rightHeader, string leftFooter, string middleFooter, string rightFooter,bool direction)
        {
            report.PrinterName = title;
            report.DataSource = ds;
            report.Parameters["Title"].Value = title;
            report.Parameters["LeftHeader"].Value = leftHeader;
            report.Parameters["MiddleHeader"].Value = middleHeader;
            report.Parameters["RightHeader"].Value = rightHeader;
            report.Parameters["LeftFooter"].Value = leftFooter;
            report.Parameters["MiddleFooter"].Value = middleFooter;
            report.Parameters["RightFooter"].Value = rightFooter;

            report.PreviewRowCount = 500;
            report.RequestParameters = false;
            printControl1.PrintingSystem = report.PrintingSystem;
            printControl1.PrintingSystem.PageSettings.Landscape = direction;
            report.ExportOptions.PrintPreview.DefaultFileName = title;
            report.CreateDocument();
            printControl1.PrintingSystem.ExportDefault = PrintingSystemCommand.ExportXls;
            printControl1.PrintingSystem.SendDefault = PrintingSystemCommand.SendXls;
        }

        public void Init(XtraReport report, IList list, string title, string leftHeader, string middleHeader, string rightHeader, string leftFooter, string middleFooter, string rightFooter, bool direction)
        {
            report.DataSource = list;
            report.Parameters["Title"].Value = title;
            report.Parameters["LeftHeader"].Value = leftHeader;
            report.Parameters["MiddleHeader"].Value = middleHeader;
            report.Parameters["RightHeader"].Value = rightHeader;
            report.Parameters["LeftFooter"].Value = leftFooter;
            report.Parameters["MiddleFooter"].Value = middleFooter;
            report.Parameters["RightFooter"].Value = rightFooter;

            report.PreviewRowCount = 500;
            report.RequestParameters = false;
            printControl1.PrintingSystem = report.PrintingSystem;
            printControl1.PrintingSystem.PageSettings.Landscape = direction;
            report.ExportOptions.PrintPreview.DefaultFileName = title;
            report.CreateDocument();
            printControl1.PrintingSystem.ExportDefault = PrintingSystemCommand.ExportXls;
            printControl1.PrintingSystem.SendDefault = PrintingSystemCommand.SendXls;
        }
    }
}