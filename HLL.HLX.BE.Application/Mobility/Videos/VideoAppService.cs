using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using AutoMapper;
using HLL.HLX.BE.Application.Mobility.Videos.Dto;
using HLL.HLX.BE.Core.Business.Videos;
using HLL.HLX.BE.Core.Model;
using HLL.HLX.BE.Core.Model.Videos;

namespace HLL.HLX.BE.Application.Mobility.Videos
{
    public class VideoAppService : HlxBeAppServiceBase, IVideoAppService
    {
        private readonly VideoDomainService _videoDomainService;

        public VideoAppService(VideoDomainService videoDomainService)
        {
            _videoDomainService = videoDomainService;
        }

        /// <summary>
        ///  获取正在直播或预告中的视频
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public GetLiveAndUnstartedVideoOutput GetLiveAndUnstartedVideo(GetLiveAndUnstartedVideoInput input)
        {
            var videos = _videoDomainService.GetLiveAndUnstartedVideo();        
            var videoDtos = Mapper.Map<List<VideoDto>>(videos);
            return new GetLiveAndUnstartedVideoOutput
            {
                Videos = videoDtos,
            };
        }

        /// <summary>
        /// 获取录播的视频
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public GetEndedVideoOutput GetEndedVideo(GetEndedVideoInput input)
        {
            var videos = _videoDomainService.GetEndedVideo();
            var videoDtos = Mapper.Map<List<VideoDto>>(videos);
            return new GetEndedVideoOutput
            {
                Videos = videoDtos,
            };              
        }

        /// <summary>
        /// 发布视频
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public PublishVideoOutput PublishVideo(PublishVideoInput input)
        {

            var video = Mapper.Map<Video>(input.Video);

            video.Status = VideoStatus.Unstarted;
            video.PublishUserId = AbpSession.GetUserId();
            video.limelightCount = 0;

           long id =  _videoDomainService.PublishVideo(video);

            return new PublishVideoOutput
            {
                Id =  id
            };

        }

        /// <summary>
        /// 删除视频
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public DeleteVideoOutput DeleteVideo(DeleteVideoInput input)
        {
            _videoDomainService.DeleteVideo(input.Id.GetValueOrDefault());
            return new DeleteVideoOutput();
        }

    }
}
