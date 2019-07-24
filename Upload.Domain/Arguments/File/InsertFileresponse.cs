using Upload.Domain.Interfaces.Arguments;

namespace Upload.Domain.Arguments.File
{
    public class InsertFileResponse : IResponse
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public static explicit operator InsertFileResponse(Entities.File file)
        {
            return new InsertFileResponse()
            {
                Id = file.Id,
                Message = "File inserted successfully."
            };
        }
    }
}
