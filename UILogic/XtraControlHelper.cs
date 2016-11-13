using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraGrid.Views.Grid;
using System.Reflection;

namespace UILogic
{
    public class XtraControlHelper
    {

        #region 递归读取父控件容器子控件值设置datatable


        public static void GetControlValueRecursion<T>(Control parent, ref T obj, ref List<string> list)
        {
            foreach (Control c in parent.Controls)
            {
                GetControlValueRecursion<T>(c, ref obj, ref list);
                GetValue<T>(c, ref obj, ref list);
            }
        }

        public static void GetValue<T>(Control cr, ref T obj, ref List<string> list)
        {
            PropertyInfo property;
            switch (cr.ToString())
            {
                case "DevExpress.XtraEditors.TextEdit":
                    property = obj.GetType().GetProperties().ToList().Where(item => item.Name + "_TextEdit" == cr.Name).FirstOrDefault();
                    if (property != null)
                    {
                        if (!cr.Text.Trim().Equals(string.Empty))
                        {
                            list.Add(property.Name);
                            property.SetValue(obj, cr.Text, null);
                        }
                    }
                    break;
                case "DevExpress.XtraEditors.MemoEdit":
                    property = obj.GetType().GetProperties().ToList().Where(item => item.Name + "_MemoEdit" == cr.Name).FirstOrDefault();
                    if (property != null)
                    {
                        if (!cr.Text.Trim().Equals(string.Empty))
                        {
                            list.Add(property.Name);
                            property.SetValue(obj, cr.Text, null);
                        }
                    }
                    break;
                case "DevExpress.XtraEditors.ComboBoxEdit":
                    property = obj.GetType().GetProperties().ToList().Where(item => item.Name + "_ComboBoxEdit" == cr.Name).FirstOrDefault();
                    if (property != null)
                    {
                        var cbe = (ComboBoxEdit)cr;
                        if (cbe.SelectedItem != null)
                        {
                            if (!((ComboBoxItem)cbe.SelectedItem).Value.Trim().Equals(string.Empty))
                            {
                                list.Add(property.Name);
                                property.SetValue(obj, ((ComboBoxItem)(cbe.SelectedItem)).Value, null);
                            }
                        }
                    }
                    break;
                case "DevExpress.XtraEditors.CalcEdit":
                    property = obj.GetType().GetProperties().ToList().Where(item => item.Name + "_CalcEdit" == cr.Name).FirstOrDefault();
                    if (property != null)
                    {
                        property.SetValue(obj, ((CalcEdit)cr).Value, null);
                    }
                    break;
                case "DevExpress.XtraEditors.DateEdit":
                    property = obj.GetType().GetProperties().ToList().Where(item => item.Name + "_DateEdit" == cr.Name).FirstOrDefault();
                    if (property != null)
                    {
                        property.SetValue(obj, ((DateEdit)cr).DateTime, null);
                    }
                    break;
                case "DevExpress.XtraEditors.CheckEdit":
                    property = obj.GetType().GetProperties().ToList().Where(item => item.Name + "_CheckEdit" == cr.Name).FirstOrDefault();
                    if (property != null)
                    {
                        property.SetValue(obj, ((CheckEdit)cr).Checked, null);
                    }
                    break;
                case "DevExpress.XtraEditors.PictureEdit":
                    property = obj.GetType().GetProperties().ToList().Where(item => item.Name + "_CheckEdit" == cr.Name).FirstOrDefault();
                    if (property != null)
                    {
                        if (((PictureEdit)cr).Image != null)
                        {
                            property.SetValue(obj, CovnertImageToByte(((PictureEdit)cr).Image), null);
                        }
                    }
                    break;
            }
        }

        #endregion

        #region 递归设置控件ReadOnly属性

        /// <summary>
        /// 设置输入框的状态
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="state"></param>
        /// <param name="mark"></param>
        public static void setControlState(Control parent, bool state, string mark)
        {
            foreach (Control c in parent.Controls)
            {
                setControlState(c, state, mark);
                setState(c, state, mark);
            }
        }


