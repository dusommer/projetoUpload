using Upload.Domain.Interfaces.Arguments;
using System;

namespace Upload.Domain.Arguments.File
{
    public class InsertFileRequest : IRequest
    {
        public string Name { get; set; }
        public decimal Size { get; set; }
        public int IdUser { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
