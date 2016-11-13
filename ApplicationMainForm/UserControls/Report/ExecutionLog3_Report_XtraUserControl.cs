using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Utils;
using ApplicationMainForm.UserControls.Edit;
using ApplicationMainForm.UserControls.Common;
using BusinessLogicLayer;
using BusinessLogicLayer.Ado;
using DomainModelLayer;
using Infrastructure;
using UILogic;

namespace ApplicationMainForm.UserControls.Report
{
    public partial class ExecutionLog3_Report_XtraUserControl : DevExpress.XtraEditors.XtraUserControl
    {
        public ExecutionLog3_Report_XtraUserControl()
        {
            InitializeComponent();
        }
		
		#region Property

        private readonly  ExecutionLog3AdoBLL _executionlog3AdoBll = new ExecutionLog3AdoBLL();

        private readonly int _pageSize = ConfigManagerHelper.GetIntAppSetting("PageSize");

        private int _recordCount = -1;

        #endregion

        #region Method
        /// <summary>
        /// 自定义查询条件
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetSearchParams()
        {
            return new Dictionary<string, object>()
                       {

                       };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPageIndex"></param>
        private void BindData(int currentPageIndex)
        {
            var defaultParams = new Dictionary<string,object>();
			
			#region 查询条件赋值
						
				if(InstanceName_LayoutControlItem.Visibility == LayoutVisibility.Always && InstanceName_TextEdit.Text.Trim() != string.Empty )
				{
					defaultParams.Add("InstanceName", InstanceName_TextEdit.Text.Trim());
				}
				
						
				if(ItemPath_LayoutControlItem.Visibility == LayoutVisibility.Always && ItemPath_TextEdit.Text.Trim() != string.Empty )
				{
					defaultParams.Add("ItemPath", ItemPath_TextEdit.Text.Trim());
				}
				
						
				if(UserName_LayoutControlItem.Visibility == LayoutVisibility.Always && UserName_TextEdit.Text.Trim() != string.Empty )
				{
					defaultParams.Add("UserName", UserName_TextEdit.Text.Trim());
				}
				
						
				if(ExecutionId_LayoutControlItem.Visibility == LayoutVisibility.Always && ExecutionId_TextEdit.Text.Trim() != string.Empty )
				{
					defaultParams.Add("ExecutionId", ExecutionId_TextEdit.Text.Trim());
				}
				
						
				if(RequestType_LayoutControlItem.Visibility == LayoutVisibility.Always && RequestType_TextEdit.Text.Trim() != string.Empty )
				{
					defaultParams.Add("RequestType", RequestType_TextEdit.Text.Trim());
				}
				
						
				if(Format_LayoutControlItem.Visibility == LayoutVisibility.Always && Format_TextEdit.Text.Trim() != string.Empty )
				{
					defaultParams.Add("Format", Format_TextEdit.Text.Trim());
				}
				
						
						
				if(ItemAction_LayoutControlItem.Visibility == LayoutVisibility.Always && ItemAction_TextEdit.Text.Trim() != string.Empty )
				{
					defaultParams.Add("ItemAction", ItemAction_TextEdit.Text.Trim());
				}
				
						
				if(TimeStart_LayoutControlItem.Visibility == LayoutVisibility.Always && TimeStart_DateEdit.DateTime != null && TimeStart_DateEdit.Text != string.Empty)
				{
					var value = new System.DateTime();
					if(System.DateTime.TryParse(TimeStart_DateEdit.DateTime.ToString("yyyy-MM-dd"),out value))
					{
						defaultParams.Add("TimeStart",value);
					}
				}
				
						
				if(TimeEnd_LayoutControlItem.Visibility == LayoutVisibility.Always && TimeEnd_DateEdit.DateTime != null && TimeEnd_DateEdit.Text != string.Empty)
				{
					var value = new System.DateTime();
					if(System.DateTime.TryParse(TimeEnd_DateEdit.DateTime.ToString("yyyy-MM-dd"),out value))
					{
						defaultParams.Add("TimeEnd",value);
					}
				}
				
						
				if(TimeDataRetrieval_LayoutControlItem.Visibility == LayoutVisibility.Always && TimeDataRetrieval_CalcEdit.Value != null && TimeDataRetrieval_CalcEdit.Text != string.Empty)
				{
					var value = new Int32();
					if(Int32.TryParse(TimeDataRetrieval_CalcEdit.Value.ToString(),out value))
					{
						defaultParams.Add("TimeDataRetrieval",value);
					}
				}
				
						
				if(TimeProcessing_LayoutControlItem.Visibility == LayoutVisibility.Always && TimeProcessing_CalcEdit.Value != null && TimeProcessing_CalcEdit.Text != string.Empty)
				{
					var value = new Int32();
					if(Int32.TryParse(TimeProcessing_CalcEdit.Value.ToString(),out value))
					{
						defaultParams.Add("TimeProcessing",value);
					}
				}
				
						
				if(TimeRendering_LayoutControlItem.Visibility == LayoutVisibility.Always && TimeRendering_CalcEdit.Value != null && TimeRendering_CalcEdit.Text != string.Empty)
				{
					var value = new Int32();
					if(Int32.TryParse(TimeRendering_CalcEdit.Value.ToString(),out value))
					{
						defaultParams.Add("TimeRendering",value);
					}
				}
				
						
				if(Source_LayoutControlItem.Visibility == LayoutVisibility.Always && Source_TextEdit.Text.Trim() != string.Empty )
				{
					defaultParams.Add("Source", Source_TextEdit.Text.Trim());
				}
				
						
				if(Status_LayoutControlItem.Visibility == LayoutVisibility.Always && Status_TextEdit.Text.Trim() != string.Empty )
				{
					defaultParams.Add("Status", Status_TextEdit.Text.Trim());
				}
				
						
				if(ByteCount_LayoutControlItem.Visibility == LayoutVisibility.Always && ByteCount_CalcEdit.Value != null && ByteCount_CalcEdit.Text != string.Empty)
				{
					var value = new Int64();
					if(Int64.TryParse(ByteCount_CalcEdit.Value.ToString(),out value))
					{
						defaultParams.Add("ByteCount",value);
					}
				}
				
						
				if(RowCount_LayoutControlItem.Visibility == LayoutVisibility.Always && RowCount_CalcEdit.Value != null && RowCount_CalcEdit.Text != string.Empty)
				{
					var value = new Int64();
					if(Int64.TryParse(RowCount_CalcEdit.Value.ToString(),out value))
					{
						defaultParams.Add("RowCount",value);
					}
				}
				
						
				//AdditionalInfo doesn't have a match control
				
			            #endregion
			
            _recordCount = _executionlog3AdoBll.GetDataTableCount(defaultParams, GetSearchParams());
			
			int from = ((int)currentPageIndex - 1) * (int)_pageSize;
			int to = from+ (int)_pageSize;
			var dtSearch = _executionlog3AdoBll.GetDataTable(defaultParams, GetSearchParams(), from, to);
            
			this.gridControl1.DataSource = dtSearch;
			this.gridControl1.RefreshDataSource();
        }

        #endregion

        #region Event

        private void ExecutionLog3_Report_XtraUserControl_Load(object sender, EventArgs e)
        {
            BindData(1);
            paging_XtraUserControl1.SetPage(_pageSize, _recordCount, new Action<PagingHelper>(pagingHelper => BindData(pagingHelper.PageIndex)));
			new  UILogic.EventInject.ButtonEventService().InitEvent(this);
        }

        private void Search_Action_SimpleButton_Click(object sender, EventArgs e)
        {
            BindData(1);
            paging_XtraUserControl1.SetPage(_pageSize, _recordCount, new Action<PagingHelper>(pagingHelper => BindData(pagingHelper.PageIndex)));
        }

        private void Report_Action_SimpleButton_Click(object sender, EventArgs e)
        {
			
        }
        #endregion
    }
}
	