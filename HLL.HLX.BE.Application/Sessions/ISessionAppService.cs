using System.Threading.Tasks;
using Abp.Application.Services;
using HLL.HLX.BE.Sessions.Dto;

namespace HLL.HLX.BE.Application.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}