        /// <summary>
        /// 设置控件状态ReadOnly
        /// </summary>
        /// <param name="cr"></param>
        /// <param name="state"></param>
        /// <param name="mark"></param>
        public static void setState(Control cr, bool state, string mark)
        {
            switch (cr.ToString())
            {
                case "DevExpress.XtraEditors.TextEdit":
                    if ((cr).Name.IndexOf(mark) != -1)
                    {
                        ((TextEdit)cr).Properties.ReadOnly = state;
                    }
                    break;
                case "DevExpress.XtraEditors.MemoEdit":
                    if ((cr).Name.IndexOf(mark) != -1)
                    {
                        ((MemoEdit)cr).Properties.ReadOnly = state;
                    }
                    break;
                case "DevExpress.XtraEditors.ComboBoxEdit":
                    if ((cr).Name.IndexOf(mark) != -1)
                    {
                        ((ComboBoxEdit)cr).Properties.ReadOnly = state;
                    }
                    break;
                case "DevExpress.XtraEditors.CalcEdit":
                    if ((cr).Name.IndexOf(mark) != -1)
                    {
                        ((CalcEdit)cr).Properties.ReadOnly = state;
                    }
                    break;
                case "DevExpress.XtraEditors.CheckEdit":
                    if ((cr).Name.IndexOf(mark) != -1)
                    {
                        ((CheckEdit)cr).Properties.ReadOnly = state;
                    }
                    break;
                case "DevExpress.XtraEditors.DateEdit":
                    if ((cr).Name.IndexOf(mark) != -1)
                    {
                        ((DateEdit)cr).Properties.ReadOnly = state;
                    }
                    break;
                case "DevExpress.XtraEditors.PictureEdit":
                    if ((cr).Name.IndexOf(mark) != -1)
                    {
                        ((PictureEdit)cr).Properties.ReadOnly = state;
                    }
                    break;
                case "DevExpress.XtraEditors.ListBoxControl":
                    if ((cr).Name.IndexOf(mark) != -1)
                    {
                        (cr).Enabled = !state;
                    }
                    break;
                case "DevExpress.XtraEditors.SimpleButton":
                    if ((cr).Name.IndexOf(mark) != -1)
                    {
                        (cr).Enabled = !state;
                    }
                    break;
            }
        }

        #endregion

        #region 遍历父容器根据datatable设置控件值，非递归版本

        /// <summary>
        ///  遍历父控件容器子控件，根据datatable设置相应值
        /// </summary>
        /// <param name="parent">父控件</param>
        /// <param name="dt">数据项</param>
        /// <param name="mark">要设置的子控件的name标识</param>
        public static void setControlValue(Control parent, DataTable dt, string mark)
        {
            string value = "";

            foreach (Control cr in parent.Controls)
            {
                switch (cr.ToString())
                {
                    case "DevExpress.XtraEditors.TextEdit":
                        if (dt.Columns.IndexOf((cr).Name.Replace(mark, "")) != -1)
                        {
                            value = dt.Rows[0][(cr).Name.Replace(mark, "")].ToString();
                            (cr).Text = value;
                        }
                        break;
                    case "DevExpress.XtraEditors.MemoEdit":
                        if (dt.Columns.IndexOf((cr).Name.Replace(mark, "")) != -1)
                        {
                            value = dt.Rows[0][(cr).Name.Replace(mark, "")].ToString();
                            (cr).Text = value;
                        }
                        break;
                    case "DevExpress.XtraEditors.ComboBoxEdit":
                        if (dt.Columns.IndexOf((cr).Name.Replace(mark, "")) != -1)
                        {
                            value = dt.Rows[0][(cr).Name.Replace(mark, "")].ToString();
                            (cr).Text = value;
                        }
                        break;
                    case "DevExpress.XtraEditors.CalcEdit":
                        if (dt.Columns.IndexOf((cr).Name.Replace(mark, "")) != -1)
                        {
                            value = dt.Rows[0][(cr).Name.Replace(mark, "")].ToString();
                            (cr).Text = value;
                        }
                        break;
                    case "DevExpress.XtraEditors.DateEdit":
                        if (dt.Columns.IndexOf((cr).Name.Replace(mark, "")) != -1)
                        {
                            value = dt.Rows[0][(cr).Name.Replace(mark, "")].ToString();
                            (cr).Text = value;
                        }
                        break;
                    case "DevExpress.XtraEditors.CheckEdit":
                        if (dt.Columns.IndexOf((cr).Name.Replace(mark, "")) != -1)
                        {
                            ((CheckEdit)cr).Checked = (dt.Rows[0][(cr).Name.Replace(mark, "")].ToString() != "0");
                        }
                        break;
                    case "DevExpress.XtraEditors.PictureEdit":
                        if (dt.Columns.IndexOf((cr).Name.Replace(mark, "")) != -1)
                        {
                            if (dt.Rows[0][(cr).Name.Replace(mark, "")] != DBNull.Value)
                            {
                                ((PictureEdit)cr).Image =
                                    Image.FromStream(new MemoryStream((byte[])dt.Rows[0][(cr).Name.Replace(mark, "")]));
                            }
                        }
                        break;
                }
            }
        }

