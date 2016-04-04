using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using HLL.HLX.BE.Core.Model.Videos;

namespace HLL.HLX.BE.Application.Mobility.Videos.Dto
{
    [AutoMapTo(typeof(Video))]
    public class PublishVideoDto
    {
        /// <summary>
        /// 视频标题
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// 视频流媒体地址
        /// </summary>
        public string StreamMediaPath { get; set; }

        /// <summary>
        /// 预计直播开始时间
        /// </summary>
        [Required]
        public DateTime? EstimatedStartTime { get; set; }
    

    }
}
