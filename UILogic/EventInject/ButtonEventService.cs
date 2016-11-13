using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;

namespace UILogic.EventInject
{
    public class ButtonEventService
    {
        private XtraForm _form;
        private static WaitDialogForm _wform;

        public void InitEvent(XtraForm form)
        {
            var ced = new List<ControlEventDictionary>
                          {
                              new ControlEventDictionary
                                  {
                                      EventType = "Click",
                                      ControlType = typeof (SimpleButton)
                                  }
                          };
            var eli = new EventInjection(ced);
            eli.beginEvent += BeginInvoke;
            eli.endEvent += EndInvoke;
            _form = form;
            eli.Init(form);
        }

        public void InitEvent(XtraUserControl userControl)
        {
            var ced = new List<ControlEventDictionary>
                          {
                              new ControlEventDictionary
                                  {
                                      EventType = "Click",
                                      ControlType = typeof (SimpleButton)
                                  }
                          };
            var eli = new EventInjection(ced);
            eli.beginEvent += BeginInvoke;
            eli.endEvent += EndInvoke;
            eli.Init(userControl);
        }

        private void BeginInvoke(object sender, EventArgs e)
        {
            if (_wform == null)
            {
                _wform = new WaitDialogForm("正在加载,请稍候...");
            }
            _wform.Show();
            Application.DoEvents();
        }

        private void EndInvoke(object sender, EventArgs e)
        {
            if (_wform != null)
            {
                _wform.Hide();
            }
        }
    }
}