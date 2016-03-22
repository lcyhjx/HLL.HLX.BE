using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;

namespace HLL.HLX.BE.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : HlxBeControllerBase
    {
        public ActionResult Index()
        {
            return View("~/App/Main/views/layout/layout.cshtml"); //Layout of the angular application.
        }
	}
}