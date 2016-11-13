using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    public static class ListExtension
    {
        public static string GetJoinListStr(this List<string> list, string flag)
        {
            var rStr = new StringBuilder();
            var n = 0;
            list.ForEach(item =>
                             {
                                 rStr = ++n != list.Count ? rStr.Append(item).Append(flag) : rStr.Append(item);
                             });
            return rStr.ToString();
        }
    }
}
