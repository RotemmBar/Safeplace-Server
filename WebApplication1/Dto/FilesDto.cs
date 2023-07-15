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
        public string content { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }


    }
}