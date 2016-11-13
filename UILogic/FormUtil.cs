using System;
using System.Windows.Forms;

namespace UILogic
{
    public static class FormUtil
    {
        /// <summary>
        /// 获取文件绝对路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetPath(string path)
        {
            string strPath = "";
            if (path.IndexOf(":\\") > 0)
            {
                strPath = path;
            }
            else
            {
                if (!path.StartsWith("\\"))
                {
                    path = "\\" + path;
                }
                strPath = Application.StartupPath + path;
            }
            return strPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public static void RestartApp(Action action = null)
        {
            try
            {
                if (action != null)
                    action();
                Application.Restart();
            }
            catch
            {
                Environment.Exit(0);
            }
        }
    }
}