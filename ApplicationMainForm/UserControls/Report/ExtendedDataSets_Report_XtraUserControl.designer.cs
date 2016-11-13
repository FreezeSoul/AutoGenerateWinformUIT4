namespace ApplicationMainForm.UserControls.Report
{
    partial class ExtendedDataSets_Report_XtraUserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

		#region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
			            this.ID_TextEdit = new DevExpress.XtraEditors.TextEdit();
                        this.LinkID_TextEdit = new DevExpress.XtraEditors.TextEdit();
                        this.Name_TextEdit = new DevExpress.XtraEditors.TextEdit();
                        this.ItemID_TextEdit = new DevExpress.XtraEditors.TextEdit();
            			
			            this.ID_GridColumn = new DevExpress.XtraGrid.Columns.GridColumn();
                        this.LinkID_GridColumn = new DevExpress.XtraGrid.Columns.GridColumn();
                        this.Name_GridColumn = new DevExpress.XtraGrid.Columns.GridColumn();
                        this.ItemID_GridColumn = new DevExpress.XtraGrid.Columns.GridColumn();
                        this.Search_Action_SimpleButton = new DevExpress.XtraEditors.SimpleButton();
            this.Report_Action_SimpleButton = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();

							this.ID_LayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
							this.LinkID_LayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
							this.Name_LayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
							this.ItemID_LayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
						
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.paging_XtraUserControl1 = new ApplicationMainForm.UserControls.Paging_XtraUserControl();
            this.Paging_LayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
	        this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
			
							 ((System.ComponentModel.ISupportInitialize)(this.ID_LayoutControlItem)).BeginInit();
							 ((System.ComponentModel.ISupportInitialize)(this.LinkID_LayoutControlItem)).BeginInit();
							((System.ComponentModel.ISupportInitialize)(this.Name_LayoutControlItem)).BeginInit();
							((System.ComponentModel.ISupportInitialize)(this.ItemID_LayoutControlItem)).BeginInit();
			
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Paging_LayoutControlItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
						((System.ComponentModel.ISupportInitialize)( this.ID_TextEdit.Properties)).BeginInit();
            			((System.ComponentModel.ISupportInitialize)( this.LinkID_TextEdit.Properties)).BeginInit();
            			((System.ComponentModel.ISupportInitialize)( this.Name_TextEdit.Properties)).BeginInit();
            			((System.ComponentModel.ISupportInitialize)( this.ItemID_TextEdit.Properties)).BeginInit();
                        this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.layoutControl1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1193, 525);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "groupControl1";
			this.groupControl1.Text = "查询条件";

            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.paging_XtraUserControl1);
            this.layoutControl1.Controls.Add(this.gridControl1);
			           this.layoutControl1.Controls.Add(this.ID_TextEdit);
                       this.layoutControl1.Controls.Add(this.LinkID_TextEdit);
                       this.layoutControl1.Controls.Add(this.Name_TextEdit);
                       this.layoutControl1.Controls.Add(this.ItemID_TextEdit);
                        this.layoutControl1.Controls.Add(this.Search_Action_SimpleButton);
            this.layoutControl1.Controls.Add(this.Report_Action_SimpleButton);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(2, 23);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(1189, 500);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // gridControl1
            // 
            this.gridControl1.Location = new System.Drawing.Point(12, 63);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1165, 401);
            this.gridControl1.TabIndex = 19;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
						this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {this.ID_GridColumn });
						this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {this.LinkID_GridColumn });
						this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {this.Name_GridColumn });
						this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {this.ItemID_GridColumn });
			            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";

			            this.ID_GridColumn.Caption = "ID";
            this.ID_GridColumn.FieldName = "ID";
            this.ID_GridColumn.Name = "ID_GridColumn";
            this.ID_GridColumn.Visible = true;
            this.ID_GridColumn.VisibleIndex = 0;
                        this.LinkID_GridColumn.Caption = "LinkID";
            this.LinkID_GridColumn.FieldName = "LinkID";
            this.LinkID_GridColumn.Name = "LinkID_GridColumn";
            this.LinkID_GridColumn.Visible = true;
            this.LinkID_GridColumn.VisibleIndex = 1;
                        this.Name_GridColumn.Caption = "Name";
            this.Name_GridColumn.FieldName = "Name";
            this.Name_GridColumn.Name = "Name_GridColumn";
            this.Name_GridColumn.Visible = true;
            this.Name_GridColumn.VisibleIndex = 2;
                        this.ItemID_GridColumn.Caption = "ItemID";
            this.ItemID_GridColumn.FieldName = "ItemID";
            this.ItemID_GridColumn.Name = "ItemID_GridColumn";
            this.ItemID_GridColumn.Visible = true;
            this.ItemID_GridColumn.VisibleIndex = 3;
            
			

            this.ID_TextEdit.Name = "ID_TextEdit";
            this.ID_TextEdit.StyleController = this.layoutControl1;
            this.ID_TextEdit.TabIndex = 1;

            this.ID_LayoutControlItem.Control = this.ID_TextEdit;
            this.ID_LayoutControlItem.CustomizationFormText = "ID";
            this.ID_LayoutControlItem.Location = new System.Drawing.Point(0, 0);
            this.ID_LayoutControlItem.Name = "ID_LayoutControlItem";
            this.ID_LayoutControlItem.Size = new System.Drawing.Size(294, 25);
            this.ID_LayoutControlItem.Text = "ID";
											

            this.LinkID_TextEdit.Name = "LinkID_TextEdit";
            this.LinkID_TextEdit.StyleController = this.layoutControl1;
            this.LinkID_TextEdit.TabIndex = 2;

            this.LinkID_LayoutControlItem.Control = this.LinkID_TextEdit;
            this.LinkID_LayoutControlItem.CustomizationFormText = "LinkID";
            this.LinkID_LayoutControlItem.Location = new System.Drawing.Point(294, 0);
            this.LinkID_LayoutControlItem.Name = "LinkID_LayoutControlItem";
            this.LinkID_LayoutControlItem.Size = new System.Drawing.Size(292, 25);
            this.LinkID_LayoutControlItem.Text = "LinkID";
											

            this.Name_TextEdit.Name = "Name_TextEdit";
            this.Name_TextEdit.StyleController = this.layoutControl1;
            this.Name_TextEdit.TabIndex = 3;

            this.Name_LayoutControlItem.Control = this.Name_TextEdit;
            this.Name_LayoutControlItem.CustomizationFormText = "Name";
            this.Name_LayoutControlItem.Location = new System.Drawing.Point(586, 0);
            this.Name_LayoutControlItem.Name = "Name_LayoutControlItem";
            this.Name_LayoutControlItem.Size = new System.Drawing.Size(291, 25);
            this.Name_LayoutControlItem.Text = "Name";
											

            this.ItemID_TextEdit.Name = "ItemID_TextEdit";
            this.ItemID_TextEdit.StyleController = this.layoutControl1;
            this.ItemID_TextEdit.TabIndex = 4;

            this.ItemID_LayoutControlItem.Control = this.ItemID_TextEdit;
            this.ItemID_LayoutControlItem.CustomizationFormText = "ItemID";
            this.ItemID_LayoutControlItem.Location = new System.Drawing.Point(877, 0);
            this.ItemID_LayoutControlItem.Name = "ItemID_LayoutControlItem";
            this.ItemID_LayoutControlItem.Size = new System.Drawing.Size(292, 25);
            this.ItemID_LayoutControlItem.Text = "ItemID";
											            // 
            // Search_Action_SimpleButton
            // 
            this.Search_Action_SimpleButton.Name = "Search_Action_SimpleButton";
            this.Search_Action_SimpleButton.StyleController = this.layoutControl1;
            this.Search_Action_SimpleButton.TabIndex = 5;
            this.Search_Action_SimpleButton.Text = "查找";
			this.Search_Action_SimpleButton.Click += new System.EventHandler(this.Search_Action_SimpleButton_Click);
            // 
            // Report_Action_SimpleButton
            // 
            this.Report_Action_SimpleButton.Name = "Report_Action_SimpleButton";
            this.Report_Action_SimpleButton.StyleController = this.layoutControl1;
            this.Report_Action_SimpleButton.TabIndex = 6;
            this.Report_Action_SimpleButton.Text = "打印";
			this.Report_Action_SimpleButton.Click += new System.EventHandler(this.Report_Action_SimpleButton_Click);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem7,
            this.layoutControlItem6,
            this.layoutControlItem1,
            this.Paging_LayoutControlItem,
            this.emptySpaceItem2,
			this.emptySpaceItem1});
			
							this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {this.ID_LayoutControlItem});
							this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {this.LinkID_LayoutControlItem});
							this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {this.Name_LayoutControlItem});
							this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {this.ItemID_LayoutControlItem});
						
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(1189, 500);
            this.layoutControlGroup1.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup1.Text = "layoutControlGroup1";
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.Report_Action_SimpleButton;
            this.layoutControlItem7.CustomizationFormText = "layoutControlItem7";
            this.layoutControlItem7.Location = new System.Drawing.Point(586, 25);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(291, 26);
            this.layoutControlItem7.Text = "layoutControlItem7";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem7.TextToControlDistance = 0;
            this.layoutControlItem7.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.Search_Action_SimpleButton;
            this.layoutControlItem6.CustomizationFormText = "layoutControlItem6";
            this.layoutControlItem6.Location = new System.Drawing.Point(293, 25);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(293, 26);
            this.layoutControlItem6.Text = "layoutControlItem6";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextToControlDistance = 0;
            this.layoutControlItem6.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gridControl1;
            this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 51);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1169, 405);
            this.layoutControlItem1.Text = "layoutControlItem1";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextToControlDistance = 0;
            this.layoutControlItem1.TextVisible = false;
			// 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
            this.emptySpaceItem1.Location = new System.Drawing.Point(877, 25);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(292, 26);
            this.emptySpaceItem1.Text = "emptySpaceItem1";
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // paging_XtraUserControl1
            // 
            this.paging_XtraUserControl1.Name = "paging_XtraUserControl1";
            this.paging_XtraUserControl1.TabIndex = 20;
            // 
            // Paging_LayoutControlItem
            // 
            this.Paging_LayoutControlItem.Control = this.paging_XtraUserControl1;
            this.Paging_LayoutControlItem.CustomizationFormText = "Paging_LayoutControlItem";
            this.Paging_LayoutControlItem.Location = new System.Drawing.Point(0, 456);
            this.Paging_LayoutControlItem.Name = "Paging_LayoutControlItem";
            this.Paging_LayoutControlItem.Size = new System.Drawing.Size(1169, 24);
            this.Paging_LayoutControlItem.Text = "Paging_LayoutControlItem";
            this.Paging_LayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
            this.Paging_LayoutControlItem.TextToControlDistance = 0;
            this.Paging_LayoutControlItem.TextVisible = false;
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.CustomizationFormText = "emptySpaceItem2";
            this.emptySpaceItem2.Location = new System.Drawing.Point(0, 25);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(293, 26);
            this.emptySpaceItem2.Text = "emptySpaceItem2";
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // ExtendedDataSets_Report_XtraUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl1);
            this.Name = "ExtendedDataSets_Report_XtraUserControl";
            this.Size = new System.Drawing.Size(1193, 525);
			this.Load += new System.EventHandler(this.ExtendedDataSets_Report_XtraUserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
			
							 ((System.ComponentModel.ISupportInitialize)(this.ID_LayoutControlItem)).EndInit();
							 ((System.ComponentModel.ISupportInitialize)(this.LinkID_LayoutControlItem)).EndInit();
							((System.ComponentModel.ISupportInitialize)(this.Name_LayoutControlItem)).EndInit();
							((System.ComponentModel.ISupportInitialize)(this.ItemID_LayoutControlItem)).EndInit();
						
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Paging_LayoutControlItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
						((System.ComponentModel.ISupportInitialize)( this.ID_TextEdit.Properties)).EndInit();
            			((System.ComponentModel.ISupportInitialize)( this.LinkID_TextEdit.Properties)).EndInit();
            			((System.ComponentModel.ISupportInitialize)( this.Name_TextEdit.Properties)).EndInit();
            			((System.ComponentModel.ISupportInitialize)( this.ItemID_TextEdit.Properties)).EndInit();
            			
            this.ResumeLayout(false);

        }

        #endregion
		
		private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.SimpleButton Search_Action_SimpleButton;
        private DevExpress.XtraEditors.SimpleButton Report_Action_SimpleButton;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
		
				private DevExpress.XtraLayout.LayoutControlItem ID_LayoutControlItem;
					private DevExpress.XtraLayout.LayoutControlItem LinkID_LayoutControlItem;
					private DevExpress.XtraLayout.LayoutControlItem Name_LayoutControlItem;
					private DevExpress.XtraLayout.LayoutControlItem	ItemID_LayoutControlItem;
					
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private UserControls.Paging_XtraUserControl paging_XtraUserControl1;
        private DevExpress.XtraLayout.LayoutControlItem Paging_LayoutControlItem;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
		private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
				private DevExpress.XtraEditors.TextEdit ID_TextEdit;
        		private DevExpress.XtraEditors.TextEdit LinkID_TextEdit;
        		private DevExpress.XtraEditors.TextEdit Name_TextEdit;
        		private DevExpress.XtraEditors.TextEdit ItemID_TextEdit;
        		
				private DevExpress.XtraGrid.Columns.GridColumn ID_GridColumn;
        		private DevExpress.XtraGrid.Columns.GridColumn LinkID_GridColumn;
        		private DevExpress.XtraGrid.Columns.GridColumn Name_GridColumn;
        		private DevExpress.XtraGrid.Columns.GridColumn ItemID_GridColumn;
        		
	}
}
	