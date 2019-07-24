using Upload.Domain.Interfaces.Arguments;

namespace Upload.Domain.Arguments.User
{
    public class UserResponse : IResponse
    {
        public static explicit operator UserResponse(Entities.User user)
        {
            if (user == null)
            {
                return null;
            }

            return new UserResponse()
            {
                Id = user.Id,
                Email = user.Email,
                Password = user.Password
            };
        }
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
