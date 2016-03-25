using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.Application.Mobility.Users.Dto
{
     [AutoMapFrom(typeof(UserAvatar))]
    public class UserAvatarDto : EntityDto<long>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long? UserId { get; set; }


        /// <summary>
        /// 图片存储路径
        /// </summary>
        public string ImageFilePath { get; set; }


        /// <summary>
        /// 图片名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 描述        
        /// </summary>
        public string Description { get; set; }
    }
}
