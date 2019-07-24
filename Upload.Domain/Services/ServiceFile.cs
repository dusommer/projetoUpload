using Upload.Domain.Arguments.File;
using Upload.Domain.Entities;
using Upload.Domain.Interfaces.Repositories;
using Upload.Domain.Interfaces.Services;
using prmToolkit.NotificationPattern;

namespace Upload.Domain.Services
{
    public class ServiceFile : Notifiable, IServiceFile
    {

        private readonly IRepositoryFile _repositoryFile;

        public ServiceFile()
        {

        }

        public ServiceFile(IRepositoryFile repositoryFile)
        {
            _repositoryFile = repositoryFile;
        }

        public InsertFileResponse InsertFile(InsertFileRequest request)
        {
            File file = null;

            if (request == null)
            {
                AddNotification("InsertFileRequest", "Request is required.");
            }
            else
            {
                file = new File(request.Name, request.Size, request.IdUser, request.CreatedDate);
                file = _repositoryFile.Insert(file);
            }

            AddNotifications(file);

            if (IsInvalid())
            {
                return null;
            }

            return (InsertFileResponse)file;
        }
    }
}
