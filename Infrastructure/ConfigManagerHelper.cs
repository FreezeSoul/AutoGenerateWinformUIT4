using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace Infrastructure
{
    public class ConfigManagerHelper
    {

        #region Public Methods

        /// <summary>
        /// 读取配置项字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetStringAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// 读取配置项整型，失败返回-1
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetIntAppSetting(string key)
        {
            int rValue;
            var value = ConfigurationManager.AppSettings[key];
            if (int.TryParse(value, out rValue))
            {
                return rValue;
            }
            return -1;
        }

        #endregion

    }
}
