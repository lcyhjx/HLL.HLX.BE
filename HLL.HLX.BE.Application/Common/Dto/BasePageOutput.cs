using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using AutoMapper;
using HLL.HLX.BE.Common;

namespace HLL.HLX.BE.Application.Common.Dto
{
    public class BasePageOutput<TDto,TBiz> : IOutputDto
    {

        public List<TDto> Items { get; set; }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

        public bool HasPreviousPage
        {
            get { return (PageIndex > 0); }
        }

        public bool HasNextPage
        {
            get { return (PageIndex + 1 < TotalPages); }
        }


        public BasePageOutput(IPagedList<TBiz> bizItems)
        {
            Items = Mapper.Map<List<TDto>>(bizItems);

            PageIndex = bizItems.PageIndex;
            PageSize = bizItems.PageSize;
            TotalCount = bizItems.TotalCount;
            TotalPages = bizItems.TotalPages;
        }
    }
}
