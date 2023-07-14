using DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using WebApplication1.Dto;

namespace WebApplication1.Controllers
{
    public class SummaryController : ApiController
    {

        //[HttpGet]
        //[Route("api/GetSummaryByDate/{PatientId}/{Date}")]
        //public IHttpActionResult GetSummaryByNum(string PatientId, string date)
        //{
        //    try
        //    {
        //        SafePlaceDbContextt db = new SafePlaceDbContextt();

        //        string therid = db.TblTreats.Where(p => p.Patient_Id == PatientId).Select(o => o.Therapist_Id).FirstOrDefault();

        //        SummaryDto Summary = db.TblSummary.Where(a => a.Summary_Date.ToString().Substring(0, 10) == date && a.WrittenBy == therid
        //        && a.TblWrittenFor.FirstOrDefault().WrittenFor == PatientId).Select(x => new SummaryDto()

        //        {
        //            Summary_Num = x.Summary_Num,
        //            WrittenBy = x.WrittenBy,
        //            Summary_Date = x.Summary_Date.ToString().Substring(0, 10),
        //            ImportanttoNote = x.ImportentToNote,
        //            Content = x.Content,
        //            StartTime = (DateTime)x.TblWrittenFor.FirstOrDefault().TblTreatment.StartTime,
        //            EndTime = (DateTime)x.TblWrittenFor.FirstOrDefault().TblTreatment.EndTime,
        //            FirstNameP = x.TblWrittenFor.FirstOrDefault().TblTreatment.TblTreats.FirstOrDefault().TblPatient.FirstName,
        //            LastNameP = x.TblWrittenFor.FirstOrDefault().TblTreatment.TblTreats.FirstOrDefault().TblPatient.LastName

        //        }).FirstOrDefault();


        //        return Ok(Summary);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(HttpStatusCode.BadRequest, ex);
        //    }
        //}

        [HttpGet]
        [Route("api/GetSummaryByNumber/{num}")]
        public IHttpActionResult SummaryByNumber(string Num)
        {
            try
            {
                SafePlaceDbContextt db = new SafePlaceDbContextt();

                int intnum = int.Parse(Num);

                SummaryDto Summary = db.TblSummary.Where(a => a.Summary_Num == intnum).Select(x=> new SummaryDto
                {
                    Summary_Num = x.Summary_Num,
                    WrittenBy = x.WrittenBy,
                    Summary_Date = x.Summary_Date.ToString().Substring(0, 10),
                    ImportanttoNote = x.ImportentToNote,
                    Content = x.Content,
                    StartTime = (DateTime)x.TblWrittenFor.FirstOrDefault().TblTreatment.StartTime,
                    EndTime = (DateTime)x.TblWrittenFor.FirstOrDefault().TblTreatment.EndTime,
                    FirstNameP = x.TblWrittenFor.FirstOrDefault().TblTreatment.TblTreats.FirstOrDefault().TblPatient.FirstName,
                    LastNameP = x.TblWrittenFor.FirstOrDefault().TblTreatment.TblTreats.FirstOrDefault().TblPatient.LastName

                }).FirstOrDefault();


                return Ok(Summary);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpGet]
        [Route("api/GetAllSummary/{PatientId}")]
        public IHttpActionResult GetAllSummary(string PatientId)
        {
            try
            {
                SafePlaceDbContextt db = new SafePlaceDbContextt();
                var therid = db.TblTreats.Where(p => p.Patient_Id == PatientId).Select(o => o.Therapist_Id).FirstOrDefault();
                List<SummaryDto> allSummaries = db.TblSummary.Where(x => x.TblWrittenFor.FirstOrDefault().WrittenFor == PatientId && x.WrittenById == therid)
                 .Select(s => new SummaryDto()
                    {
                        Summary_Num = s.Summary_Num,
                        Summary_Date = s.Summary_Date.ToString().Substring(0, 10),
                        Patient_Id = s.TblWrittenFor.FirstOrDefault().TblTreatment.TblTreats.FirstOrDefault().Patient_Id

                    }).ToList();

                return Ok(allSummaries);

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPost]
        [Route("api/PostSummary")]
        public IHttpActionResult Post([FromBody] NewSummaryDto value)
        {
            SafePlaceDbContextt db = new SafePlaceDbContextt();
            try
            {
                var usertype = db.TblUsers.Where(o => o.Email == value.WrittenBy).Select(p => p.UserType).FirstOrDefault();
                var writtenbyid = db.TblUsers.Where(p => p.Email == value.WrittenBy).Select(m => m.PhoneNumber).FirstOrDefault();
                var writtenforid = "";

                var writtenby = "";

                if (usertype == 0)
                {
                    writtenby = "p";
                    writtenforid = writtenbyid;
                }
                else if (usertype == 1)
                {
                    writtenby = "t";
                    writtenforid = db.TblTreats.Where(o => o.Treatment_Id == value.Treatment_Id).Select(p => p.Patient_Id).FirstOrDefault();
                }

                TblSummary newSummary = new TblSummary();
                int nextSummaryNum = db.TblSummary.Any() ? db.TblSummary.Max(s => s.Summary_Num) + 1 : 1;
                newSummary.Summary_Num = nextSummaryNum;
                newSummary.WrittenBy = writtenby;
                newSummary.Content = value.Content;
                newSummary.Summary_Date = value.Summary_Date;
                newSummary.ImportentToNote = value.ImportanttoNote;
                newSummary.WrittenById = writtenbyid;
                db.TblTreatment.FirstOrDefault(t => t.Treatment_Id == value.Treatment_Id).WasDone = "S";
                db.TblSummary.Add(newSummary);

                TblWrittenFor newWrittenFor = new TblWrittenFor();
                newWrittenFor.Summary_Num = nextSummaryNum;
                newWrittenFor.Treatment_Id = value.Treatment_Id;
                newWrittenFor.WrittenBy = writtenbyid;
                newWrittenFor.WrittenFor = writtenforid;
                db.TblWrittenFor.Add(newWrittenFor);


                db.SaveChanges();
                return Ok("Save");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("api/GetPatientSummaries")]
        public IHttpActionResult GetPatientSummaries(string email)
        {
            try
            {
                SafePlaceDbContextt db = new SafePlaceDbContextt();

                var patientid = db.TblPatient.Where(o => o.Email == email).Select(p => p.Patient_Id).FirstOrDefault();

                List<SummaryDto> allSummaries = db.TblSummary.Where(x => x.TblWrittenFor.FirstOrDefault().WrittenFor == patientid && x.WrittenById == patientid)
                 .Select(s => new SummaryDto()
                 {
                     Summary_Num = s.Summary_Num,
                     Summary_Date = s.Summary_Date.ToString().Substring(0, 10),
                     Patient_Id = s.TblWrittenFor.FirstOrDefault().TblTreatment.TblTreats.FirstOrDefault().Patient_Id

                 }).ToList();

                return Ok(allSummaries);

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }
        }




  }







