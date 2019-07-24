using Upload.Domain.Arguments.File;
using Upload.Domain.Interfaces.Services.Base;

namespace Upload.Domain.Interfaces.Services
{
    public interface IServiceFile : IServiceBase
    {
        InsertFileResponse InsertFile(InsertFileRequest request);
    }
}
