using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Dto
{
    public class FilesDto
    {
        public int file_num { get; set; }
        public DateTime date_sent { get; set; }
        public int file_type_num { get; set; }
        public byte[] content { get; set; }
    }
}