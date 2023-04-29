using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Dto
{
    public class UsersDto
    {
        public string email { get; set; }
        public string password { get; set; }
        public string phone_number { get; set; }
        public string user_type { get; set; }
    }
}