using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.Application.Mobility.Users.Dto
{
    [AutoMapTo(typeof (UserAvatar))]
    public class UpdateUserAvatarInput : IInputDto, ICustomValidate, IShouldNormalize
    {
        [Required]
        public long? UserId { get; set; }

        //[Required]
        public string ImageBase64 { get; set; }

        /// <summary>
        ///     图片名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     自定义验证input
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