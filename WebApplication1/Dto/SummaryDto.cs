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
        public string Patient_Id { get; set; }
        public string FirstNameP { get; set; }
        public string LastNameP { get; set; }
        public string Content { get; set; }
        public string Summary_Date { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public string ImportanttoNote { get; set; }
        public List<TblTreatment> TblTreatment { get; set; }
        public string WrittenById { get; set; }

    }

}