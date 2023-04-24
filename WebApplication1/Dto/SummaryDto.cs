using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DATA;

namespace WebApplication1.Dto
{
    public class SummaryDto
    {
        public int Summary_Num { get; set; }
        public string WrittenBy { get; set; }
        public string Content { get; set; }
        public System.DateTime Summary_Date { get; set; }
        public string ImportanttoNote { get; set; }
        public List<TblTreatment> TblTreatment { get; set; }
    }
}