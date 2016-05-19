using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace HLL.HLX.BE.Application.Common.Dto
{
    public class ListItemDto
    {
        public string Text { get; set; }

        public string Value { get; set; }
    }


    public class SelectListItemDto : ListItemDto
    {
        public bool Selected { get; set; }        
    }
}
