using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLL.HLX.BE.Application.Common.Dto
{
    public class BasePageInput : BaseInput, IPageInputDto
    {

        public int? PageIndex { get; set; }

        public int? PageSize { get; set; }

        public override void Normalize()
        {
            base.Normalize();

            if (PageIndex == null)
            {
                PageIndex = 0;
            }
            if (PageSize == null)
            {
                PageSize = int.MaxValue;
            }
        }
    }
}
