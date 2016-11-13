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
    public partial class ExtendedDataSets_Report_XtraUserControl : DevExpress.XtraEditors.XtraUserControl
    {
        public ExtendedDataSets_Report_XtraUserControl()
        {
            InitializeComponent();
        }
		
		#region Property

        private readonly  ExtendedDataSetsAdoBLL _extendeddatasetsAdoBll = new ExtendedDataSetsAdoBLL();

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
						
				if(ID_LayoutControlItem.Visibility == LayoutVisibility.Always && ID_TextEdit.Text.Trim() != string.Empty )
				{
					defaultParams.Add("ID", ID_TextEdit.Text.Trim());
				}
				
						
				if(LinkID_LayoutControlItem.Visibility == LayoutVisibility.Always && LinkID_TextEdit.Text.Trim() != string.Empty )
				{
					defaultParams.Add("LinkID", LinkID_TextEdit.Text.Trim());
				}
				
						
				if(Name_LayoutControlItem.Visibility == LayoutVisibility.Always && Name_TextEdit.Text.Trim() != string.Empty )
				{
					defaultParams.Add("Name", Name_TextEdit.Text.Trim());
				}
				
						
				if(ItemID_LayoutControlItem.Visibility == LayoutVisibility.Always && ItemID_TextEdit.Text.Trim() != string.Empty )
				{
					defaultParams.Add("ItemID", ItemID_TextEdit.Text.Trim());
				}
				
			            #endregion
			
            _recordCount = _extendeddatasetsAdoBll.GetDataTableCount(defaultParams, GetSearchParams());
			
			int from = ((int)currentPageIndex - 1) * (int)_pageSize;
			int to = from+ (int)_pageSize;
			var dtSearch = _extendeddatasetsAdoBll.GetDataTable(defaultParams, GetSearchParams(), from, to);
            
			this.gridControl1.DataSource = dtSearch;
			this.gridControl1.RefreshDataSource();
        }

        #endregion

        #region Event

        private void ExtendedDataSets_Report_XtraUserControl_Load(object sender, EventArgs e)
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
	