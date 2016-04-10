using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLL.HLX.BE.Common.Util
{
    public static  class AppSettingUtil
    {
        public static string GetAppSetting(string key, string defaultValue)
        {
            var strValue = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(strValue))
            {
                return defaultValue;
            }

            return strValue;
        }

        public static int GetAppSetting4Int(string key, int defaultValue)
        {
            var strValue = ConfigurationManager.AppSettings[key];
            int value;
            if (int.TryParse(strValue, out value))
            {
                return value;
            }

            return defaultValue;
        }

        public static uint GetAppSetting4Uint(string key, uint defaultValue)
        {
            var strValue = ConfigurationManager.AppSettings[key];
            uint value;
            if (UInt32.TryParse(strValue, out value))
            {
                return value;
            }

            return defaultValue;
        }

        public static bool GetAppSetting4Bool(string key, bool defaultValue = false)
        {
            var strValue = ConfigurationManager.AppSettings[key];
            var value = defaultValue;
            if (bool.TryParse(strValue, out value))
            {
                return value;
            }

            return defaultValue;
        }
    }
}