        #endregion

        #region 遍历父容器根据datatable递归设置控件值

        /// <summary>
        /// 设置控件值递归调用
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="dt"></param>
        /// <param name="mark"></param>
        /// <param name="isRecursion"></param>
        public static void setControlValueRecursion(Control parent, DataTable dt, string mark)
        {
            foreach (Control c in parent.Controls)
            {
                setControlValueRecursion(c, dt, mark);
                setValue(c, dt, mark);
            }
        }

        /// <summary>
        /// 设置控件值
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="dt"></param>
        /// <param name="mark"></param>
        public static void setValue(Control cr, DataTable dt, string mark)
        {
            //如果为空则返回
            if (dt.Rows.Count == 0)
            {
                return;
            }
            string value = "";


            switch (cr.ToString())
            {
                case "DevExpress.XtraEditors.TextEdit":
                    if (dt.Columns.IndexOf((cr).Name.Replace(mark, "")) != -1)
                    {
                        value = dt.Rows[0][(cr).Name.Replace(mark, "")].ToString();
                        (cr).Text = value;
                    }
                    break;
                case "DevExpress.XtraEditors.MemoEdit":
                    if (dt.Columns.IndexOf((cr).Name.Replace(mark, "")) != -1)
                    {
                        value = dt.Rows[0][(cr).Name.Replace(mark, "")].ToString();
                        (cr).Text = value;
                    }
                    break;
                case "DevExpress.XtraEditors.ComboBoxEdit":
                    if (dt.Columns.IndexOf((cr).Name.Replace(mark, "")) != -1)
                    {
                        value = dt.Rows[0][(cr).Name.Replace(mark, "")].ToString();
                        //((DevExpress.XtraEditors.ComboBoxEdit)cr).Text = value;
                        int i = 0;
                        foreach (ComboBoxItem item in ((ComboBoxEdit)cr).Properties.Items)
                        {
                            if (item.Value == value)
                            {
                                ((ComboBoxEdit)cr).SelectedIndex = i;
                                break;
                            }
                            i++;
                        }
                    }
                    break;
                case "DevExpress.XtraEditors.CalcEdit":
                    if (dt.Columns.IndexOf((cr).Name.Replace(mark, "")) != -1)
                    {
                        value = dt.Rows[0][(cr).Name.Replace(mark, "")].ToString();
                        (cr).Text = value;
                    }
                    break;
                case "DevExpress.XtraEditors.DateEdit":
                    if (dt.Columns.IndexOf((cr).Name.Replace(mark, "")) != -1)
                    {
                        value = dt.Rows[0][(cr).Name.Replace(mark, "")].ToString();
                        (cr).Text = value;
                    }
                    break;
                case "DevExpress.XtraEditors.CheckEdit":
                    if (dt.Columns.IndexOf((cr).Name.Replace(mark, "")) != -1)
                    {
                        ((CheckEdit)cr).Checked = (dt.Rows[0][(cr).Name.Replace(mark, "")].ToString() != "0");
                    }
                    break;
                case "DevExpress.XtraEditors.PictureEdit":
                    if (dt.Columns.IndexOf((cr).Name.Replace(mark, "")) != -1)
                    {
                        if (dt.Rows[0][(cr).Name.Replace(mark, "")] != DBNull.Value)
                        {
                            ((PictureEdit)cr).Image =
                                Image.FromStream(new MemoryStream((byte[])dt.Rows[0][(cr).Name.Replace(mark, "")]));
                        }
                    }
                    break;
            }
        }

