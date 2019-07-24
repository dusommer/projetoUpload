using Upload.Domain.Arguments.User;
using Upload.Domain.Entities;
using Upload.Domain.Interfaces.Repositories;
using Upload.Domain.Interfaces.Services;
using prmToolkit.NotificationPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Domain.Arguments.User;

namespace Upload.Domain.Services
{
    public class ServiceUser : Notifiable, IServiceUser
    {

        private readonly IRepositoryUser _repositoryUser;

        public ServiceUser()
        {

        }

        public ServiceUser(IRepositoryUser repositoryUser)
        {
            _repositoryUser = repositoryUser;
        }

        public bool EmailExists(string request)
        {
            return _repositoryUser.EmailExists(request);
        }

        public UserResponse GetByLoginAndPass(LoginUserRequest request)
        {
            if (request == null)
            {
                AddNotification("LoginUserRequest", "Request is required.");
            }

            if (IsInvalid())
            {
                return null;
            }
            User user = _repositoryUser.GetByLoginAndPass(request);

            return (UserResponse)user;
        }

        public InsertUserResponse InsertUser(InsertUserRequest request)
        {
            User user = null;

            if (request == null)
            {
                AddNotification("InsertFileRequest", "Request is required.");
            }

            if (_repositoryUser.EmailExists(request.Email))
            {
                AddNotification("Email exists", "Esse e-mail já está cadastrado");
            }
            else
            {
                user = new User(request?.Email, request?.Password);
                AddNotifications(user);
            }


            if (IsInvalid())
            {
                return null;
            }

            user = _repositoryUser.Insert(user);

            return (InsertUserResponse)user;
        }
    }
}
