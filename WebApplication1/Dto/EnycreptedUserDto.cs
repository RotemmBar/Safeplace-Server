using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Dto
{
    public class EnycreptedUserDto
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public DateTime birthdate { get; set; }
        public DateTime startdate { get; set; }
        public string gender { get; set; }
        public string user_id { get; set; }
        public int issuperuser { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }
}