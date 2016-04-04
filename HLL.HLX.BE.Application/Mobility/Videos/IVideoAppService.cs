using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using HLL.HLX.BE.Application.Mobility.Videos.Dto;

namespace HLL.HLX.BE.Application.Mobility.Videos
{
    public interface IVideoAppService : IApplicationService
    {
        /// <summary>
        ///  获取正在直播或预告中的视频
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
         GetLiveAndUnstartedVideoOutput GetLiveAndUnstartedVideo(GetLiveAndUnstartedVideoInput input);

        /// <summary>
        /// 获取录播的视频
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        GetEndedVideoOutput GetEndedVideo(GetEndedVideoInput input);


        /// <summary>
        /// 发布视频
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PublishVideoOutput PublishVideo(PublishVideoInput input);

        /// <summary>
        /// 删除视频
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        DeleteVideoOutput DeleteVideo(DeleteVideoInput input);

    }
}
