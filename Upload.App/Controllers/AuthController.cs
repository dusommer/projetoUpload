using Newtonsoft.Json;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using ToDo.Domain.Arguments.User;
using Upload.App.Models;
using Upload.App.Utils;
using Upload.Domain.Arguments.User;

namespace Upload.App.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        [HttpGet]
        public ActionResult Login(string returnUrl, string message)
        {
            ViewBag.MessageUserCreate = message;
            var model = new LoginModel
            {
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var request = new LoginUserRequest()
            {
                Email = model.Email,
                Password = Utils.Utils.PassGenerate(model.Password)
            };

            var userLogin = JsonConvert.DeserializeObject<UserResponse>
                (ServiceApiUtil.ApiResponse("api/user/GetByLoginAndPass", "POST", request));
            if (userLogin != null)
            {
                var identify = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userLogin.Email),
                    new Claim(ClaimTypes.Sid, userLogin.Id.ToString())
                },
                "ApplicationCookie");
                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;
                authManager.SignIn(identify);
                return Redirect(GetRedirectUrl(model.ReturnUrl));
            }

            ModelState.AddModelError("", "Usuário ou senha inválidos");
            return View(model);
        }

        [HttpGet]
        public ActionResult LogOut()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("index", "upload");
        }

        public ActionResult Create()
        {
            return View(new UserModel());
        }
        [HttpPost]
        public ActionResult Create(UserModel user)
        {
            if (!ModelState.IsValid)
            {
                return View("Create");
            }

            var emailExists = JsonConvert.DeserializeObject<bool>
                (ServiceApiUtil.ApiResponse("api/user/EmailExists?request=" + user.Email, "GET"));

            if (emailExists)
            {
                ModelState.AddModelError("", "E-mail já cadastrado.");
                return View(user);
            }
            var request = new InsertUserRequest()
            {
                Email = user.Email,
                Password = Utils.Utils.PassGenerate(user.Password)
            };

            var response = JsonConvert.DeserializeObject<InsertUserResponse>
                (ServiceApiUtil.ApiResponse("api/user/Insert", "POST", request));

            return RedirectToAction("Login", new { returnUrl = "", message = "Usuário criado com sucesso." });
        }

        private string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("index", "upload");
            }

            return returnUrl;
        }
    }
}