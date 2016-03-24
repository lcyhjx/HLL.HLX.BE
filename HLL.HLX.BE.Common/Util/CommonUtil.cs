using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace HLL.HLX.BE.Common.Util
{
    public static class CommonUtil
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="text"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object Deserialize(string text, Type type)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }
            XmlSerializer serializer = new XmlSerializer(type);
            StringReader reader = new StringReader(text);
            return serializer.Deserialize(reader);
        }

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();

            XmlWriterSettings settings = new XmlWriterSettings {OmitXmlDeclaration = true};
            XmlWriter xw = XmlWriter.Create(builder, settings);
            XmlSerializer x = new XmlSerializer(obj.GetType());
            x.Serialize(xw, obj);
            
            return builder.ToString();
        }

        public static int GetRandomInt(int minValue, int maxValue)
        {
            var random = new Random(Guid.NewGuid().GetHashCode());
            return random.Next(minValue, maxValue);
        }

        /// <summary>
        /// 检查是否是一个有效的密码
        /// </summary>
        /// <param name="password">密码字符串</param>
        /// <returns></returns>
        public static bool CheckIsValidPassword(string password)
        {
            //ASCII码值 33 -126的字符
            //英文字母，英文符号，数字
            //不允许有空格，特殊符号(比如制表符，设备控制符，换码等... )
            return password.All(t => t >= 33 && t <= 126);
        }
    }
}
