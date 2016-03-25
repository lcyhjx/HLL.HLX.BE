using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;

namespace HLL.HLX.BE.Application.Common.Dto
{
    public class BaseInput : IInputDto, ICustomValidate, IShouldNormalize
    {

        /// <summary>
        /// 自定义验证input
        /// </summary>
        /// <param name="results"></param>

        public virtual void AddValidationErrors(List<ValidationResult> results)
        {

        }

        public virtual void Normalize()
        {

        }
    }
}
