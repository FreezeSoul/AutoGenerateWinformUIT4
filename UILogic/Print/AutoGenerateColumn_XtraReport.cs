using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Linq;
using System.Collections.Generic;

namespace UILogic.Print
{
    public partial class AutoGenerateColumn_XtraReport : DevExpress.XtraReports.UI.XtraReport
    {

        public AutoGenerateColumn_XtraReport()
        {
            InitializeComponent();
        }

        public Dictionary<string, float> CustomSizeList = new Dictionary<string, float>();

        public AutoGenerateColumn_XtraReport Init(DataSet ds)
        {
            //this.PaperKind = System.Drawing.Printing.PaperKind.A4Rotated;
            //this.PrintingSystem.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4Rotated;
            this.DataSource = ds;
            GenerateColumnHeader(ds);
            GenerateDataBindColumn(ds);
            this.PrintingSystem.PageSettings.Landscape = true;
            return this;
        }

        public void GenerateColumnHeader(DataSet ds)
        {
            xrTable1.BeginInit();
            this.xrTableRow1.Cells.Clear();
            DataTable dt = ds.Tables[0];
            foreach (DataColumn dc in dt.Columns)
            {
                XRTableCell xrTableCellTemp = new XRTableCell();
                xrTableCellTemp.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
                xrTableCellTemp.Name = "xrTableCellTemp" + dc.ColumnName;
                xrTableCellTemp.StylePriority.UseFont = false;
                xrTableCellTemp.Text = dc.ColumnName;
                xrTableCellTemp.Weight = 1;
                xrTableCellTemp.WordWrap = false;
                this.xrTableRow1.Cells.Add(xrTableCellTemp);
            }
            this.xrTableRow1.Weight = 1;
             
            if (this.CustomSizeList.Count > 0)
            {
                float leftWidth = this.xrTableRow1.SizeF.Width;
                foreach (KeyValuePair<string, float> keyValue in this.CustomSizeList)
                {
                    leftWidth = leftWidth - keyValue.Value;
                }
                float averWidth = leftWidth / (this.xrTableRow1.Cells.Count - this.CustomSizeList.Count);
                foreach (DataColumn dc in dt.Columns)
                {
                    if (CustomSizeList.Keys.Contains(dc.ColumnName))
                    {
                        (xrTableRow1.Cells["xrTableCellTemp" + dc.ColumnName] as XRTableCell).SizeF = new SizeF(CustomSizeList[dc.ColumnName], xrTableRow1.SizeF.Height);
                        (xrTableRow1.Cells["xrTableCellTemp" + dc.ColumnName] as XRTableCell).WidthF = CustomSizeList[dc.ColumnName];
                    }
                    else
                    {
                        (xrTableRow1.Cells["xrTableCellTemp" + dc.ColumnName] as XRTableCell).SizeF = new SizeF(averWidth, xrTableRow1.SizeF.Height);
                        (xrTableRow1.Cells["xrTableCellTemp" + dc.ColumnName] as XRTableCell).WidthF = averWidth;

                    }
                }
            }
            xrTable1.EndInit();
        }

        public void GenerateDataBindColumn(DataSet ds)
        {
            DataTable dt = ds.Tables[0];
            foreach (DataColumn dc in dt.Columns)
            {
                XRLabel xrLabelTemp = new XRLabel();
                xrLabelTemp.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(177)))), ((int)(((byte)(183)))));
                xrLabelTemp.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                            | DevExpress.XtraPrinting.BorderSide.Right)
                            | DevExpress.XtraPrinting.BorderSide.Bottom)));
                xrLabelTemp.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, dt.TableName + "." + dc.ColumnName)});
                xrLabelTemp.Font = new System.Drawing.Font("Î¢ÈíÑÅºÚ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                float x = (xrTableRow1.Cells["xrTableCellTemp" + dc.ColumnName] as XRTableCell).LocationFloat.X;
                xrLabelTemp.LocationFloat = new DevExpress.Utils.PointFloat(x, 0F);
                xrLabelTemp.Name = "xrLabelTemp" + dc.ColumnName;
                xrLabelTemp.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
                float width = (xrTableRow1.Cells["xrTableCellTemp" + dc.ColumnName] as XRTableCell).SizeF.Width;
                xrLabelTemp.SizeF = new System.Drawing.SizeF(width, 25F);
                xrLabelTemp.StylePriority.UseBorderColor = false;
                xrLabelTemp.StylePriority.UseBorders = false;
                xrLabelTemp.StylePriority.UseFont = false;
                xrLabelTemp.WordWrap = false;
                xrLabelTemp.StylePriority.UseTextAlignment = false;
                xrLabelTemp.Text = "xrLabelTemp";
                xrLabelTemp.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                this.Detail.Controls.Add(xrLabelTemp);
            }
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        }

    }
}
