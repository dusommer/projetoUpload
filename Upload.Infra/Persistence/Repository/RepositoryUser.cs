using System.Linq;
using ToDo.Domain.Arguments.User;
using Upload.Domain.Arguments.User;
using Upload.Domain.Entities;
using Upload.Domain.Interfaces.Repositories;
using Upload.Infra.Persistence.Repository.Base;

namespace Upload.Infra.Persistence.Repository
{
    public class RepositoryUser : RepositoryBase<User, int>, IRepositoryUser
    {
        protected readonly UploadContext _context;

        public RepositoryUser(UploadContext context) : base(context)
        {
            _context = context;
        }

        public bool EmailExists(string email)
        {
            return _context.Users.Any(x => x.Email.Equals(email));
        }

        public User GetByLoginAndPass(LoginUserRequest request)
        {
            return _context.Users.Where(x => x.Email.Equals(request.Email) && x.Password.Equals(request.Password)).FirstOrDefault();
        }
    }
}
