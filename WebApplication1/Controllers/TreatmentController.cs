using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DATA;
using WebApplication1.Dto;
namespace WebApplication1.Controllers
{
    public class TreatmentController : ApiController
    {

        [HttpGet]
        [Route("api/amen/{year}/{month}/{day}")]
        public IHttpActionResult Getfreetreat(int year, int month, int day)
        {
            #region
            //DateTime daytemp = DateTime.Today; //the day of the treatment 
            //DateTime start = daytemp.Date.AddHours(8); //Our earliest appoinment (8:00)
            //DateTime end = daytemp.Date.AddHours(-1); //Our latest appointment (23:00)
            #endregion
            //var hour = new Dictionary<int, string>();
            //hour.Add(1, "10:00");
            //hour.Add(2, "14:00");
            //hour.Add(3, "16:00");

            string[] hours = new string[] { "10:00", "14:00", "16:00" }; ///when therapist is avialble 



            string id = "1";
            DateTime udate = new DateTime(year, month, day);

            SafePlaceDbContextt db = new SafePlaceDbContextt();

            List<TblTreatment> treatsbyday = db.TblTreatments.Where(o => o.Treatment_Date == udate).ToList();
            List<TblTreatment> treatsbydayandther = treatsbyday.Where(y => y.TblTreats.Any(c => c.Therapist_Id == id)).ToList();
            List<TblTreatment> room1 = treatsbyday.Where(u => u.Room_Num == 1).ToList(); //all treatments happenning TODAY in room 1
            List<TblTreatment> room2 = treatsbyday.Where(u => u.Room_Num == 2).ToList(); //all treatments happenning TODAY in room 2

            var lis = new Dictionary<int, string>();

            foreach (var treatment in treatsbydayandther)
            {
                DateTime temp = (DateTime)treatment.StartTime;
                string dem = temp.ToShortTimeString();
                lis[treatment.Treatment_Id] = dem; //Dictionary ordered by Treatment number- the value is the string of the hour
            } //Insets hours to dictionary. (Therapist+Day) Based on Treatment Id

            foreach (var tre in lis)
            {
                for (int i = 0; i < hours.Length; i++)
                {
                    if (tre.Value == hours[i])
                    {
                        hours[i] = "Taken";
                    }
                }
            } //Checks if any of the hours are taken based on theapist+day
            foreach (var r1 in room1)
            {
                DateTime t = (DateTime)r1.StartTime;
                string r1time = t.ToShortTimeString(); //a string of all hours hapenning in room 1 for given day

                for (int i = 0; i < hours.Length; i++)
                {
                    if (hours[i] == "Taken")
                    {
                        break;
                    }

                    if (hours[i] == r1time)
                    {
                        hours[i] = "Taken";
                    }
                }

            } //Checks if any of the hours are taken based on Room1 and day
            foreach (var r2 in room2)
            {
                DateTime t = (DateTime)r2.StartTime;
                string r2time = t.ToShortTimeString(); //a string of all hours hapenning in room 1 for given day

                for (int i = 0; i < hours.Length; i++)
                {
                    if (hours[i] == "Taken")
                    {
                        break;
                    }

                    if (hours[i] == r2time)
                    {
                        hours[i] = "Taken";
                    }
                }

            }

            #region dic
            //foreach (var tre in lis)
            //{
            //    foreach(var hr in hour)
            //    {
            //        if (tre.Value == hr.Value)
            //        {
            //            hour.Remove(hr.Key);
            //        }
            //    }
            //} //Checks if any of the hours are taken based on theapist+day



            //foreach (var r1 in room1)
            //{
            //    DateTime t = (DateTime)r1.StartTime;
            //    string r1time = t.ToShortTimeString(); //a string of all hours hapenning in room 1 for given day

            //    for (int i = 1; i <= hour.Count; i++)
            //    {

            //        if (hour[i] == r1time)
            //        {
            //            hour.Remove(i);
            //        }
            //    }

            //} //Checks if any of the hours are taken based on Room1 and day


            //foreach (var r2 in room2)
            //{
            //    DateTime t = (DateTime)r2.StartTime;
            //    string r2time = t.ToShortTimeString(); //a string of all hours hapenning in room 1 for given day

            //    for (int i = 1; i <= hour.Count; i++)
            //    {
            //        if (hour[i] == r2time)
            //        {
            //            hour.Remove(i);
            //        }
            //    }

            //}
            //Checks if any of the hours are taken based on Room2 and day
            #endregion dic
            return Ok(hours);

            /////***NEED TO ADD: End times
        }

        
        // GET: api/Treatment/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Treatment
        [HttpPost]
        [Route("api/CreateTre")]
        public void Post([FromBody] TblTreatment value)
        {
            SafePlaceDbContextt db = new SafePlaceDbContextt();
            int temp = db.TblTreatments.Max(o => o.Treatment_Id) + 1;

            string date = value.Treatment_Date.ToString();
            string time = value.StartTime.ToString();

            string dateAndTime = date.Trim() + ' ' + time.Trim();

            DateTime dattem = DateTime.Parse(date);

            try
            {
                TblTreatment trea = new TblTreatment();

                trea.Treatment_Id = temp;
                trea.Treatment_Date = dattem;
                trea.StartTime = value.StartTime;
                trea.EndTime = value.StartTime.Value.AddHours(1);
                trea.WasDone = value.WasDone;
                trea.Type_Id = value.Type_Id;
                trea.Room_Num = value.Room_Num;


                TblTreat tr = new TblTreat();

                tr.Patient_Id = "1";
                tr.Therapist_Id = "1";
                tr.Treatment_Id = temp;

                //db.TblTreats.Add(tr);
                //db.TblTreatment.Add(trea);
                db.SaveChanges();
            }

            catch (Exception e)
            {
                throw (e);
            }

        }


    }
}
