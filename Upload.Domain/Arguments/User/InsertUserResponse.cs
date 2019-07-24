using Upload.Domain.Interfaces.Arguments;

namespace Upload.Domain.Arguments.User
{
    public class InsertUserResponse : IResponse
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public static explicit operator InsertUserResponse(Entities.User user)
        {
            return new InsertUserResponse()
            {
                Id = user.Id,
                Message = "File inserted successfully."
            };
        }
    }
}
