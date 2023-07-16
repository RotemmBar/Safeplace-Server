using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DATA;
using WebApplication1.Dto;

namespace WebApplication1.Controllers
{
    public class PatientController : ApiController
    {
        [HttpGet]
        [Route("api/patientstreatment/")]
        public IHttpActionResult GetAllPatientTreatments(string email)
        {
            try
            {
              
                SafePlaceDbContextt db = new SafePlaceDbContextt();
                string id = db.TblPatient.Where(o => o.Email == email).Select(p => p.Patient_Id).FirstOrDefault();
                string FirstName = db.TblTreats.Where(t => t.Patient_Id == id).Select(t => t.TblTherapist.FirstName).FirstOrDefault();
                string LastName = db.TblTreats.Where(t => t.Patient_Id == id).Select(t => t.TblTherapist.LastName).FirstOrDefault();
                string patientfirst = db.TblPatient.Where(o => o.Email == email).Select(p => p.FirstName).FirstOrDefault();
                string patientlast = db.TblPatient.Where(o => o.Email == email).Select(p => p.LastName).FirstOrDefault();


                string TherapistName = FirstName +' '+ LastName;
                string patientname = patientfirst + ' ' + patientlast;




                List<TreatmentDto> treatment = db.TblTreatment.Where(o => o.TblTreats.
                Any(y => y.Patient_Id == id)).Where(c => c.Treatment_Date > DateTime.Today && c.Room_Num!=3 && c.WasDone == "n" || c.WasDone == "N").
                Select(p => new TreatmentDto()
                {
                    Treatment_Id = p.Treatment_Id,
                    WasDone = p.WasDone,
                    Type_Id = (int)p.Type_Id,
                    Room_Num = (int)p.Room_Num,
                    datetemp = p.Treatment_Date.ToString(),
                    startTimetemp = p.StartTime.ToString().Substring(13),
                    endtimetemp = p.EndTime.ToString().Substring(13),
                    TherapistName = TherapistName,
                    PatientName=patientname

                }).ToList();

                return Ok(treatment);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }

        //[HttpGet]
        //[Route("api/patient")]
        //public List<PatientDto> Get()
        //{
        //    SafePlaceDbContextt db = new SafePlaceDbContextt();
        //    List<PatientDto> patients = db.TblPatient.Select(p => new PatientDto()
        //    {
        //        patientId = p.Patient_Id,
        //        FirstName = p.FirstName,
        //        LastName = p.LastName,
        //        //Email = p., ////Go Over
        //        Age = DateTime.Now.Year - p.BirthDate.Value.Year,
        //        NumTreatments = p.TblTreats.Count(),
        //        phoneNumber = p.PhoneNumber
        //    }).ToList();

        //    return patients;
        //}

        [HttpGet]
        [Route("api/getpatient")]
        public IHttpActionResult GetPatientsByTherapistId(string email)
        {
            try
            {
                SafePlaceDbContextt db = new SafePlaceDbContextt();
                var therapistPhone = db.TblUsers.Where(o => o.Email == email).Select(p => p.PhoneNumber).FirstOrDefault();
                var therapistId = db.TblTherapist.Where(n => n.PhoneNumber == therapistPhone).Select(f => f.Therapist_Id).FirstOrDefault();
                List<PatientDto> patients = db.TblPatient
                    .Where(p => p.TblTreats.Any(t => t.Therapist_Id == therapistId))
                    .Select(p => new PatientDto()
                    {
                        patientId = p.Patient_Id,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        TherapistFirstName = p.TblTreats.FirstOrDefault(t => t.Therapist_Id == therapistId).TblTherapist.FirstName,
                        TherapistLastName = p.TblTreats.FirstOrDefault(t => t.Therapist_Id == therapistId).TblTherapist.LastName
                    })
                    .ToList();

                return Ok(patients);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }

        }

        [HttpGet]
        [Route("api/patientCard/{patientId}")]
        public IHttpActionResult GetPatientByPatienttId(string patientId)
        {
            try
            {
                SafePlaceDbContextt db = new SafePlaceDbContextt();

                var therid = db.TblTreats.Where(o => o.Patient_Id == patientId).Select(p => p.Therapist_Id).FirstOrDefault();

                PatientDto patient = db.TblPatient
                    .Where(p => p.Patient_Id == patientId)
                    .Select(p => new PatientDto()
                    {
                        patientId = p.Patient_Id,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        Email = p.Email,
                        Age = DateTime.Now.Year - p.BirthDate.Value.Year,
                        NumTreatments = p.TblTreats.Count(),
                        phoneNumber = p.PhoneNumber,
                        TherapistId=therid
                    })
                    .SingleOrDefault();

                return Ok(patient);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }


        [HttpGet]
        [Route("api/getdayoff")]
        public IHttpActionResult GetPTherapistDayOff(string email)
        {
            try
            {
                SafePlaceDbContextt db = new SafePlaceDbContextt();
                var patientid = db.TblPatient.Where(o => o.Email == email).Select(v => v.Patient_Id).FirstOrDefault();
                string therId = db.TblTreats.Where(p => p.Patient_Id == patientid).Select(r => r.Therapist_Id).FirstOrDefault();
                var theremail = db.TblTherapist.Where(g => g.Therapist_Id == therId).Select(j => j.Email).FirstOrDefault();

                List<DayoffDto> dayoffbyther = db.TblDaysoff.Where(y => y.Email == theremail).
                Select(p => new DayoffDto()
                {
                    Dayoff=p.Dayoff
  
                }).ToList();


                return Ok(dayoffbyther);



            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }

        }

        [HttpGet]
        [Route("api/prevpatientstreatment/")]
        public IHttpActionResult GetAllPrevPatientTreatments(string email)
        {
            try
            {

                SafePlaceDbContextt db = new SafePlaceDbContextt();
                string id = db.TblPatient.Where(o => o.Email == email).Select(p => p.Patient_Id).FirstOrDefault();
                string FirstName = db.TblTreats.Where(t => t.Patient_Id == id).Select(t => t.TblTherapist.FirstName).FirstOrDefault();
                string LastName = db.TblTreats.Where(t => t.Patient_Id == id).Select(t => t.TblTherapist.LastName).FirstOrDefault();

                string TherapistName = FirstName + ' ' + LastName;

                DateTime lastMonth = DateTime.Today.AddMonths(-1);

                List<TreatmentDto> treatment = db.TblTreatment
                    .Where(o => o.TblTreats.Any(y => y.Patient_Id == id))
                    .Where(c => c.Treatment_Date >= lastMonth && c.Treatment_Date < DateTime.Now && c.Room_Num!=3)
                    .Select(p => new TreatmentDto()
                    {
                    Treatment_Id = p.Treatment_Id,
                    WasDone = p.WasDone,
                    Type_Id = (int)p.Type_Id,
                    Room_Num = (int)p.Room_Num,
                    datetemp = p.Treatment_Date.ToString(),
                    startTimetemp = p.StartTime.ToString().Substring(13),
                    endtimetemp = p.EndTime.ToString().Substring(13),
                    TherapistName = TherapistName,
                }).ToList();

                return Ok(treatment);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
