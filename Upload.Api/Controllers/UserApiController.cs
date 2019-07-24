using Upload.Api.Controllers.Base;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Upload.Domain.Interfaces.Services;
using Upload.Domain.Arguments.User;
using Upload.Infra.Transaction;
using ToDo.Domain.Arguments.User;

namespace Upload.Api.Controllers
{
    [RoutePrefix("api/user")]
    public class UserApiController : BaseController
    {
        private readonly IServiceUser _serviceUser;

        public UserApiController(IUnitOfWork unitOfWork, IServiceUser serviceUser) : base(unitOfWork)
        {
            _serviceUser = serviceUser;
        }

        [Route("Insert")]
        [HttpPost]
        public async Task<HttpResponseMessage> Insert(InsertUserRequest request)
        {
            try
            {
                var response = _serviceUser.InsertUser(request);

                return await ResponseAsync(response, _serviceUser);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }

        [Route("GetByLoginAndPass")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetByLoginAndPass(LoginUserRequest request)
        {
            try
            {
                var response = _serviceUser.GetByLoginAndPass(request);

                return await ResponseAsync(response, _serviceUser);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }

        [Route("EmailExists")]
        [HttpGet]
        public async Task<HttpResponseMessage> EmailExists(string request)
        {
            try
            {
                var response = _serviceUser.EmailExists(request);

                return await ResponseAsync(response, _serviceUser);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }
    }
}