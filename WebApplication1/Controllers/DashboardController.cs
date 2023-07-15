using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1;
using DATA;
using WebApplication1.Dto;
using System.Globalization;

namespace WebApplication1.Controllers
{
    public class DashboardController : ApiController
    {
        [HttpGet]
        [Route("api/DaAllTher")]
        public IHttpActionResult GetAllTherapists()
        {
            {
                SafePlaceDbContextt db = new SafePlaceDbContextt();

                List<TblTherapist> the = db.TblTherapist.ToList();

                var therdic = new Dictionary<string, string>();

                foreach (var i in the)
                {
                    therdic[i.Therapist_Id] = i.FirstName;
                }

                List<TblPatient> patient = db.TblPatient.ToList();
                var patientdic = new Dictionary<string, string>();

                foreach (var i in patient)
                {
                    patientdic[i.Patient_Id] = i.FirstName;
                }


                List<TblTreats> treat = db.TblTreats.ToList();

                var result = new Dictionary<string,HashSet<string>>();

                foreach (var i in treat)
                {
                    var thername = therdic[i.Therapist_Id];
                    var pateintname = patientdic[i.Patient_Id];

                    if (!result.ContainsKey(thername))
                        {
                        result[thername] = new HashSet<string>();
                    }
                    result[thername].Add(pateintname);
                }




                return Ok(result);


            }
        }


        [HttpGet]
        [Route("api/DaTreatmentsperDay")]
        public IHttpActionResult GetTreatmentsPerDay()
        {
            {

                SafePlaceDbContextt db = new SafePlaceDbContextt();

                List<TblTreatment> treatments = db.TblTreatment.ToList();

                var weekday = new Dictionary<string, int>();

                foreach (var treatment in treatments)
                {
                    DateTime treatmentDate = treatment.Treatment_Date;
                    string dayOfWeek = treatmentDate.ToString("dddd", CultureInfo.InvariantCulture);

                    if (weekday.ContainsKey(dayOfWeek))
                    {
                        weekday[dayOfWeek]++;
                    }
                    else
                    {
                        weekday[dayOfWeek] = 1;
                    }
                }

                // Arrange weekdays in the correct order (Sunday to Friday)
                var orderedWeekdays = new List<string> { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
                var result = orderedWeekdays.ToDictionary(day => day, day => weekday.ContainsKey(day) ? weekday[day] : 0);

                return Ok(result);


            }
        }



    }
}