        #endregion

        #region 递归读取父控件容器子控件值设置datatable

        /// <summary>
        /// 获取窗体中的值
        /// </summary>
        /// <param name="cr"></param>
        /// <param name="dt"></param>
        /// <param name="mark"></param>
        public static void getControlValueRecursion(Control parent, ref DataTable dt, string mark)
        {
            foreach (Control c in parent.Controls)
            {
                getControlValueRecursion(c, ref dt, mark);
                getValue(c, ref dt, mark);
            }
        }

        /// <summary>
        /// 为更新datatable 获取窗口控件的值
        /// </summary>
        /// <param name="cr"></param>
        /// <param name="dt"></param>
        /// <param name="mark"></param>
        public static void getValue(Control cr, ref DataTable dt, string mark)
        {
            string value = "";
            switch (cr.ToString())
            {
                case "DevExpress.XtraEditors.TextEdit":
                    if (dt.Columns.IndexOf((cr).Name.Replace(mark, "")) != -1)
                    {
                        value = (cr).Text;
                        dt.Rows[0][(cr).Name.Replace(mark, "")] = getDBNULL(value);
                    }
                    break;
                case "DevExpress.XtraEditors.MemoEdit":
                    if (dt.Columns.IndexOf((cr).Name.Replace(mark, "")) != -1)
                    {
                        value = (cr).Text;
                        dt.Rows[0][(cr).Name.Replace(mark, "")] = getDBNULL(value);
                    }
                    break;
                case "DevExpress.XtraEditors.ComboBoxEdit":
                    if (dt.Columns.IndexOf((cr).Name.Replace(mark, "")) != -1)
                    {
                        // value = ((DevExpress.XtraEditors.ComboBoxEdit)cr).Text;
                        var cbe = (ComboBoxEdit)cr;
                        if (cbe.SelectedItem != null)
                        {
                            value = ((ComboBoxItem)(cbe.SelectedItem)).Value;
                            dt.Rows[0][(cr).Name.Replace(mark, "")] = getDBNULL(value);
                            if ((cr).Tag != null && (cr).Tag.ToString().Trim() != string.Empty)
                                dt.Rows[0][(cr).Tag.ToString().Trim()] = ((ComboBoxItem)(cbe.SelectedItem)).Text;
                        }
                    }
                    break;
                case "DevExpress.XtraEditors.CalcEdit":
                    if (dt.Columns.IndexOf((cr).Name.Replace(mark, "")) != -1)
                    {
                        value = (cr).Text;
                        dt.Rows[0][(cr).Name.Replace(mark, "")] = getDBNULL(value);
                    }
                    break;
                case "DevExpress.XtraEditors.DateEdit":
                    if (dt.Columns.IndexOf((cr).Name.Replace(mark, "")) != -1)
                    {
                        value = (cr).Text;
                        dt.Rows[0][(cr).Name.Replace(mark, "")] = getDBNULL(value);
                    }
                    break;
                case "DevExpress.XtraEditors.CheckEdit":
                    if (dt.Columns.IndexOf((cr).Name.Replace(mark, "")) != -1)
                    {
                        dt.Rows[0][(cr).Name.Replace(mark, "")] = ((CheckEdit)cr).Checked;
                    }
                    break;
                case "DevExpress.XtraEditors.PictureEdit":
                    if (dt.Columns.IndexOf((cr).Name.Replace(mark, "")) != -1)
                    {
                        if (((PictureEdit)cr).Image != null)
                        {
                            dt.Rows[0][(cr).Name.Replace(mark, "")] = CovnertImageToByte(((PictureEdit)cr).Image);
                        }
                    }
                    break;
            }
        }

        #endregion

