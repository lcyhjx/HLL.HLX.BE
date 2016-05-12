using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLL.HLX.BE.Application.MobilityH5.Orders.Dto
{
    public class CartItemAttributeDto
    {
        public int AttributeId { get; set; }


        /// <summary>
        /// 属性值
        /// 1 属性值已预先定义，存储valueId
        /// 1.1 单选  示例: 4
        /// 1.2 多选  示例: 5,12,8 (Id用逗号分隔)
        /// 2 属性未预先定义, 存储用户输入的值
        /// </summary>
        public string Values { get; set; }
    }
}
