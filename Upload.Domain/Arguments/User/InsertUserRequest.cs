using Upload.Domain.Interfaces.Arguments;

namespace Upload.Domain.Arguments.User
{
    public class InsertUserRequest : IRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
