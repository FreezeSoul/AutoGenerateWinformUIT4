using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using DevExpress.XtraTab.ViewInfo;
using Infrastructure;
using UILogic;

namespace ApplicationMainForm
{
    public partial class Main_RibbonForm : DevExpress.XtraBars.Ribbon.RibbonForm, IMenuActionBehavior
    {
        #region FormEvent
        public Main_RibbonForm()
        {
            InitializeComponent();
            xtraTabControl1.AllowDrop = true;
            xtraTabControl1.DragOver += new DragEventHandler(xtraTabControl1_DragOver);
            xtraTabControl1.MouseDown += new MouseEventHandler(xtraTabControl1_MouseDown);
            CloseTab_BarButtonItem.ItemClick += new ItemClickEventHandler(CloseTab_BarButtonItem_ItemClick);
            CloseAllTabs_BarButtonItem.ItemClick += new ItemClickEventHandler(CloseAllTabs_BarButtonItem_ItemClick);
            CloseOtherTabs_BarButtonItem.ItemClick += new ItemClickEventHandler(CloseOtherTabs_BarButtonItem_ItemClick);
        }

        private void Main_RibbonForm_Load(object sender, EventArgs e)
        {
            new MenuHelper(this).InitMenuTree(this.navBarControl1, Application.StartupPath + "\\MenuData.xml");
        }
        #endregion

        #region ControlEvent

        private bool _hoverFlag;
        private XtraTabPage _currentTabPag;
        protected void xtraTabControl1_DragOver(object sender, DragEventArgs e)
        {
            var pt = MousePosition;
            var tabCtrl = sender as XtraTabControl;
            var hover_tab = tabCtrl.CalcHitInfo(tabCtrl.PointToClient(pt)).Page;
            if (e.KeyState == 1)
            {
                if (hover_tab != null)
                {
                    _hoverFlag = true;
                    if (e.Data.GetDataPresent(typeof(XtraTabPage)))
                    {
                        e.Effect = DragDropEffects.Move;
                        var drag_tab = (XtraTabPage)e.Data.GetData(typeof(XtraTabPage));
                        var item_drag_index = FindIndex(tabCtrl, drag_tab);
                        var drop_location_index = FindIndex(tabCtrl, hover_tab);
                        if (item_drag_index != drop_location_index)
                        {
                            tabCtrl.TabPages.Move(drop_location_index, drag_tab);
                            tabCtrl.TabPages.Move(item_drag_index, hover_tab);
                            tabCtrl.SelectedTabPage = drag_tab;
                        }
                    }
                }
                else
                {
                    _hoverFlag = false;
                    if (e.Data.GetDataPresent(typeof(XtraTabPage)))
                    {
                        e.Effect = DragDropEffects.Move;
                        _currentTabPag = (XtraTabPage)e.Data.GetData(typeof(XtraTabPage));
                    }
                }
            }

        }

        protected void xtraTabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            var pt = MousePosition;
            var tabCtrl = sender as XtraTabControl;
            var tp = tabCtrl.CalcHitInfo(tabCtrl.PointToClient(pt)).Page;
            if (e.Button == MouseButtons.Left)
            {
                if (tp != null)
                {
                    DoDragDrop(tp, DragDropEffects.Move);
                }
                if (_currentTabPag != null && _hoverFlag == false)
                {
                    if (_currentTabPag.Controls.Count == 0) return;
                    var uc = _currentTabPag.Controls[0];
                    uc.Dock = DockStyle.Fill;

                    var dockPanel1 =
                        dockManager1.AddPanel(new Point((this.Size.Width - uc.Size.Width)/2,
                                                        (this.Size.Height - uc.Size.Height)/2));
                    var controlContainer1 = dockPanel1.ControlContainer;
                    dockPanel1.Text = _currentTabPag.Text;
                    dockPanel1.FloatSize = uc.Size;
                    dockPanel1.Tag = _currentTabPag;
                    dockPanel1.ClosedPanel += new DockPanelEventHandler((obj, arg) =>
                                                                            {
                                                                                tabCtrl.TabPages.Add(_currentTabPag);
                                                                                _currentTabPag.Controls.Add(uc);
                                                                            });
                    controlContainer1.Controls.Add(uc);
                    tabCtrl.TabPages.Remove(_currentTabPag);
                    _hoverFlag = true;
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                var info = tabCtrl.CalcHitInfo(tabCtrl.PointToClient(pt));
                if (info.HitTest == XtraTabHitTest.PageHeader)
                {
                    popupMenu1.ShowPopup(pt);
                }
            }
        }


