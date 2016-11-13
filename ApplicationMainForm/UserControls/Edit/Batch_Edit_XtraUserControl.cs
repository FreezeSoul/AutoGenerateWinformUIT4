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
    public partial class Batch_Edit_XtraUserControl : DevExpress.XtraEditors.XtraUserControl
    {
		
        public Batch_Edit_XtraUserControl()
        {
            InitializeComponent();
        }
		
		#region Property

        public String BatchID { set; get; }

        public FormState State { set; get; }

        public XtraForm FormContainer { set; get; }

        private Batch _batch;

        private readonly BatchEntityBLL _batchEntityBll = new BatchEntityBLL();
        
        #endregion 

        #region Method
		
  		private void SetTitle()
        {
            switch (State)
            {
                case FormState.View:
                    this.groupControl1.Text = "Batch信息查看";
					this.FormContainer.Text = "Batch";
                    break;
                case FormState.Add:
                    this.groupControl1.Text = "Batch信息增加";
					this.FormContainer.Text = "Batch";
                    break;
                case FormState.Edit:
                    this.groupControl1.Text = "Batch信息修改";
					this.FormContainer.Text = "Batch";
                    break;
            }
        }
		
   		private bool ValidateData(List<string> mappedList, ref List<string> errorInfo)
        {
            var flag = true;
						 if (!mappedList.Contains("BatchID"))
            {
                errorInfo.Add(string.Format("{0}不得为空！","BatchID"));
                flag = false;
            }
						 if (!mappedList.Contains("AddedOn"))
            {
                errorInfo.Add(string.Format("{0}不得为空！","AddedOn"));
                flag = false;
            }
						 if (!mappedList.Contains("Action"))
            {
                errorInfo.Add(string.Format("{0}不得为空！","Action"));
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
            _batch = _batchEntityBll.GetByKey(this.BatchID);
            if (_batch != null)
            {
            						
				if(BatchID_LayoutControlItem.Visibility == LayoutVisibility.Always)
				{
					if(_batch.BatchID!=null)
					{
                        BatchID_TextEdit.Text = _batch.BatchID.ToString().Trim();
					}
				}
				
									
				if(AddedOn_LayoutControlItem.Visibility == LayoutVisibility.Always)
				{
					if(_batch.AddedOn!=null)
					{
						AddedOn_DateEdit.DateTime = _batch.AddedOn;
					}
				}
				
									
				if(Action_LayoutControlItem.Visibility == LayoutVisibility.Always)
				{
					if(_batch.Action!=null)
					{
					 	Action_TextEdit.Text = _batch.Action.Trim();
					}
				}
				
									
				if(Item_LayoutControlItem.Visibility == LayoutVisibility.Always)
				{
					if(_batch.Item!=null)
					{
					 	Item_TextEdit.Text = _batch.Item.Trim();
					}
				}
				
									
				if(Parent_LayoutControlItem.Visibility == LayoutVisibility.Always)
				{
					if(_batch.Parent!=null)
					{
					 	Parent_TextEdit.Text = _batch.Parent.Trim();
					}
				}
				
									
				if(Param_LayoutControlItem.Visibility == LayoutVisibility.Always)
				{
					if(_batch.Param!=null)
					{
					 	Param_TextEdit.Text = _batch.Param.Trim();
					}
				}
				
									
					if(BoolParam_LayoutControlItem.Visibility == LayoutVisibility.Always)
					{
						if(_batch.BoolParam!=null)
						{
							BoolParam_ComboBoxEdit.Text = _batch.BoolParam.Value?"是":"否";
						}
					}
					
									
									
				            }
			#endregion
        }

        private void MapData(ref List<string> mappedList)
        {
			#region 控件取值
            if (_batch != null)
            {
										
				if(BatchID_LayoutControlItem.Visibility == LayoutVisibility.Always && BatchID_TextEdit.Text.Trim() != string.Empty )
				{
					mappedList.Add("BatchID");
					_batch.BatchID = new Guid(BatchID_TextEdit.Text.Trim());
				}
				
											
				if(AddedOn_LayoutControlItem.Visibility == LayoutVisibility.Always && AddedOn_DateEdit.DateTime != null && AddedOn_DateEdit.Text != string.Empty)
				{
					var value = new System.DateTime();
					if(System.DateTime.TryParse(AddedOn_DateEdit.DateTime.ToString("yyyy-MM-dd HH:mm:ss"),out value))
					{
						mappedList.Add("AddedOn");
						_batch.AddedOn = value;
					}
				}
				
											
				if(Action_LayoutControlItem.Visibility == LayoutVisibility.Always && Action_TextEdit.Text.Trim() != string.Empty )
				{
					mappedList.Add("Action");
					_batch.Action = Action_TextEdit.Text.Trim();
				}
				
											
				if(Item_LayoutControlItem.Visibility == LayoutVisibility.Always && Item_TextEdit.Text.Trim() != string.Empty )
				{
					mappedList.Add("Item");
					_batch.Item = Item_TextEdit.Text.Trim();
				}
				
											
				if(Parent_LayoutControlItem.Visibility == LayoutVisibility.Always && Parent_TextEdit.Text.Trim() != string.Empty )
				{
					mappedList.Add("Parent");
					_batch.Parent = Parent_TextEdit.Text.Trim();
				}
				
											
				if(Param_LayoutControlItem.Visibility == LayoutVisibility.Always && Param_TextEdit.Text.Trim() != string.Empty )
				{
					mappedList.Add("Param");
					_batch.Param = Param_TextEdit.Text.Trim();
				}
				
											
					if(BoolParam_LayoutControlItem.Visibility == LayoutVisibility.Always && BoolParam_ComboBoxEdit.Text != "请选择" )
					{
						mappedList.Add("BoolParam");
						_batch.BoolParam = BoolParam_ComboBoxEdit.Text=="是"?true:false;
					}
					
											
											
					            }
			#endregion
        }

        #endregion

        #region Event

        private void  Batch_Edit_XtraUserControl_Load(object sender, EventArgs e)
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
                    _batch = new Batch();
                    MapData(ref mappedList);
					 if (ValidateData(mappedList,ref msgErrors))
                    {
	                    _batchEntityBll.Add(_batch);
	                    _batchEntityBll.Save();
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
                    	_batchEntityBll.Save();
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
	