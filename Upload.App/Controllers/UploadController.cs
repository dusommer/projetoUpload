using System.Web.Mvc;

namespace Upload.App.Controllers
{
    [Authorize]
    public class UploadController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }
    }
}