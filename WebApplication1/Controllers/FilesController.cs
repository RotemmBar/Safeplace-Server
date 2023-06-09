﻿using DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web.Helpers;
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

                //string id = db.TblUsers.Where(o => o.Email == model.email).Select(p => p.Patient_Id).FirstOrDefault();
                int userFileNum = model.file_num;

                TblFile tblFile = new TblFile();
                tblFile.Content = fileContent;
                tblFile.DateSent = model.date_sent;
                tblFile.File_Num = model.file_num;
                tblFile.FileType_Num = model.file_type_num;

                string Patient_Id_test = "0506369673";
                TblFills tblFills = new TblFills();
                tblFills.Patient_Id = Patient_Id_test;
                tblFills.File_Num = userFileNum;



                db.TblFile.Add(tblFile);
                db.TblFills.Add(tblFills);


                db.SaveChanges();

                return Ok();
            }
            else
            {
                return BadRequest("Invalid file data");
            }
        }


        [HttpPost]
        [Route("api/getpdffiles")]
        public IHttpActionResult LoadFile([FromBody] FillsDto model)
        {
            // Assuming the ID is passed as a property in the model
            string id = model.Id;

            // Query the database to retrieve the file numbers
            var fileNumbers = db.TblFills
                .Where(f => f.Patient_Id == id)
                .Select(f => f.File_Num)
                .ToList();

            // Query the database to retrieve the files based on the file numbers
            var files = db.TblFile
                .Where(f => fileNumbers.Contains(f.File_Num))
                .Select(f => new {
                         f.File_Num,
                        f.DateSent,
                        f.FileType_Num,
                        f.Content
         })
         .ToList();

            // Return the files as a response
            return Ok(files);


        }
    }
}
