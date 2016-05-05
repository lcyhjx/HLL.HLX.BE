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

    //101 - Video

    public enum VideoStatus
    {
       /// <summary>
       /// 预告
       /// </summary>
        Unstarted = 1010101,
        /// <summary>
        /// 直播中
        /// </summary>
        Started = 1010102,
        /// <summary>
        /// 已结束
        /// </summary>
        Ended = 1010103
    }


    public enum UserLiveRommStatus
    {
        /// <summary>
        /// 在房间中
        /// </summary>
        In = 1010201,
        /// <summary>
        /// 已退出房间
        /// </summary>
        Out = 1010202
    }
}