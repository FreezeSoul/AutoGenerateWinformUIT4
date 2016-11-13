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
    public partial class ExtendedDataSources_Report_XtraUserControl : DevExpress.XtraEditors.XtraUserControl
    {
        public ExtendedDataSources_Report_XtraUserControl()
        {
            InitializeComponent();
        }
		
		#region Property

        private readonly  ExtendedDataSourcesAdoBLL _extendeddatasourcesAdoBll = new ExtendedDataSourcesAdoBLL();

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
						
				if(DSID_LayoutControlItem.Visibility == LayoutVisibility.Always && DSID_TextEdit.Text.Trim() != string.Empty )
				{
					defaultParams.Add("DSID", DSID_TextEdit.Text.Trim());
				}
				
						
				if(ItemID_LayoutControlItem.Visibility == LayoutVisibility.Always && ItemID_TextEdit.Text.Trim() != string.Empty )
				{
					defaultParams.Add("ItemID", ItemID_TextEdit.Text.Trim());
				}
				
						
				if(SubscriptionID_LayoutControlItem.Visibility == LayoutVisibility.Always && SubscriptionID_TextEdit.Text.Trim() != string.Empty )
				{
					defaultParams.Add("SubscriptionID", SubscriptionID_TextEdit.Text.Trim());
				}
				
						
				if(Name_LayoutControlItem.Visibility == LayoutVisibility.Always && Name_TextEdit.Text.Trim() != string.Empty )
				{
					defaultParams.Add("Name", Name_TextEdit.Text.Trim());
				}
				
						
				if(Extension_LayoutControlItem.Visibility == LayoutVisibility.Always && Extension_TextEdit.Text.Trim() != string.Empty )
				{
					defaultParams.Add("Extension", Extension_TextEdit.Text.Trim());
				}
				
						
				if(Link_LayoutControlItem.Visibility == LayoutVisibility.Always && Link_TextEdit.Text.Trim() != string.Empty )
				{
					defaultParams.Add("Link", Link_TextEdit.Text.Trim());
				}
				
						
				if(CredentialRetrieval_LayoutControlItem.Visibility == LayoutVisibility.Always && CredentialRetrieval_CalcEdit.Value != null && CredentialRetrieval_CalcEdit.Text != string.Empty)
				{
					var value = new Int32();
					if(Int32.TryParse(CredentialRetrieval_CalcEdit.Value.ToString(),out value))
					{
						defaultParams.Add("CredentialRetrieval",value);
					}
				}
				
						
						
						
						
					if(OriginalConnectStringExpressionBased_LayoutControlItem.Visibility == LayoutVisibility.Always && OriginalConnectStringExpressionBased_ComboBoxEdit.Text != "请选择" )
					{
						defaultParams.Add("OriginalConnectStringExpressionBased",OriginalConnectStringExpressionBased_ComboBoxEdit.Text=="是"?true:false);
					}
					
						
						
						
				if(Flags_LayoutControlItem.Visibility == LayoutVisibility.Always && Flags_CalcEdit.Value != null && Flags_CalcEdit.Text != string.Empty)
				{
					var value = new Int32();
					if(Int32.TryParse(Flags_CalcEdit.Value.ToString(),out value))
					{
						defaultParams.Add("Flags",value);
					}
				}
				
						
				if(Version_LayoutControlItem.Visibility == LayoutVisibility.Always && Version_CalcEdit.Value != null && Version_CalcEdit.Text != string.Empty)
				{
					var value = new Int32();
					if(Int32.TryParse(Version_CalcEdit.Value.ToString(),out value))
					{
						defaultParams.Add("Version",value);
					}
				}
				
			            #endregion
			
            _recordCount = _extendeddatasourcesAdoBll.GetDataTableCount(defaultParams, GetSearchParams());
			
			int from = ((int)currentPageIndex - 1) * (int)_pageSize;
			int to = from+ (int)_pageSize;
			var dtSearch = _extendeddatasourcesAdoBll.GetDataTable(defaultParams, GetSearchParams(), from, to);
            
			this.gridControl1.DataSource = dtSearch;
			this.gridControl1.RefreshDataSource();
        }

        #endregion

        #region Event

        private void ExtendedDataSources_Report_XtraUserControl_Load(object sender, EventArgs e)
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
	