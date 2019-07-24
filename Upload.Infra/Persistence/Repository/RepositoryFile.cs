using Upload.Domain.Entities;
using Upload.Domain.Interfaces.Repositories;
using Upload.Infra.Persistence.Repository.Base;

namespace Upload.Infra.Persistence.Repository
{
    public class RepositoryFile : RepositoryBase<File, int>, IRepositoryFile
    {
        protected readonly UploadContext _context;

        public RepositoryFile(UploadContext context) : base(context)
        {
            _context = context;
        }
    }
}
