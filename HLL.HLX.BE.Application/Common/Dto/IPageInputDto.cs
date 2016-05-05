using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLL.HLX.BE.Application.Common.Dto
{
    public interface IPageInputDto
    {
        int? PageIndex { get; set; }

        int? PageSize { get; set; }
    }
}
