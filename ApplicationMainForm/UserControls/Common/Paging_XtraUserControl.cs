using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Infrastructure;

namespace ApplicationMainForm.UserControls
{
    public partial class Paging_XtraUserControl : DevExpress.XtraEditors.XtraUserControl
    {
        private PagingHelper _pagingHelper;

        public PagingHelper MyPagingHelper
        {
            get
            {
                return _pagingHelper;
            }
        }

        public Action<PagingHelper> RefreshPaging
        {
            set;
            get;
        }

        public Paging_XtraUserControl()
        {
            InitializeComponent();
        }

        public void SetPage(int pageSize, int recordCount)
        {
            _pagingHelper = new PagingHelper(pageSize, recordCount);
            RenderPageInfo();
        }

        public void SetPage(int pageSize, int recordCount, Action<PagingHelper> refreshPaging)
        {
            _pagingHelper = new PagingHelper(pageSize, recordCount);
            RefreshPaging = refreshPaging;
            RenderPageInfo();
        }

        private void RenderPageInfo()
        {
            this.RecordInfo_LabelControl.Text = string.Format(@"{0}/页 共{1}记录", _pagingHelper.PageSize, _pagingHelper.RecordCount);
            this.PageInfo_LabelControl.Text = string.Format(@"{0}/{1}页", _pagingHelper.PageIndex, _pagingHelper.PageCount);
        }


        private void FirstPage_HyperLinkEdit_OpenLink(object sender, DevExpress.XtraEditors.Controls.OpenLinkEventArgs e)
        {
            _pagingHelper.PageIndex = _pagingHelper.RecordCount == 0 ? 0 : 1;
            if (RefreshPaging != null)
                RefreshPaging(_pagingHelper);
            RenderPageInfo();
        }

        private void NextPage_HyperLinkEdit_OpenLink(object sender, DevExpress.XtraEditors.Controls.OpenLinkEventArgs e)
        {
            _pagingHelper.PageIndex = _pagingHelper.PageIndex < _pagingHelper.PageCount ? _pagingHelper.PageIndex + 1 : _pagingHelper.PageCount;
            if (RefreshPaging != null)
                RefreshPaging(_pagingHelper);
            RenderPageInfo();
        }

        private void PreviousPage_HyperLinkEdit_OpenLink(object sender, DevExpress.XtraEditors.Controls.OpenLinkEventArgs e)
        {
            _pagingHelper.PageIndex = _pagingHelper.PageIndex > 1 ? _pagingHelper.PageIndex - 1 : (_pagingHelper.RecordCount == 0 ? 0 : 1);
            if (RefreshPaging != null)
                RefreshPaging(_pagingHelper);
            RenderPageInfo();
        }

        private void LastPage_HyperLinkEdit_OpenLink(object sender, DevExpress.XtraEditors.Controls.OpenLinkEventArgs e)
        {
            _pagingHelper.PageIndex = _pagingHelper.PageCount;
            if (RefreshPaging != null)
                RefreshPaging(_pagingHelper);
            RenderPageInfo();
        }
    }
}
