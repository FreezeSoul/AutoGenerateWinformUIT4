using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BusinessLogicLayer;
using BusinessLogicLayer.Entity;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Utils;
using DomainModelLayer;
using Infrastructure;
using UILogic;

namespace ApplicationMainForm.UserControls.Edit
{
    public partial class Users_Edit_XtraUserControl : DevExpress.XtraEditors.XtraUserControl
    {
		
        public Users_Edit_XtraUserControl()
        {
            InitializeComponent();
        }
		
		#region Property

        public System.Guid UserID { set; get; }

        public FormState State { set; get; }

        public XtraForm FormContainer { set; get; }

        private Users _users;

        private readonly UsersEntityBLL _usersEntityBll = new UsersEntityBLL();
        
        #endregion 

        #region Method
		
  		private void SetTitle()
        {
            switch (State)
            {
                case FormState.View:
                    this.groupControl1.Text = "Users信息查看";
					this.FormContainer.Text = "Users";
                    break;
                case FormState.Add:
                    this.groupControl1.Text = "Users信息增加";
					this.FormContainer.Text = "Users";
                    break;
                case FormState.Edit:
                    this.groupControl1.Text = "Users信息修改";
					this.FormContainer.Text = "Users";
                    break;
            }
        }
		
   		private bool ValidateData(List<string> mappedList, ref List<string> errorInfo)
        {
            var flag = true;
						 if (!mappedList.Contains("UserType"))
            {
                errorInfo.Add(string.Format("{0}不得为空！","UserType"));
                flag = false;
            }
						 if (!mappedList.Contains("AuthType"))
            {
                errorInfo.Add(string.Format("{0}不得为空！","AuthType"));
                flag = false;
            }
			            return flag;
        }
		
        private void CheckRole()
        {

        }
		
        private void BindData()
        {
			#region 控件赋值
            _users = _usersEntityBll.GetByKey(this.UserID);
            if (_users != null)
            {
            						
				if(UserID_LayoutControlItem.Visibility == LayoutVisibility.Always)
				{
					if(_users.UserID!=null)
					{
                        UserID_TextEdit.Text = _users.UserID.ToString().Trim();
					}
				}
				
									
				//Sid doesn't have a match control
				
									
				if(UserType_LayoutControlItem.Visibility == LayoutVisibility.Always)
				{
					if(_users.UserType!=null)
					{
						UserType_CalcEdit.Value = Convert.ToDecimal(_users.UserType);
					}
				}
				
									
				if(AuthType_LayoutControlItem.Visibility == LayoutVisibility.Always)
				{
					if(_users.AuthType!=null)
					{
						AuthType_CalcEdit.Value = Convert.ToDecimal(_users.AuthType);
					}
				}
				
									
				if(UserName_LayoutControlItem.Visibility == LayoutVisibility.Always)
				{
					if(_users.UserName!=null)
					{
					 	UserName_TextEdit.Text = _users.UserName.Trim();
					}
				}
				
				            }
			#endregion
        }

        private void MapData(ref List<string> mappedList)
        {
			#region 控件取值
            if (_users != null)
            {
										
				//Sid doesn't have a match control
				
											
				if(UserType_LayoutControlItem.Visibility == LayoutVisibility.Always && UserType_CalcEdit.Value != null && UserType_CalcEdit.Text != string.Empty)
				{
					var value = new Int32();
					if(Int32.TryParse(UserType_CalcEdit.Value.ToString(),out value))
					{
						mappedList.Add("UserType");
						_users.UserType = value;
					}
				}
				
											
				if(AuthType_LayoutControlItem.Visibility == LayoutVisibility.Always && AuthType_CalcEdit.Value != null && AuthType_CalcEdit.Text != string.Empty)
				{
					var value = new Int32();
					if(Int32.TryParse(AuthType_CalcEdit.Value.ToString(),out value))
					{
						mappedList.Add("AuthType");
						_users.AuthType = value;
					}
				}
				
											
				if(UserName_LayoutControlItem.Visibility == LayoutVisibility.Always && UserName_TextEdit.Text.Trim() != string.Empty )
				{
					mappedList.Add("UserName");
					_users.UserName = UserName_TextEdit.Text.Trim();
				}
				
					            }
			#endregion
        }

        #endregion

        #region Event

        private void  Users_Edit_XtraUserControl_Load(object sender, EventArgs e)
        {
            if (State == FormState.Edit || State == FormState.View)
            {
				 this.Save_Action_SimpleButton.Enabled = State == FormState.Edit;
                BindData();
            }
			SetTitle();
            CheckRole();
			new  UILogic.EventInject.ButtonEventService().InitEvent(this.FormContainer);
        }

        private void Save_Action_SimpleButton_Click(object sender, EventArgs e)
        {
            if (XtraExtension.Confirm(@"确认是否保存数据!") == DialogResult.OK)
            {
			    var msgErrors = new List<string>();
				var mappedList = new List<string>();
                if (State == FormState.Add)
                {
                    _users = new Users();
                    MapData(ref mappedList);
					 if (ValidateData(mappedList,ref msgErrors))
                    {
	                    _usersEntityBll.Add(_users);
	                    _usersEntityBll.Save();
					}
					else
                    {
                        XtraExtension.ShowAlarm(msgErrors.GetJoinListStr("\r\n"));
                    }
                }
                if (State == FormState.Edit)
                {
                    MapData(ref mappedList);
					 if (ValidateData(mappedList,ref msgErrors))
                    {
                    	_usersEntityBll.Save();
					}
					else
                    {
                        XtraExtension.ShowAlarm(msgErrors.GetJoinListStr("\r\n"));
                    }
                }
            }
        }

        private void Cancel_Action_SimpleButton_Click(object sender, EventArgs e)
        {
            if (FormContainer != null)
                FormContainer.Close();
        }

        #endregion
    }
}
	