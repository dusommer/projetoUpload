using Upload.Domain.Interfaces.Arguments;

namespace ToDo.Domain.Arguments.User
{
    public class LoginUserRequest : IRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
