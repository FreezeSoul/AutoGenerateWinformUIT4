using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace Infrastructure
{
    public static class MyDataTableExtension
    {
        public static string DataTableToXml(this DataTable dt)
        {
            if (dt.TableName == "") dt.TableName = "Table1";
            var sw = new StringWriter();
            XmlWriter xw = XmlWriter.Create(sw);
            dt.WriteXml(xw, XmlWriteMode.WriteSchema);
            string rValue = sw.ToString();
            xw.Close();
            return rValue;
        }

        public static DataTable XmlToDataTable(this DataTable dt, string xmlStr)
        {
            if (dt.TableName == "") dt.TableName = "Table1";
            var sr = new StringReader(xmlStr);
            XmlReader xr = XmlReader.Create(sr);
            dt.ReadXml(xr);
            xr.Close();
            return dt;
        }

        public static void ChangeData(this DataTable dt, DataTable newDt)
        {
            try
            {
                dt.Clear();
                for (int i = 0; i < newDt.Rows.Count; i++)
                {
                    var row = dt.NewRow();
                    row.ItemArray = newDt.Rows[i].ItemArray;
                    dt.Rows.Add(row);
                }
                dt.AcceptChanges();
            }
            catch { }
        }

        public static void SetColumnValue(this DataRow dr, string name, object value)
        {
            if (dr.Table.Columns.Contains(name))
            {
                dr[name] = value;
            }
        }

        public static DataTable ConvertToDataTable<T>(IEnumerable<T> list)
        {
            var dt = new DataTable { TableName = typeof(T).Name };
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            properties.ToList().ForEach(item =>
                                            {
                                                if (!item.PropertyType.IsByRef)
                                                {
                                                    Type columnType = item.PropertyType;
                                                    if (item.PropertyType.IsGenericType &&
                                                        item.PropertyType.GetGenericTypeDefinition() ==
                                                        typeof(Nullable<>))
                                                    {
                                                        columnType = item.PropertyType.GetGenericArguments()[0];
                                                    }
                                                    dt.Columns.Add(item.Name).DataType = columnType;
                                                }
                                            });
            foreach (T obj in list)
            {
                var dr = dt.NewRow();
                properties.ToList().ForEach(item =>
                                                {
                                                    if (!item.PropertyType.IsByRef)
                                                        dr[item.Name] = item.GetValue(obj, null);
                                                });
                dt.Rows.Add(dr);
            }
            return dt;
        }

        //注意关于Nullable泛型的处理
        //* 用 PropertyType.IsGenericType 决定property是否是generic类型
        //* 用 ProprtyType.GetGenericTypeDefinition() == typeof(Nullable<>) 检测它是否是一个nullable类型
        //* 用 PropertyType.GetGenericArguments() 获取基类型。
        public static DataTable ToDataTable(IList list)
        {
            var result = new DataTable();

            var propertys = list.AsQueryable().ElementType.GetProperties();

            foreach (var pi in propertys)
            {
                var columnType = pi.PropertyType;

                if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    columnType = pi.PropertyType.GetGenericArguments()[0];
                }
                result.Columns.Add(pi.Name, columnType);
            }

            for (int i = 0; i < list.Count; i++)
            {
                var tempList = new ArrayList();
                foreach (PropertyInfo pi in propertys)
                {
                    object obj = pi.GetValue(list[i], null);
                    tempList.Add(obj);
                }
                object[] array = tempList.ToArray();
                result.LoadDataRow(array, true);
            }
            return result;
        }

        //注意下面方法没有修改支持Nullable
        public static DataTable ToDataTable<T>(IList<T> list)
        {
            return ToDataTable(list, null);
        }

        public static DataTable ToDataTable<T>(IList<T> list, params string[] propertyName)
        {
            var propertyNameList = new List<string>();
            if (propertyName != null)
                propertyNameList.AddRange(propertyName);

            var result = new DataTable();
            var propertys = list.AsQueryable().ElementType.GetProperties();
            foreach (var pi in propertys)
            {
                if (propertyNameList.Count == 0)
                {
                    result.Columns.Add(pi.Name, pi.PropertyType);
                }
                else
                {
                    if (propertyNameList.Contains(pi.Name))
                        result.Columns.Add(pi.Name, pi.PropertyType);
                }
            }

            for (int i = 0; i < list.Count; i++)
            {
                var tempList = new ArrayList();
                foreach (var pi in propertys)
                {
                    if (propertyNameList.Count == 0)
                    {
                        var obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    else
                    {
                        if (propertyNameList.Contains(pi.Name))
                        {
                            object obj = pi.GetValue(list[i], null);
                            tempList.Add(obj);
                        }
                    }
                }
                var array = tempList.ToArray();
                result.LoadDataRow(array, true);
            }
            return result;
        }
    }
}