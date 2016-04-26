using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using HLL.HLX.BE.Common.ComponentModel;

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

        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <returns>The converted value.</returns>
        public static T To<T>(object value)
        {
            //return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            return (T)To(value, typeof(T));
        }

        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="destinationType">The type to convert the value to.</param>
        /// <returns>The converted value.</returns>
        public static object To(object value, Type destinationType)
        {
            return To(value, destinationType, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="destinationType">The type to convert the value to.</param>
        /// <param name="culture">Culture</param>
        /// <returns>The converted value.</returns>
        public static object To(object value, Type destinationType, CultureInfo culture)
        {
            if (value != null)
            {
                var sourceType = value.GetType();

                TypeConverter destinationConverter = GetNopCustomTypeConverter(destinationType);
                TypeConverter sourceConverter = GetNopCustomTypeConverter(sourceType);
                if (destinationConverter != null && destinationConverter.CanConvertFrom(value.GetType()))
                    return destinationConverter.ConvertFrom(null, culture, value);
                if (sourceConverter != null && sourceConverter.CanConvertTo(destinationType))
                    return sourceConverter.ConvertTo(null, culture, value, destinationType);
                if (destinationType.IsEnum && value is int)
                    return Enum.ToObject(destinationType, (int)value);
                if (!destinationType.IsInstanceOfType(value))
                    return Convert.ChangeType(value, destinationType, culture);
            }
            return value;
        }

        public static TypeConverter GetNopCustomTypeConverter(Type type)
        {
            //we can't use the following code in order to register our custom type descriptors
            //TypeDescriptor.AddAttributes(typeof(List<int>), new TypeConverterAttribute(typeof(GenericListTypeConverter<int>)));
            //so we do it manually here

            if (type == typeof(List<int>))
                return new GenericListTypeConverter<int>();
            if (type == typeof(List<decimal>))
                return new GenericListTypeConverter<decimal>();
            if (type == typeof(List<string>))
                return new GenericListTypeConverter<string>();
            //if (type == typeof(ShippingOption))
            //    return new ShippingOptionTypeConverter();
            //if (type == typeof(List<ShippingOption>) || type == typeof(IList<ShippingOption>))
            //    return new ShippingOptionListTypeConverter();

            return TypeDescriptor.GetConverter(type);
        }
    }
}
