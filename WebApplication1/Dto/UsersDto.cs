using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Dto
{
    public class UsersDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string UserType { get; set; }
    }
}