        private int FindIndex(XtraTabControl tabControl, XtraTabPage page)
        {
            var i = 0;
            while (i < tabControl.TabPages.Count)
            {
                if (tabControl.TabPages[i].Equals(page))
                {
                    return i;
                }
                i += 1;
            }
            return -1;
        }

        private void CloseAllTabs_BarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (XtraTabPage tabPage in xtraTabControl1.TabPages)
            {
                if (TabPages.ContainsKey(tabPage.Name))
                {
                    TabPages.Remove(tabPage.Name);
                }
            }
            xtraTabControl1.TabPages.Clear();
        }

        private void CloseTab_BarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (TabPages.ContainsKey(xtraTabControl1.SelectedTabPage.Name))
            {
                TabPages.Remove(xtraTabControl1.SelectedTabPage.Name);
            }
            xtraTabControl1.TabPages.Remove(xtraTabControl1.SelectedTabPage);
        }

        private void CloseOtherTabs_BarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            XtraTabPage leavePage = null;
            foreach (XtraTabPage tabPage in xtraTabControl1.TabPages)
            {
                if (TabPages.ContainsKey(tabPage.Name) && xtraTabControl1.SelectedTabPage != tabPage)
                {
                    TabPages.Remove(tabPage.Name);
                }
                else
                {
                    leavePage = tabPage;
                }
            }
            xtraTabControl1.TabPages.Clear();
            if (leavePage != null)
                xtraTabControl1.TabPages.Add(leavePage);
        }

        private void xtraTabControl1_CloseButtonClick(object sender, EventArgs e)
        {
            if (TabPages.ContainsKey(((XtraTabControl)sender).SelectedTabPage.Name))
            {
                TabPages.Remove(((XtraTabControl)sender).SelectedTabPage.Name);
            }
            xtraTabControl1.TabPages.Remove(((XtraTabControl)sender).SelectedTabPage);
        }

        #endregion

        #region MenuLogic

        protected Dictionary<string, XtraTabPage> TabPages = new Dictionary<string, XtraTabPage>();
        protected Dictionary<string, XtraForm> XtraForms = new Dictionary<string, XtraForm>();

        public Action<MenuActionArgs> InvokeMethod
        {
            get
            {
                return obj =>
                           {
                               if (obj.Type == "XtraForm")
                               {
                                   OpenFrm(string.Format("{0}.{1}", obj.NameSpace, obj.ClassName), obj.Describe);
                               }
                               if (obj.Type == "UserControl")
                               {
                                   LoadUc(string.Format("{0}.{1}", obj.NameSpace, obj.ClassName), obj.Describe);
                               }
                           };
            }
        }

        private void OpenFrm(string formName, string title)
        {
            if (XtraForms.ContainsKey(formName) && !XtraForms[formName].IsDisposed)
            {
                XtraForms[formName].Activate();
            }
            else
            {
                var frm = ReflectionHelper.GetObjFromAssemblyByName<Main_RibbonForm>(formName) as XtraForm;
                if (frm != null)
                {
                    if (!XtraForms.ContainsKey(formName))
                        XtraForms.Add(formName, frm);
                    else
                        XtraForms[formName] = frm;
                    frm.Text = title;
                    frm.Show();
                }
            }
        }

        private void LoadUc(string ucName, string title)
        {

            if (TabPages.ContainsKey(ucName))
            {
                xtraTabControl1.SelectedTabPage = TabPages[ucName];
            }
            else
            {
                TabPages.Add(ucName, new XtraTabPage()
                                        {
                                            Text = title,
                                            Name = ucName
                                        });
                var uc = ReflectionHelper.GetObjFromAssemblyByName<Main_RibbonForm>(ucName) as XtraUserControl;
                if (uc != null)
                {
                    xtraTabControl1.TabPages.Add(TabPages[ucName]);
                    TabPages[ucName].LoadControl(uc);
                    xtraTabControl1.SelectedTabPage = TabPages[ucName];
                }
            }
        }
        #endregion
    }
}