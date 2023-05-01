﻿using System;
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
            SafePlaceDbContextt db = new SafePlaceDbContextt();
            List<TherapistDto> listMeeting = db.TblTreats.Where(a => a.Therapist_Id == id && a.TblTreatment.Treatment_Date == DateTime.Today)
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


        // POST: api/Therapist
        [HttpPost]
        [Route("api/PostSummary")]
        public IHttpActionResult Post([FromBody] NewSummaryDto value)
        {
            
            SafePlaceDbContextt db = new SafePlaceDbContextt();
            try
            {
                TblSummary newSummary = new TblSummary();
                int nextSummaryNum = db.TblSummaries.Any() ? db.TblSummaries.Max(s => s.Summary_Num) + 1 : 1;
                newSummary.Summary_Num = nextSummaryNum;
                newSummary.WrittenBy = value.WrittenBy;
                newSummary.Content = value.Content;
                newSummary.Summary_Date = value.Summary_Date;
                newSummary.ImportentToNote = value.ImportanttoNote;
                //newSummary.TblTreatments = new List<TblTreatment>();
                db.TblSummaries.Add(newSummary);

                TblWrittenFor newWrittenFor = new TblWrittenFor();
                newWrittenFor.Summary_Num = nextSummaryNum;
                newWrittenFor.Treatment_Id = value.Treatment_Id;
                db.TblWrittenFors.Add(newWrittenFor);

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