        #region 递归设置父控件容器子控件要绑定的字段

        /// <summary>
        /// 递归设置某一控件子控件要绑定的字段
        /// </summary>
        /// <param name="parent">父容器</param>
        /// <param name="dt">绑定数据源</param>
        /// <param name="mark">控件标记</param>
        public static void setControlBindFieldRecursion(Control parent, DataTable dt, string mark)
        {
            foreach (Control c in parent.Controls)
            {
                setControlBindFieldRecursion(c, dt, mark);
                setControlBindField(c, dt, mark);
            }
        }

        public static void setControlBindField(Control cr, DataTable dt, string mark)
        {
            switch (cr.ToString())
            {
                case "DevExpress.XtraEditors.TextEdit":
                    if (dt.Columns.IndexOf((cr).Name.Replace(mark, "")) != -1)
                    {
                        (cr).DataBindings.Add("EditValue", dt, (cr).Name.Replace(mark, ""));
                    }
                    break;
                case "DevExpress.XtraEditors.MemoEdit":
                    if (dt.Columns.IndexOf((cr).Name.Replace(mark, "")) != -1)
                    {
                        (cr).DataBindings.Add("EditValue", dt, (cr).Name.Replace(mark, ""));
                    }
                    break;
                case "DevExpress.XtraEditors.ComboBoxEdit":
                    if (dt.Columns.IndexOf((cr).Name.Replace(mark, "")) != -1)
                    {
                        (cr).DataBindings.Add("EditValue", dt, (cr).Name.Replace(mark, ""));
                    }
                    break;
                case "DevExpress.XtraEditors.CalcEdit":
                    if (dt.Columns.IndexOf((cr).Name.Replace(mark, "")) != -1)
                    {
                        (cr).DataBindings.Add("EditValue", dt, (cr).Name.Replace(mark, ""));
                    }
                    break;
                case "DevExpress.XtraEditors.DateEdit":
                    if (dt.Columns.IndexOf((cr).Name.Replace(mark, "")) != -1)
                    {
                        (cr).DataBindings.Add("EditValue", dt, (cr).Name.Replace(mark, ""));
                    }
                    break;
                case "DevExpress.XtraEditors.CheckEdit":
                    if (dt.Columns.IndexOf((cr).Name.Replace(mark, "")) != -1)
                    {
                        (cr).DataBindings.Add("EditValue", dt, (cr).Name.Replace(mark, ""));
                    }
                    break;
                case "DevExpress.XtraEditors.PictureEdit":
                    if (dt.Columns.IndexOf((cr).Name.Replace(mark, "")) != -1)
                    {
                        (cr).DataBindings.Add("EditValue", dt, (cr).Name.Replace(mark, ""));
                    }
                    break;
            }
        }

        #endregion

        #region 递归设置父控件容器子控件值验证事件

