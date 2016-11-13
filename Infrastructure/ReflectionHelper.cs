using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Infrastructure
{
    public class ReflectionHelper
    {
        /// <summary>
        /// 返回T的类型 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string TypeOfT<T>()
        {
            Type t = typeof(T);
            return t.ToString();
        }
        /// <summary>
        /// 通过实例返回T的类型 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string TypeOfEntity<T>(T name)
        {
            Type t = name.GetType();//实例
            return t.ToString();
        }
        /// <summary>
        /// 通过实例返回当前程序集路径
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string AssemblyLocationOfEntity<T>(T name)
        {
            Type t = name.GetType();
            Assembly asm = t.Assembly;
            return asm.Location;//当前路径
        }
        /// <summary>
        /// 通过实例返回当前所有类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string AssemblyOfTypes<T>(T name)
        {
            Type t = name.GetType();
            Assembly asm = t.Assembly;
            StringBuilder sb = new StringBuilder();
            Type[] types = asm.GetTypes();
            foreach (Type item in types)
            {
                sb.Append(item);
                sb.Append("</br>");
            }
            return sb.ToString();
        }
        /// <summary>
        /// 通过实例返回当前程序集所有模块信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string AssemblyModules<T>(T name)
        {
            Type t = name.GetType();
            Assembly asm = t.Assembly;
            StringBuilder sb = new StringBuilder();
            Module[] types = asm.GetModules();
            foreach (Module item in types)
            {
                sb.Append(item.Name);
                sb.Append("</br>");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 通过实例返回当前程序集所有字段信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string FieldsOfEntity<T>(T name)
        {
            Type t = name.GetType();
            StringBuilder sb = new StringBuilder();
            FieldInfo[] field = t.GetFields();
            foreach (FieldInfo item in field)
            {
                sb.Append("name:" + item.Name);
                sb.Append("  type:" + item.FieldType);
                sb.Append("  Attributes:" + item.Attributes);
                sb.Append("</br>");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 通过实例返回当前程序集所有属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetPropertiesOfT<T>()
        {
            Type t = typeof(T);
            StringBuilder sb = new StringBuilder();
            PropertyInfo[] field = t.GetProperties();
            foreach (PropertyInfo item in field)
            {
                sb.Append("    name:" + item.Name);
                sb.Append("    type:" + item.PropertyType);
                sb.Append("    Attributes:" + item.Attributes);
                sb.Append("    IsRead:" + item.CanRead);
                sb.Append("    IsWrite:" + item.CanWrite);
                sb.Append("</br>");
            }
            return sb.ToString();
        }

        /// <summary>
        ///返回T的方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetMethodInfoOfT<T>()
        {
            Type t = typeof(T);
            StringBuilder sb = new StringBuilder();
            MethodInfo[] methods = t.GetMethods();
            foreach (MethodInfo item in methods)
            {
                sb.Append("    name:" + item.Name);
                sb.Append("    Attributes:" + item.Attributes);
                sb.Append("    返回类型:" + item.ReturnType);
                sb.Append("</br>");
            }
            return sb.ToString();
        }

        /// <summary>
        ///返回T的事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetEventInfoOfT<T>()
        {
            Type t = typeof(T);
            StringBuilder sb = new StringBuilder();
            EventInfo[] methods = t.GetEvents();
            foreach (EventInfo item in methods)
            {
                sb.Append("name:" + item.Name);
                sb.Append("  ;         DeclaringType:" + item.DeclaringType);
                sb.Append("</br>");
            }
            return sb.ToString();
        }

        /// <summary>
        ///返回T的所有构造函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetConstructorInfoOfT<T>()
        {
            Type t = typeof(T);
            StringBuilder sb = new StringBuilder();
            ConstructorInfo[] cons = t.GetConstructors();
            foreach (ConstructorInfo item in cons)
            {
                sb.Append("构造函数:" + item.Name);
                sb.Append("属性:" + item.Attributes);
                sb.Append("</br>");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static object GetObjFromAssemblyByName<T>(string typeName)
        {
            Assembly asm = typeof(T).Assembly;
            return asm.CreateInstance(typeName);
        }
    }
}
