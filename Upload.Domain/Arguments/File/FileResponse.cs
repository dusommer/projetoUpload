using Upload.Domain.Interfaces.Arguments;
using System;

namespace ToDo.Domain.Arguments.File
{
    public class FileResponse : IResponse
    {
        public static explicit operator FileResponse(Upload.Domain.Entities.File file)
        {
            if (file == null)
            {
                return null;
            }

            return new FileResponse()
            {
                Name = file.Name,
                Size = file.Size,
                IdUser = file.IdUser,
                CreatedDate = file.CreatedDate,
                NomeInterno = file.NomeInterno
            };
        }

        public Guid NomeInterno { get; set; }
        public string Name { get; set; }
        public decimal Size { get; set; }
        public int IdUser { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
