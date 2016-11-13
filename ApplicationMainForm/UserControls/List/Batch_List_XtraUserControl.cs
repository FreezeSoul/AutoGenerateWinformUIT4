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
using BusinessLogicLayer.Entity;
using DomainModelLayer;
using Infrastructure;
using UILogic;

namespace ApplicationMainForm.UserControls.List
{
    public partial class Batch_List_XtraUserControl : DevExpress.XtraEditors.XtraUserControl
    {
        public Batch_List_XtraUserControl()
        {
            InitializeComponent();
        }
		
		#region Property

        private readonly  BatchEntityBLL _batchEntityBll = new BatchEntityBLL();

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
            var keyList = new List<string>();
            var modelParam = new Batch();
			
			#region 查询条件赋值
						
				if(BatchID_LayoutControlItem.Visibility == LayoutVisibility.Always && BatchID_TextEdit.Text.Trim() != string.Empty )
				{
					keyList.Add("BatchID");
					modelParam.BatchID = new Guid(BatchID_TextEdit.Text.Trim());
				}
				
						
				if(AddedOn_LayoutControlItem.Visibility == LayoutVisibility.Always && AddedOn_DateEdit.DateTime != null && AddedOn_DateEdit.Text != string.Empty)
				{
					var value = new System.DateTime();
					if(System.DateTime.TryParse(AddedOn_DateEdit.DateTime.ToString("yyyy-MM-dd"),out value))
					{
						keyList.Add("AddedOn");
						modelParam.AddedOn = value;
					}
				}
				
						
				if(Action_LayoutControlItem.Visibility == LayoutVisibility.Always && Action_TextEdit.Text.Trim() != string.Empty )
				{
					keyList.Add("Action");
					modelParam.Action = Action_TextEdit.Text.Trim();
				}
				
						
				if(Item_LayoutControlItem.Visibility == LayoutVisibility.Always && Item_TextEdit.Text.Trim() != string.Empty )
				{
					keyList.Add("Item");
					modelParam.Item = Item_TextEdit.Text.Trim();
				}
				
						
				if(Parent_LayoutControlItem.Visibility == LayoutVisibility.Always && Parent_TextEdit.Text.Trim() != string.Empty )
				{
					keyList.Add("Parent");
					modelParam.Parent = Parent_TextEdit.Text.Trim();
				}
				
						
				if(Param_LayoutControlItem.Visibility == LayoutVisibility.Always && Param_TextEdit.Text.Trim() != string.Empty )
				{
					keyList.Add("Param");
					modelParam.Param = Param_TextEdit.Text.Trim();
				}
				
						
					if(BoolParam_LayoutControlItem.Visibility == LayoutVisibility.Always && BoolParam_ComboBoxEdit.Text != "请选择" )
					{
						keyList.Add("BoolParam");
						modelParam.BoolParam = BoolParam_ComboBoxEdit.Text=="是"?true:false;
					}
					
						
						
						//遍历赋值
			//XtraControlHelper.GetControlValueRecursion<BaseStationLog>(layoutControl1, ref modelParam, ref keyList);
            #endregion
			
            var qAll = _batchEntityBll.GetList(keyList, modelParam, GetSearchParams(), null, null);
            _recordCount = qAll.Count();

			var qSearch = _batchEntityBll.GetList(keyList, modelParam, GetSearchParams(), currentPageIndex, _pageSize);
            
			this.gridControl1.DataSource = qSearch.ToList();
			this.gridControl1.RefreshDataSource();
        }

        #endregion

        #region Event

        private void Batch_List_XtraUserControl_Load(object sender, EventArgs e)
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
		
        private void Add_Action_SimpleButton_Click(object sender, EventArgs e)
        {
			#region 打开添加页面
			var uc = new Batch_Edit_XtraUserControl();
            uc.State = FormState.Add;
            var shell = new Shell_XtraForm()
                            {
                                ManualWidth = uc.Width,
                                ManualHeight = uc.Height + 50
                            };
			uc.FormContainer = shell;				
            shell.ResizeForm();
            shell.LoadControl(uc);
			shell.Closed += (obj, arg) => BindData(paging_XtraUserControl1.MyPagingHelper.PageIndex);
            shell.Show();
			#endregion
        }
		
        private void View_RepositoryItemHyperLinkEdit_Click(object sender, EventArgs e)
        {
			var model = gridView1.GetRow(gridView1.FocusedRowHandle) as Batch;
            if (model != null)
            {
				#region 打开查看页面
                var uc = new Batch_Edit_XtraUserControl();
                uc.BatchID = model.BatchID.ToString();
                uc.State = FormState.View;
                var shell = new Shell_XtraForm()
                                {
                                    ManualWidth = uc.Width,
                                    ManualHeight = uc.Height + 50
                                };
				uc.FormContainer = shell;
                shell.ResizeForm();
                shell.LoadControl(uc);
				shell.Closed += (obj, arg) => BindData(paging_XtraUserControl1.MyPagingHelper.PageIndex);
                shell.Show();
				#endregion
            }
        }

        private void Edit_RepositoryItemHyperLinkEdit_Click(object sender, EventArgs e)
        {
			var model = gridView1.GetRow(gridView1.FocusedRowHandle) as Batch;
            if (model != null)
            {
				#region 打开编辑页面
                var uc = new Batch_Edit_XtraUserControl();
                uc.BatchID = model.BatchID.ToString();
                uc.State = FormState.Edit;
                var shell = new Shell_XtraForm()
                                {
                                    ManualWidth = uc.Width,
                                    ManualHeight = uc.Height + 50
                                };
				uc.FormContainer = shell;
                shell.ResizeForm();
                shell.LoadControl(uc);
				shell.Closed += (obj, arg) => BindData(paging_XtraUserControl1.MyPagingHelper.PageIndex);
                shell.Show();
				#endregion
            }
        }

        private void Delete_RepositoryItemHyperLinkEdit_Click(object sender, EventArgs e)
        {
			if (XtraExtension.Confirm("请确认是否要删除当前记录?") == DialogResult.OK)
            {
                var model = gridView1.GetRow(gridView1.FocusedRowHandle) as Batch;
                if (model != null)
                {
                    _batchEntityBll.DeleteByKey(model.BatchID.ToString());
                    BindData(paging_XtraUserControl1.MyPagingHelper.PageIndex);
                }
            }
        }
        #endregion
    }
}
	