using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using HLL.HLX.BE.Application.Common.Dto;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.Application.Mobility.Users.Dto
{
    [AutoMapTo(typeof(User))]
    public class UpdateUserInput : BaseInput
    {
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 公司职务
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        ///  个性签名
        /// </summary>
        public string Signature { get; set; }

    }
}
