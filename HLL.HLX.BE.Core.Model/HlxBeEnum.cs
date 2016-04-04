using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLL.HLX.BE.Core.Model
{
    /*
     * 所有枚举的值为7位数字，并且值不重复
     * 示例000XXNN
     * 000- 前三位表示某一类枚举
     * XX- 中间两位表示一类枚举中的不同枚举对象
     * NN- 后两位表示一个枚举对象的不同枚举值
     * 1010101,1010102
     * 1010201,1010201
     */
    //class HlxBeEnum
    //{
    //}

    public enum VideoStatus
    {
        Unstarted = 1010101,
        Started = 1010102,
        Ended = 1010103
    }

}
