using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.Application.Mobility.Users.Dto
{
    [AutoMapFrom(typeof (User))]
    public class UserDto : EntityDto<long>
    {
        /// <summary>
        ///     昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        ///     性别
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        ///     公司
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        ///     公司职务
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     手机号
        /// </summary>
        //[Index(IsUnique = true)]
        public string PhoneNumber { get; set; }

        /// <summary>
        ///     个性签名
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        ///     头像图片存储路径
        /// </summary>
        public string AvatarFilePath { get; set; }
    }
}