using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1;
using DATA;
using WebApplication1.Dto;
using System.Globalization;
namespace WebApplication1.Controllers
{
    public class AdminController : ApiController
    {
        [HttpGet]
        [Route("api/GetAllTherapits/")]
        public List<TherapistDto> GetAllTherapits(string email)
        {
            try
            {
                SafePlaceDbContextt db = new SafePlaceDbContextt();
                List<TherapistDto> listTherapist = db.TblTherapist.Where(u=>u.Email!=email)
                .Select(x => new TherapistDto
                {
                    Therapist_Id = x.Therapist_Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    StartTime = x.StartDate,
                    Gender = x.Gender,
                    BirthDate = x.BirthDate,
                    YearsOfExperience = x.YearsOfExperience,
                }).ToList();

                if (listTherapist.Any())
                {
                    return listTherapist;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception e)
            {
                return null;
            }
        }


        [HttpGet]
        [Route("api/TherapisCard/{therapistId}")]
        public IHttpActionResult GetPatientByPatienttId(string therapistId)
        {
            try
            {
                SafePlaceDbContextt db = new SafePlaceDbContextt();

                TherapistDto the = db.TblTherapist.Where(o => o.Therapist_Id == therapistId).Select(o => new TherapistDto()
                {
                    FirstName = o.FirstName,
                    LastName = o.LastName,
                    Email = o.Email,
                    Age = DateTime.Now.Year - o.BirthDate.Year,
                    NumTreatments = o.TblTreats.Count(),
                    Therapist_Id=o.Therapist_Id,
                    YearsOfExperience=o.YearsOfExperience,

                }).SingleOrDefault();
     

                return Ok(the);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }


    }
}
