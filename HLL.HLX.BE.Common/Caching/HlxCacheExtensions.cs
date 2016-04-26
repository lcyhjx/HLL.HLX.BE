using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Runtime.Caching;

namespace HLL.HLX.BE.Common.Caching
{
    public static class HlxCacheExtensions
    {
        //public static void RemoveByPattern(this ICache cacheManager, string pattern)
        //{
        //    var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        //    var keysToRemove = new List<String>();

        //    cacheManager.
        //    foreach (var item in Cache)
        //        if (regex.IsMatch(item.Key))
        //            keysToRemove.Add(item.Key);

        //    foreach (string key in keysToRemove)
        //    {
        //        Remove(key);
        //    }
        //}
    }
}
