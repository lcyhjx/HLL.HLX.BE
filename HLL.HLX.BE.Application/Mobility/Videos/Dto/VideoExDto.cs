using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using HLL.HLX.BE.Core.Business.Videos;

namespace HLL.HLX.BE.Application.Mobility.Videos.Dto
{
    [AutoMapFrom(typeof(VideoEx))]
    public class VideoExDto : VideoDto
    {

        /// <summary>
        /// 视频发布人用户名
        /// </summary>
        public string PublishUserName { get; set; }

        /// <summary>
        /// 视频发布人用户头像路径
        /// </summary>
        public string PublishUserAvatarImagePath { get; set; }
    }
}
