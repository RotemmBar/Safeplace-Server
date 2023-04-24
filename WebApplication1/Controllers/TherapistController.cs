using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1;
using DATA;
using WebApplication1.Dto;

namespace WebApplication1.Controllers
{
    public class TherapistController : ApiController
    {
        [HttpGet]
        [Route("api/Therapist/{id}")]
        public List<TherapistDto> Get(string id)
        {
            SafePlaceDBContext db = new SafePlaceDBContext();
            List<TherapistDto> listMeeting = db.TblTreats.Where(a => a.Therapist_Id == id && a.TblTreatment.TreatmentDate == DateTime.Today)
            .Select(x => new TherapistDto
            {
                Therapist_Id = x.Therapist_Id,
                FirstName = x.TblTherapist.FirstName,
                LastName = x.TblTherapist.LastName,
                Treatment_Date = x.TblTreatment.TreatmentDate,
                StartTime = x.TblTreatment.StartTime,
                EndTime = x.TblTreatment.EndTime,
                Room_Num = x.TblTreatment.Room_Num,
                WasDone = x.TblTreatment.WasDone,
                PatientFirstName = x.TblUsers.TblPatient.FirstName,
                PatientLastName = x.TblUsers.TblPatient.LastName,
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


        // POST: api/Therapist
        [HttpPost]
        [Route("api/PostSummary")]
        public IHttpActionResult Post([FromBody] TblSummary value)
        {
            SafePlaceDBContext db = new SafePlaceDBContext();
            try
            {
                TblSummary newSummary = new TblSummary();
                newSummary.Summary_Num = value.Summary_Num;
                newSummary.WrittenBy = value.WrittenBy;
                newSummary.Content = value.Content;
                newSummary.Summary_Date = value.Summary_Date;
                newSummary.ImportanttoNote = value.ImportanttoNote;
                newSummary.TblTreatment = new List<TblTreatment>();
                db.TblSummary.Add(newSummary);
                db.SaveChanges();
                return Ok("Save");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //// GET: api/Patient/5
        //public string Get(int id)
        //{
        //    return "value";
        //}


        //// PUT: api/Therapist/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Therapist/5
        //public void Delete(int id)
        //{
        //}
    }
}
