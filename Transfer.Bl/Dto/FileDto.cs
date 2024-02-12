using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Bl.Dto
{
    public class FileDto
    {
        public Guid Id { get; init; }

        public string Path { get; init; }

        public string Extention { get; init; }

        public long Size { get; init; }

        public string ContentType { get; init; }
    }
}
