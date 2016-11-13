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
    public partial class Users_List_XtraUserControl : DevExpress.XtraEditors.XtraUserControl
    {
        public Users_List_XtraUserControl()
        {
            InitializeComponent();
        }
		
		#region Property

        private readonly  UsersEntityBLL _usersEntityBll = new UsersEntityBLL();

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
            var modelParam = new Users();
			
			#region 查询条件赋值
						
				if(UserID_LayoutControlItem.Visibility == LayoutVisibility.Always && UserID_TextEdit.Text.Trim() != string.Empty )
				{
					keyList.Add("UserID");
					modelParam.UserID = new Guid(UserID_TextEdit.Text.Trim());
				}
				
						
				//Sid doesn't have a match control
				
						
				if(UserType_LayoutControlItem.Visibility == LayoutVisibility.Always && UserType_CalcEdit.Value != null && UserType_CalcEdit.Text != string.Empty)
				{
					var value = new Int32();
					if(Int32.TryParse(UserType_CalcEdit.Value.ToString(),out value))
					{
						keyList.Add("UserType");
						modelParam.UserType = value;
					}
				}
				
						
				if(AuthType_LayoutControlItem.Visibility == LayoutVisibility.Always && AuthType_CalcEdit.Value != null && AuthType_CalcEdit.Text != string.Empty)
				{
					var value = new Int32();
					if(Int32.TryParse(AuthType_CalcEdit.Value.ToString(),out value))
					{
						keyList.Add("AuthType");
						modelParam.AuthType = value;
					}
				}
				
						
				if(UserName_LayoutControlItem.Visibility == LayoutVisibility.Always && UserName_TextEdit.Text.Trim() != string.Empty )
				{
					keyList.Add("UserName");
					modelParam.UserName = UserName_TextEdit.Text.Trim();
				}
				
						//遍历赋值
			//XtraControlHelper.GetControlValueRecursion<BaseStationLog>(layoutControl1, ref modelParam, ref keyList);
            #endregion
			
            var qAll = _usersEntityBll.GetList(keyList, modelParam, GetSearchParams(), null, null);
            _recordCount = qAll.Count();

			var qSearch = _usersEntityBll.GetList(keyList, modelParam, GetSearchParams(), currentPageIndex, _pageSize);
            
			this.gridControl1.DataSource = qSearch.ToList();
			this.gridControl1.RefreshDataSource();
        }

        #endregion

        #region Event

        private void Users_List_XtraUserControl_Load(object sender, EventArgs e)
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
			var uc = new Users_Edit_XtraUserControl();
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
			var model = gridView1.GetRow(gridView1.FocusedRowHandle) as Users;
            if (model != null)
            {
				#region 打开查看页面
                var uc = new Users_Edit_XtraUserControl();
                uc.UserID = model.UserID;
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
			var model = gridView1.GetRow(gridView1.FocusedRowHandle) as Users;
            if (model != null)
            {
				#region 打开编辑页面
                var uc = new Users_Edit_XtraUserControl();
                uc.UserID = model.UserID;
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
                var model = gridView1.GetRow(gridView1.FocusedRowHandle) as Users;
                if (model != null)
                {
                    _usersEntityBll.DeleteByKey(model.UserID);
                    BindData(paging_XtraUserControl1.MyPagingHelper.PageIndex);
                }
            }
        }
        #endregion
    }
}
	