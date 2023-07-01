using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Dto
{
    public class NewSummaryDto
    {
        public int Summary_Num { get; set; }
        public string WrittenBy { get; set; }
        public string Content { get; set; }
        public string ImportanttoNote { get; set; }
        public System.DateTime Summary_Date { get; set; }
        public int Treatment_Id { get; set; }
        public string Email;
        public string WrittenById { get; set; }


    }
}