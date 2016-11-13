using System.IO;
using Infrastructure;

namespace UILogic
{
    public class SkinINIManager
    {
        private const string _SkinSection = "SkinSetting";
        private const string _SkinKey = "SkinName";
        private string _iniPath;

        public string IniPath
        {
            get
            {
                if (_iniPath == null)
                    _iniPath = FormUtil.GetPath(ConfigManagerHelper.GetStringAppSetting("SkinINI"));

                return _iniPath;
            }
        }

        public string ReadSkin()
        {
            string SkinStr = "";
            if (File.Exists(IniPath))
            {
                var inifile = new INIFileHelper(IniPath);
                SkinStr = inifile.IniReadValue(_SkinSection, _SkinKey);
            }
            return SkinStr;
        }

        public void WriteSkin(string skinValue)
        {
            if (!File.Exists(IniPath))
            {
                File.Create(IniPath);
            }
            var inifile = new INIFileHelper(IniPath);
            inifile.IniWriteValue(_SkinSection, _SkinKey, skinValue);
        }
    }
}