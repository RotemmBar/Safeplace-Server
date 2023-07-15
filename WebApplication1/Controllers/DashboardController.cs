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
    public class DashboardController : ApiController
    {
        [HttpGet]
        [Route("api/DaAllTher")]
        public IHttpActionResult Get()
        {
            {
                SafePlaceDbContextt db = new SafePlaceDbContextt();

                List<TblTherapist> the = db.TblTherapist.ToList();

                var therdic = new Dictionary<string, string>();

                foreach (var i in the)
                {
                    therdic[i.Therapist_Id] = i.FirstName;
                }

                List<TblPatient> patient = db.TblPatient.ToList();
                var patientdic = new Dictionary<string, string>();

                foreach (var i in patient)
                {
                    patientdic[i.Patient_Id] = i.FirstName;
                }


                List<TblTreats> treat = db.TblTreats.ToList();

                var result = new Dictionary<string,HashSet<string>>();

                foreach (var i in treat)
                {
                    var thername = therdic[i.Therapist_Id];
                    var pateintname = patientdic[i.Patient_Id];

                    if (!result.ContainsKey(thername))
                        {
                        result[thername] = new HashSet<string>();
                    }
                    result[thername].Add(pateintname);
                }




                return Ok(result);


            }
        }

        // POST: api/Dashboard
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Dashboard/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Dashboard/5
        public void Delete(int id)
        {
        }
    }
}
