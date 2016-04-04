using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLL.HLX.BE.Application.Common.Dto;

namespace HLL.HLX.BE.Application.Mobility.Videos.Dto
{
    public class DeleteVideoInput : BaseInput
    {
        [Required]
        public long? Id { get; set; }
    }
}