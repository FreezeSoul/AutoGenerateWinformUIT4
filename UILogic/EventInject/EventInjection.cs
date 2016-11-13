using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.ComponentModel;

namespace UILogic.EventInject
{
    public class EventInjection
    {
        public List<ControlEventDictionary> ControlEventList
        {
            set;
            get;
        }

        public event EventHandler beginEvent;
        public event EventHandler endEvent;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="controlEventList"></param>
        public EventInjection(List<ControlEventDictionary> controlEventList)
        {
            this.ControlEventList = controlEventList;
        }

        /// <summary>
        ///  初始化
        /// </summary>
        /// <param name="form"></param>
        public void Init(Form form)
        {
            foreach (Control cControl in form.Controls)
            {
                HandlerControl(cControl);
                RecursiveControlsHandler(cControl);
            }
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        /// <param name="control"></param>
        public void Init(Control control)
        {
            foreach (Control cControl in control.Controls)
            {
                HandlerControl(cControl);
                RecursiveControlsHandler(cControl);
            }
        }


        /// <summary>
        /// 递归控件
        /// </summary>
        /// <param name="pControl"></param>
        private void RecursiveControlsHandler(Control pControl)
        {
            foreach (Control cControl in pControl.Controls)
            {
                HandlerControl(cControl);
                RecursiveControlsHandler(cControl);
            }
        }

        /// <summary>
        /// 控件Handle By FreezeSoul
        /// </summary>
        /// <param name="cControl"></param>
        private void HandlerControl(Control cControl)
        {
            foreach (var controlEventObj in ControlEventList)
            {
                if (cControl.GetType().Equals(controlEventObj.ControlType))
                {
                    var eventInfos = cControl.GetType().GetEvents();
                    if (eventInfos.ToList().Exists(eventTpyeObj => eventTpyeObj.Name == controlEventObj.EventType))
                    {
                        var propertyInfo = cControl.GetType().GetProperty("Events", BindingFlags.Instance | BindingFlags.NonPublic);
                        var eventHandlerList = propertyInfo.GetValue(cControl, null) as EventHandlerList;
                        var fieldInfo = typeof(Control).GetField("Event" + controlEventObj.EventType, BindingFlags.Static | BindingFlags.NonPublic);
                        if (fieldInfo != null && eventHandlerList != null)
                        {
                            var d = eventHandlerList[fieldInfo.GetValue(cControl)];
                            if (d != null)
                            {
                                var deTempList = new List<Delegate>();
                                d.GetInvocationList().ToList().ForEach(de =>
                                {
                                    deTempList.Add(de);
                                    eventHandlerList.RemoveHandler(fieldInfo.GetValue(cControl), de);
                                });
                                eventHandlerList.AddHandler(fieldInfo.GetValue(cControl), beginEvent);
                                deTempList.ForEach(de => eventHandlerList.AddHandler(fieldInfo.GetValue(cControl), de));
                                eventHandlerList.AddHandler(fieldInfo.GetValue(cControl), endEvent);
                            }
                        }
                    }
                }
            }
        }
       
    }

    public class ControlEventDictionary
    {
        public string EventType
        {
            set;
            get;
        }
        public Type ControlType
        {
            set;
            get;
        }
    }
}
