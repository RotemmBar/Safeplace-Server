using System;
using System.Collections.Generic;
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
        [Route("api/patientstreatment/{id}")]
        public List<TreatmentDto> GetAllPatientTreatments(string id)
        {
            SafePlaceDb db = new SafePlaceDb();

            List<TreatmentDto> treatment = db.TblTreatment.Where(o => o.TblTreats.Any(y => y.Patient_Id == id)).Where(c => c.Treatment_Date > DateTime.Today).
                Select(p => new TreatmentDto()
                {
                    Treatment_Id = p.Treatment_Id,
                    WasDone = p.WasDone,
                    Type_Id = p.Type_Id,
                    Room_Num = p.Room_Num,
                    TreatmentDate = (DateTime)p.Treatment_Date,
                    StartTime = (DateTime)p.StartTime,
                    EndTime = (DateTime)p.EndTime
                }).ToList();

            return treatment;
        }

        [HttpGet]
        [Route("api/patient")]
        public List<PatientDto> Get()
        {
            SafePlaceDb db = new SafePlaceDb();
            List<PatientDto> patients = db.TblPatient.Select(p => new PatientDto()
            {
                patientId = p.Patient_Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                //Email = p., ////Go Over
                Age = DateTime.Now.Year - p.BirthDate.Value.Year,
                NumTreatments = p.TblTreats.Count(),
                phoneNumber = p.PhoneNumber
            }).ToList();

            return patients;
        }

        [HttpGet]
        [Route("api/patient/{therapistId}")]
        public IHttpActionResult GetPatientsByTherapistId(string therapistId)
        {
            try
            {
                SafePlaceDb db = new SafePlaceDb();
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
                SafePlaceDb db = new SafePlaceDb();
                PatientDto patient = db.TblPatient
                    .Where(p => p.Patient_Id == patientId)
                    .Select(p => new PatientDto()
                    {
                        patientId = p.Patient_Id,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        //Email = p.TblUsers.Email, ///GO OVER
                        Age = DateTime.Now.Year - p.BirthDate.Value.Year,
                        NumTreatments = p.TblTreats.Count(),
                        phoneNumber = p.PhoneNumber
                    })
                    .SingleOrDefault();

                return Ok(patient);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }


        //// GET: api/Patient/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/Patient
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Patient/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Patient/5
        //public void Delete(int id)
        //{
        //}
    }
}
