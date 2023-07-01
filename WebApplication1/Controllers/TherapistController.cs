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
    public class TherapistController : ApiController
    {
        [HttpGet]
        [Route("api/Therapist/")]
        public List<TherapistDto> Get(string email)
        {
            {
                SafePlaceDbContextt db = new SafePlaceDbContextt();
                string phone = db.TblUsers.Where(o => o.Email == email).Select(p => p.PhoneNumber).FirstOrDefault();
                string id = db.TblTherapist.Where(o => o.PhoneNumber == phone).Select(p => p.Therapist_Id).FirstOrDefault();
                List<TherapistDto> listMeeting = db.TblTreats.Where(a => a.Therapist_Id == id && a.TblTreatment.Treatment_Date == DateTime.Today && a.TblTreatment.Room_Num!=3 )
                .Select(x => new TherapistDto
                {
                    Therapist_Id = x.Therapist_Id,
                    FirstName = x.TblTherapist.FirstName,
                    LastName = x.TblTherapist.LastName,
                    Treatment_Date = x.TblTreatment.Treatment_Date,
                    StartTime = x.TblTreatment.StartTime,
                    EndTime = x.TblTreatment.EndTime,
                    Room_Num = (int)x.TblTreatment.Room_Num,
                    WasDone = x.TblTreatment.WasDone,
                    PatientFirstName = x.TblPatient.FirstName,
                    PatientLastName = x.TblPatient.LastName,
                    Treatment_Id = x.TblTreatment.Treatment_Id
                }).ToList();

                if (listMeeting.Any())
                {
                    return listMeeting;
                }
                else
                {
                    return null;
                }
            }
        }

        [HttpGet]
        [Route("api/Therapistpreviou")]
        public List<TherapistDto> Getprevmeeting(string email)
        {
            {
                SafePlaceDbContextt db = new SafePlaceDbContextt();
                string phone = db.TblUsers.Where(o => o.Email == email).Select(p => p.PhoneNumber).FirstOrDefault();
                string id = db.TblTherapist.Where(o => o.PhoneNumber == phone).Select(p => p.Therapist_Id).FirstOrDefault();

                DateTime currentHour = DateTime.Now.Date.AddHours(DateTime.Now.Hour); // Get the current hour
                DateTime threeDaysAgo = currentHour.AddDays(-4);

                List<TherapistDto> listMeeting = db.TblTreats.Where(a => a.Therapist_Id == id && 
                a.TblTreatment.StartTime >= threeDaysAgo && a.TblTreatment.StartTime <= currentHour &&
                a.TblTreatment.Room_Num!=3)
                 .Select(x => new TherapistDto
                {
                    Therapist_Id = x.Therapist_Id,
                    FirstName = x.TblTherapist.FirstName,
                    LastName = x.TblTherapist.LastName,
                    Treatment_Date = x.TblTreatment.Treatment_Date,
                    StartTime = x.TblTreatment.StartTime,
                    EndTime = x.TblTreatment.EndTime,
                    Room_Num = (int)x.TblTreatment.Room_Num,
                    WasDone = x.TblTreatment.WasDone,
                    PatientFirstName = x.TblPatient.FirstName,
                    PatientLastName = x.TblPatient.LastName,
                    Treatment_Id = x.TblTreatment.Treatment_Id
                }).ToList();



                if (listMeeting.Any())
                {
                    return listMeeting;
                }
                else
                {
                    return null;
                }
            }
        }

        [HttpPost]
        [Route("api/Daysoff")]
        public IHttpActionResult Daysoff([FromBody] TherapistDto value)
        {
            SafePlaceDbContextt db = new SafePlaceDbContextt();
            var therEmail = value.Email;
            var therId = db.TblTherapist.Where(o => o.Email == therEmail).Select(p => p.Therapist_Id).FirstOrDefault().ToString();
            var futurearr = value.Free;
            int request_Id = db.TblDaysoff.Any() ? db.TblDaysoff.Max(s => s.Request_Id) + 1 : 1;
            

            List<TblDaysoff> freedays = new List<TblDaysoff>();
            try
            {
                foreach(var i in futurearr)
                {
                    if (DateTime.TryParseExact(i, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                        freedays.Add(new TblDaysoff
                        {
                            Request_Id = request_Id,
                            Therapist_Id = therId,
                            Email = therEmail,
                            Dayoff = date
                        });
                }

                db.TblDaysoff.AddRange(freedays);

                db.SaveChanges();
                return Ok("Save");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
