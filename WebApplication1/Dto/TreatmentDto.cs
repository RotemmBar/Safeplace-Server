﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Dto
{
    public class TreatmentDto
    {
        public int Treatment_Id { get; set; }
        public string WasDone { get; set; }
        public int Type_Id { get; set; }
        public int Room_Num { get; set; }

        public bool recommended;
        public DateTime TreatmentDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string datetemp;
        public string startTimetemp;
        public string endtimetemp;
        public string TherapistName;
        public string Therapist_Id;
        public string Patient_Id;
        public string PatientName;
        public string available;
        public string Patient_Email;

        public string TherapistId { get; set; }
        

    }
}