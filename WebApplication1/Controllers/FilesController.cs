using DATA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApplication1.Controllers
{
    public class FilesController : ApiController
    {

        SafePlaceDbContextt db = new SafePlaceDbContextt();

        [HttpPost("upload")]
        public async Task<IHttpActionResult> Upload(IFormFile file)
        {
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            var fileBytes = ms.ToArray();

            var fileData = new FileData
            {
                FileName = file.FileName,
                Data = fileBytes
            };

            db.Tb.Add(fileData);
            await db.SaveChangesAsync();

            return Ok();
        }
    }
}
