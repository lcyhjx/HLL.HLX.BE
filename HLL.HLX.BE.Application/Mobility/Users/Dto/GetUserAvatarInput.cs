using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;

namespace HLL.HLX.BE.Application.Mobility.Users.Dto
{
    public class GetUserAvatarInput : IInputDto, ICustomValidate, IShouldNormalize
    {
        /// <summary>
        /// 自定义验证input
        /// </summary>
        /// <param name="results"></param>
        public void AddValidationErrors(List<ValidationResult> results)
        {

        }

        public void Normalize()
        {

        }
    }
}