        public static void ValidationMoreThanZero(Control parent, DXValidationProvider dxValidationProvider1)
        {
            var rangeValidationRule = new ConditionValidatonRule();
            rangeValidationRule.ConditionOperator = ConditionOperator.GreaterOrEqual;
            rangeValidationRule.Value1 = 0;
            rangeValidationRule.ErrorText = "请输入一个大于或等于0的数值";
            SetControlValidationRecursion(parent, "DevExpress.XtraEditors.CalcEdit", dxValidationProvider1,
                                          rangeValidationRule);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cr"></param>
        /// <param name="dt"></param>
        /// <param name="mark"></param>
        public static void SetControlValidationRecursion(Control parent, string mark,
                                                         DXValidationProvider dxValidationProvider1,
                                                         ValidationRuleBase validationRule1)
        {
            foreach (Control c in parent.Controls)
            {
                SetControlValidationRecursion(c, mark, dxValidationProvider1, validationRule1);
                setControlValidation(c, mark, dxValidationProvider1, validationRule1);
            }
        }

        /// <summary>
        /// 为cr设置验证
        /// </summary>
        /// <param name="cr"></param>
        /// <param name="dt"></param>
        /// <param name="mark"></param>
        public static void setControlValidation(Control cr, string mark, DXValidationProvider dxValidationProvider1,
                                                ValidationRuleBase validationRule1)
        {
            if (cr.ToString() == mark)
            {
                if (cr.Text != "")
                {
                    dxValidationProvider1.SetValidationRule(cr, validationRule1);
                }
                else
                {
                    dxValidationProvider1.SetValidationRule(cr, null);
                }
            }
        }

        #endregion

        #region 递归设置控件属性,委托使用

        #region Delegates

        public delegate void SetPropertiesMethod(Control cr);

        #endregion

        public static void setControlPropertiesRecursion(Control parent, string mark, SetPropertiesMethod method)
        {
            foreach (Control c in parent.Controls)
            {
                setControlPropertiesRecursion(c, mark, method);
                setControlProperties(c, mark, method);
            }
        }

        /// <summary>
        ///\
        /// </summary>
        /// <param name="cr"></param>
        /// <param name="dt"></param>
        /// <param name="mark"></param>
        public static void setControlProperties(Control cr, string mark, SetPropertiesMethod method)
        {
            if (cr.ToString() == mark)
            {
                method.Invoke(cr);
            }
        }

        public static void SetCalcNull(Control cr)
        {
            if (cr is CalcEdit)
            {
                (cr as CalcEdit).Properties.AllowNullInput = DefaultBoolean.True;
                //(cr as DevExpress.XtraEditors.CalcEdit).CausesValidation = false;

                var myToolTipController = new ToolTipController();
                myToolTipController.ShowBeak = true;
                myToolTipController.Rounded = true;
                myToolTipController.ShowShadow = true;
                myToolTipController.ReshowDelay = 0;
                myToolTipController.InitialDelay = 0;
                myToolTipController.AutoPopDelay = 10000;

                (cr as CalcEdit).ToolTipTitle = "操作提示";
                (cr as CalcEdit).ToolTipIconType = ToolTipIconType.Information;
                (cr as CalcEdit).ToolTip = "Ctrl + Del清除输入内容!";
                (cr as CalcEdit).ToolTipController = myToolTipController;
            }
        }

        #endregion

        #region 公共方法

        private static Byte[] CovnertImageToByte(Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Jpeg);
                ms.Position = 0;
                var imageBytes = new byte[ms.Length];
                ms.Read(imageBytes, 0, imageBytes.Length);
                return imageBytes;
            }
        }

