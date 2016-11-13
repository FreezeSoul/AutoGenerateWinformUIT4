using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace ArcGisComponent.Common
{
    public class SerializerHelper
    {
        public static string Serialiaze<T>(T obj)
        {
            var xs = new XmlSerializer(typeof(T));
            var sb = new StringBuilder();
            var writer = new StringWriter(sb);
            xs.Serialize(writer, obj);
            var str = sb.ToString();
            writer.Close();
            return str;
        }

        public static T Deserialize<T>(String str)
        {
            var xs = new XmlSerializer(typeof(T));
            var reader = new StringReader(str);
            T obj = (T)xs.Deserialize(reader);
            reader.Close();
            return obj;
        }
    }
}
