using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Upload.Api.Controllers.Base;
using Upload.Domain.Arguments.File;
using Upload.Domain.Interfaces.Services;
using Upload.Infra.Transaction;

namespace Upload.Api.Controllers
{
    [RoutePrefix("api/file")]
    public class FileApiController : BaseController
    {
        private readonly IServiceFile _serviceFile;

        public FileApiController(IUnitOfWork unitOfWork, IServiceFile serviceFile) : base(unitOfWork)
        {
            _serviceFile = serviceFile;
        }

        [Route("Insert")]
        [HttpPost]
        public async Task<HttpResponseMessage> Insert(InsertFileRequest request)
        {
            try
            {
                var response = _serviceFile.InsertFile(request);

                return await ResponseAsync(response, _serviceFile);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }
    }
}