        public static bool checkIsModified(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                checkIsModified(c);

                if (checkModified(c))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool checkModified(Control c)
        {
            if (c is BaseEdit)
            {
                return (c as BaseEdit).IsModified;
            }
            return false;
        }


        /// <summary>
        /// 数据如果是空，设置为DBNull
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static object getDBNULL(string s)
        {
            if (s == "")
                return DBNull.Value;
            else
                return s;
        }

        public static ComboBoxItem[] getObjectByList(Dictionary<string, string> objectlist)
        {
            var objects = new ComboBoxItem[objectlist.Count];
            int i = 0;
            foreach (var o in objectlist)
            {
                var myo = new ComboBoxItem();
                myo.Value = o.Key;
                myo.Text = o.Value;
                objects[i++] = myo;
            }
            return objects;
        }

        public static DataTable getBoolList()
        {
            var dt = new DataTable();
            dt.Columns.Add("value");
            dt.Columns.Add("text");
            DataRow dr = dt.NewRow();
            dr["value"] = 1;
            dr["text"] = "是";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["value"] = 0;
            dr["text"] = "否";
            dt.Rows.Add(dr);
            return dt;
        }

        /// <summary>
        /// 返回一个gridview的datatable副本
        /// </summary>
        /// <param name="gv"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static DataTable ChageToCopy(GridView gv, string key)
        {
            //原本想使用dtSource将其中datarow调用SetAdded设置为增加状态 ，调用底层da更新
            //却引发“只能在具有 Unchanged DataRowState 的 DataRows 上调用 SetAdded 和 SetModified 。”异常
            //因此此处构建一个副本。
            DataTable dtSource = ((DataView)gv.DataSource).Table;
            var dtTarget = new DataTable();
            if (dtSource != null)
            {
                dtTarget = dtSource.Clone();
                dtTarget.TableName = dtSource.TableName;
                foreach (DataRow drSource in dtSource.Rows)
                {
                    DataRow drTarget = dtTarget.NewRow();

                    for (int i = 0; i < dtSource.Columns.Count; i++)
                    {
                        drTarget[i] = drSource[i];
                    }
                    //覆盖主键
                    drTarget[key] = Guid.NewGuid().ToString();

                    dtTarget.Rows.Add(drTarget);
                }
            }
            return dtTarget;
        }


        public static DataTable ChageToCopy(DataTable dtSource, string key)
        {
            //原本想使用dtSource将其中datarow调用SetAdded设置为增加状态 ，调用底层da更新
            //却引发“只能在具有 Unchanged DataRowState 的 DataRows 上调用 SetAdded 和 SetModified 。”异常
            //因此此处构建一个副本。
            var dtTarget = new DataTable();
            if (dtSource != null)
            {
                dtTarget = dtSource.Clone();
                dtTarget.TableName = dtSource.TableName;
                foreach (DataRow drSource in dtSource.Rows)
                {
                    DataRow drTarget = dtTarget.NewRow();

                    for (int i = 0; i < dtSource.Columns.Count; i++)
                    {
                        drTarget[i] = drSource[i];
                    }
                    //覆盖主键
                    if (key != "")
                        drTarget[key] = Guid.NewGuid().ToString();

                    dtTarget.Rows.Add(drTarget);
                }
            }
            return dtTarget;
        }

        public static void SelectComboxByValue(ComboBoxEdit combox, string value)
        {
            int i = 0;
            foreach (ComboBoxItem item in combox.Properties.Items)
            {
                if (item.Value == value)
                {
                    combox.SelectedIndex = i;
                    break;
                }
                i++;
            }
        }

        /// <summary>
        /// 下拉列表对象，如listBoxControl1.Items.Add(new Common.ComboBoxItem(dr["name"].ToString(), dr["code_pollute"].ToString()));
        /// </summary>
        public class ComboBoxItem
        {
            public string Text;
            public string Value;

            public ComboBoxItem()
                : this("", null)
            {
            }

            public ComboBoxItem(string text, string value)
            {
                Text = text;
                Value = value;
            }

            public override string ToString()
            {
                return Text;
            }
        }

        #endregion

        #region Color & HEX

        // 方法1  
        public static string RGB_2_HEX(byte R, byte G, byte B)
        {
            return "#" +
                   String.Format("{0:X}", Color.FromArgb(R, G, B).ToArgb()).Substring(2);
        }

        // 方法2  
        public static string RGB2HEX(byte R, byte G, byte B)
        {
            return "#" +
                   Convert.ToString(Color.FromArgb(R, G, B).ToArgb(), 16).Substring(2);
        }

        public static string RGB_HEX(int colorInt)
        {
            return "#" +
                   Convert.ToString(colorInt, 16).Substring(2);
        }


        public static Color HexToColor(string hexColor)
        {
            //Remove # if present

            if (hexColor.IndexOf('#') != -1)
                hexColor = hexColor.Replace("#", "");
            int red = 0;
            int green = 0;
            int blue = 0;
            if (hexColor.Length == 6)
            {
                //#RRGGBB
                red = int.Parse(hexColor.Substring(0, 2), NumberStyles.AllowHexSpecifier);
                green = int.Parse(hexColor.Substring(2, 2), NumberStyles.AllowHexSpecifier);
                blue = int.Parse(hexColor.Substring(4, 2), NumberStyles.AllowHexSpecifier);
            }
            else if (hexColor.Length == 3)
            {
                //#RGB
                red = int.Parse(hexColor[0] + hexColor[0].ToString(), NumberStyles.AllowHexSpecifier);
                green = int.Parse(hexColor[1] + hexColor[1].ToString(), NumberStyles.AllowHexSpecifier);
                blue = int.Parse(hexColor[2] + hexColor[2].ToString(), NumberStyles.AllowHexSpecifier);
            }

            return Color.FromArgb(red, green, blue);
        }

        #endregion
    }
}