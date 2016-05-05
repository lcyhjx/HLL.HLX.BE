using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using HLL.HLX.BE.Application.Common.Dto;
using HLL.HLX.BE.Common;

namespace HLL.HLX.BE.Application.Mobility.Videos.Dto
{
   
    public class GetEndedVideoOutput<TDto, TBiz> : BasePageOutput<TDto, TBiz>
    {

        public GetEndedVideoOutput(IPagedList<TBiz> bizItems) : base(bizItems)
        {

        }
    }
}
