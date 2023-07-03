using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Dto
{
    public class TherapistDto
    {
        public string Therapist_Id;
        public string FirstName;
        public string LastName;
        public Nullable<System.DateTime> Treatment_Date;
        public Nullable<System.DateTime> StartTime;
        public Nullable<System.DateTime> EndTime;
        public string WasDone;
        public int Room_Num;
        public string PatientFirstName;
        public string PatientLastName;
        public int Treatment_Id;
        public string Email;
        public string[] Free;



        public string Gender { get; set; }
        public System.DateTime BirthDate { get; set; }
        public System.DateTime StartDate { get; set; }
        public int YearsOfExperience { get; set; }


        public int Age;
        public int NumTreatments;




    }

    public class DayoffDto
    {

        public Nullable<System.DateTime> Dayoff { get; set; }

    }
}