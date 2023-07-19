using DATA;
using System;
using System.Linq;
using System.Web.Http;
using WebApplication1.Dto;

namespace WebApplication1.Controllers
{
    public class FilesController : ApiController
    {

        SafePlaceDbContextt db = new SafePlaceDbContextt();

        //[HttpPost]
        //[Route("api/files")]
        //public IHttpActionResult UploadFile([FromBody] FilesDto model)
        //{

        //    if (!string.IsNullOrEmpty(model.content))
        //    {
        //        byte[] fileContent = Convert.FromBase64String(model.content);

        //        //string id = db.TblUsers.Where(o => o.Email == model.email).Select(p => p.Patient_Id).FirstOrDefault();
        //        int userFileNum = model.file_num;

        //        TblFile tblFile = new TblFile();
        //        tblFile.Content = fileContent;
        //        tblFile.DateSent = model.date_sent;
        //        tblFile.File_Num = model.file_num;
        //        tblFile.FileType_Num = model.file_type_num;

        //        string Patient_Id_test = "0506369673";
        //        TblFills tblFills = new TblFills();
        //        tblFills.Patient_Id = Patient_Id_test;
        //        tblFills.File_Num = userFileNum;



        //        db.TblFile.Add(tblFile);
        //        db.TblFills.Add(tblFills);


        //        db.SaveChanges();

        //        return Ok();
        //    }
        //    else
        //    {
        //        return BadRequest("Invalid file data");
        //    }
        //}
        [HttpPost]
        [Route("api/files")]
        public IHttpActionResult UploadFile([FromBody] FilesDto model)
        {
            if (model.FileName != null || model.FilePath != null)
            {
                //Get the max file num
                int maxFileNum = db.TblFile.Max(o => o.File_Num);

                //increase the max file num by one to get a new file num
                int newFileNum = maxFileNum + 1;


                // Save this file path to your SQL Server database
                TblFile tblFile = new TblFile();
                tblFile.FilePath = model.FilePath; // Assuming ContentPath is the property where you want to save the path
                tblFile.DateSent = DateTime.Today;
                tblFile.File_Num = newFileNum;
                tblFile.FileType_Num = model.file_type_num;
                tblFile.File_name = model.FileName;

                string filler_id1 = db.TblUsers.Where(x => x.Email == model.filler_Id).Select(y => y.PhoneNumber).FirstOrDefault().ToString(); //אימייל של המשתמש
                //string patientId1 = db.TblTreats.Where(x => x.Treatment_Id == model.TreatmentId).Select(y => y.Patient_Id).ToString();
                //string TherapistId = db.TblTreats.Where(x => x.Treatment_Id == model.TreatmentId).Select(y => y.Therapist_Id).ToString();

                string patientId1 = db.TblTreats.Where(x => x.Treatment_Id == model.TreatmentId).Select(y => y.Patient_Id).FirstOrDefault().ToString();
                TblFills tblFills = new TblFills();
                tblFills.Patient_Id = patientId1;
                tblFills.File_Num = newFileNum;
                tblFills.Filler_Id = filler_id1;

                db.TblFile.Add(tblFile);
                db.TblFills.Add(tblFills);

                db.SaveChanges();

                return Ok();
            }
            else {
                return BadRequest();
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
                          f.File_name,
                         f.File_Num,
                        f.DateSent,
                        f.FileType_Num,
                        f.FilePath
                })
         .ToList();

            // Return the files as a response
            return Ok(files);


        }


        [HttpGet]
        [Route("api/gettherapistpatientsfiles2/{therapistId}")]
        public IHttpActionResult GetPatientFiles(string therapistId)
        {
            var therapist_Patientlist = db.TblTreats
             .Where(f => f.Therapist_Id == therapistId)
             .Select(f => f.Patient_Id)
              .Distinct()
               .ToList(); //מקבל את הרשימת מטופלים של אותו מטפל ספציפי

            
            var fills = db.TblFills
           .Where(f => therapist_Patientlist.Contains(f.Patient_Id) && f.Filler_Id == f.Patient_Id)
           .Select(f => new
          {
          f.File_Num,
          f.Patient_Id,
          f.Filler_Id
            })
          .ToList();

            // Extract FileNums from fills
            var fileNums = fills.Select(f => f.File_Num).ToList();

            
            // Get files from TblFile that have the same File_Num
            var files = db.TblFile
                .Where(f => fileNums.Contains(f.File_Num))
                .Select(f => new
                {
                    f.File_name,
                    f.File_Num,
                    f.DateSent,
                    f.FileType_Num,
                    f.FilePath,
                    f.TblFills.FirstOrDefault().TblPatient.FirstName,
                    f.TblFills.FirstOrDefault().TblPatient.LastName
                })
                .ToList();

            // Return the files as a response
            return Ok(files);
        }

        [HttpGet]
        [Route("api/gettherapistfiles/{therapistId}")]
        public IHttpActionResult GetTherapistFiles(string therapistId)
        {
            var therapist_Patientlist = db.TblTreats
             .Where(f => f.Therapist_Id == therapistId)
             .Select(f => f.Patient_Id)
              .Distinct()
               .ToList(); //מקבל את הרשימת מטופלים של אותו מטפל ספציפי


            var fills = db.TblFills
           .Where(f => therapist_Patientlist.Contains(f.Patient_Id) && f.Filler_Id == therapistId)
           .Select(f => new
           {
               f.File_Num,
               f.Patient_Id,
               f.Filler_Id
           })
          .ToList();

            // Extract FileNums from fills
            var fileNums = fills.Select(f => f.File_Num).ToList();

            // Get files from TblFile that have the same File_Num
            var files = db.TblFile
                .Where(f => fileNums.Contains(f.File_Num))
                .Select(f => new
                {
                    f.File_name,
                    f.File_Num,
                    f.DateSent,
                    f.FileType_Num,
                    f.FilePath,
                    f.TblFills.FirstOrDefault().TblPatient.FirstName,
                    f.TblFills.FirstOrDefault().TblPatient.LastName,
                    TherapistFirstName = f.TblFills.FirstOrDefault().TblPatient.TblTreats.FirstOrDefault().TblTherapist.FirstName,
                    TherapistLastName = f.TblFills.FirstOrDefault().TblPatient.TblTreats.FirstOrDefault().TblTherapist.LastName
                })
                .ToList();

            // Return the files as a response
            return Ok(files);
        }

        [HttpGet]
        [Route("api/getFileById/{meetingNum}")]
        public IHttpActionResult GetFileById(int meetingNum)
        {
            var file = db.TblFile
                .FirstOrDefault(f => f.TblFills.Any(fill => fill.TblPatient.TblTreats
                    .Any(treat => treat.TblTreatment.Treatment_Id == meetingNum)));

            if (file == null)
            {
                // File not found, return appropriate response
                return NotFound();
            }

            var fileInfo = new
            {
                file.File_name,
                file.File_Num,
                file.DateSent,
                file.FileType_Num,
                file.FilePath,
                file.TblFills.FirstOrDefault().TblPatient.FirstName,
                file.TblFills.FirstOrDefault().TblPatient.LastName
            };

            // Return the file information as a response
            return Ok(fileInfo);
        }

    }


}


