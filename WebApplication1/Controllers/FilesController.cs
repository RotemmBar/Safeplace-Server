using DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Dto;

namespace WebApplication1.Controllers
{
    public class FilesController : ApiController
    {

        SafePlaceDbContextt db = new SafePlaceDbContextt();

        [HttpPost]
        [Route("api/files")]
        public IHttpActionResult UploadFile([FromBody] FilesDto model)
        {

            if (!string.IsNullOrEmpty(model.content))
            {
                byte[] fileContent = Convert.FromBase64String(model.content);

                TblFile tblFile = new TblFile();
                tblFile.Content = fileContent;
                tblFile.DateSent = model.date_sent;
                tblFile.File_Num = model.file_num;
                tblFile.FileType_Num = model.file_type_num;
                db.TblFile.Add(tblFile);
                db.SaveChanges();

                return Ok();
            }
            else
            {
                return BadRequest("Invalid file data");
            }
        }
    }
}
