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
            SafePlaceDbContextt db = new SafePlaceDbContextt();

            List<TreatmentDto> treatment = db.TblTreatment.Where(o => o.TblTreats.Any(y => y.Patient_Id == id)).Where(c => c.Treatment_Date > DateTime.Today).
                Select(p => new TreatmentDto()
                {
                    Treatment_Id = p.Treatment_Id,
                    WasDone = p.WasDone,
                    Type_Id =(int) p.Type_Id,
                    Room_Num =(int) p.Room_Num,
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
            SafePlaceDbContextt db = new SafePlaceDbContextt();
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
                SafePlaceDbContextt db = new SafePlaceDbContextt();
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


    }
}
