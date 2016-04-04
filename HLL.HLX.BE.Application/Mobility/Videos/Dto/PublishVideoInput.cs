using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLL.HLX.BE.Application.Common.Dto;

namespace HLL.HLX.BE.Application.Mobility.Videos.Dto
{
    public class PublishVideoInput :BaseInput
    {
        [Required]
        public PublishVideoDto Video { get; set; }

        public override void AddValidationErrors(List<ValidationResult> results)
        {
            
            base.AddValidationErrors(results);

            if (string.IsNullOrEmpty(Video.Title))
            {
                 results.Add(new ValidationResult(string.Format("{0}不能为空","Video.Title")));
            }

            if (Video.EstimatedStartTime== null || Video.EstimatedStartTime.Value == DateTime.MinValue)
            {
                results.Add(new ValidationResult(string.Format("{0}不能为空", "Video.EstimatedStartTime")));
            }
        }
    }
}
