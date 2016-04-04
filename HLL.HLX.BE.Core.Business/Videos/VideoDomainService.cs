using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Services;
using HLL.HLX.BE.Core.Model;
using HLL.HLX.BE.Core.Model.Videos;

namespace HLL.HLX.BE.Core.Business.Videos
{
    /// <summary>
    /// 视频相关的业务类
    /// </summary>
    public class VideoDomainService : DomainService
    {
        private readonly IVideoRepository _videoRepository;


        public VideoDomainService(IVideoRepository videoRepository)
        {
            _videoRepository = videoRepository;
        }


        /// <summary>
        /// 查找正在直播或预告中的视频
        /// </summary>
        /// <returns></returns>
        public List<Video> GetLiveAndUnstartedVideo()
        {
            var query =
                _videoRepository.GetAll()
                    .Where(x => x.Status == VideoStatus.Unstarted || x.Status == VideoStatus.Started);
            return query.ToList();
        }

        /// <summary>
        /// 获取录播的视频
        /// </summary>
        /// <returns></returns>
        public List<Video> GetEndedVideo()
        {
            var query =
                _videoRepository.GetAll()
                    .Where(x => x.Status == VideoStatus.Ended);
            return query.ToList();
        }

        /// <summary>
        /// 发布视频
        /// </summary>
        /// <param name="video"></param>
        /// <returns></returns>
        public long PublishVideo(Video video)
        {

            long id = _videoRepository.InsertAndGetId(video);
            return id;
        }


        /// <summary>
        ///删除视频
        /// </summary>
        /// <param name="id"></param>
        public void DeleteVideo(long id)
        {
            _videoRepository.Delete(id);
        }
    }
}