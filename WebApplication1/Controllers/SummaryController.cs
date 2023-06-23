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
    public class SummaryController : ApiController
    {

        [HttpGet]
        [Route("api/GetSummaryByDate/{PatientId}/{Date}")]
        public IHttpActionResult GetSummaryByNum(string PatientId, string date)
        {
            try
            {
                SafePlaceDbContextt db = new SafePlaceDbContextt();
                SummaryDto Summary = db.TblSummary.Where(a => a.Summary_Date.ToString().Substring(0, 10) == date && a.WrittenBy == "t" 
                && a.TblWrittenFor.FirstOrDefault().TblTreatment.TblTreats.FirstOrDefault().Patient_Id == PatientId).Select(x => new SummaryDto()

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
                List<SummaryDto> allSummaries = db.TblSummary.Where(x => x.TblWrittenFor.FirstOrDefault().TblTreatment.TblTreats.FirstOrDefault().Patient_Id == PatientId)

                    .Select(s => new SummaryDto()
                    {
                        Summary_Num = 0,
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
