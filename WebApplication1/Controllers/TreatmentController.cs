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
            string[] hours = new string[] { "10:00", "14:00", "16:00" }; ///when therapist is available 

            TreatmentDto[] freetreatment = new TreatmentDto[]
            {
               new TreatmentDto
               {
                   Room_Num=0,
                   startTimetemp="10:00",
                   available="Y"

               },
                new TreatmentDto
               {
                  Room_Num=0,
                  startTimetemp="14:00",
                  available="Y"

               },
                 new TreatmentDto
               {
                   Room_Num=0,
                   startTimetemp="16:00",
                   available="Y"
               },

            };

    
        
            string therapistid = "1";
            DateTime udate = new DateTime(year, month, day);

            SafePlaceDbContextt db = new SafePlaceDbContextt();

            List<TblTreatment> treatsbyday = db.TblTreatment.Where(o => o.Treatment_Date == udate).ToList(); //treatment for the day picked
            List<TblTreatment> treatsbydayandtherapist = treatsbyday.Where(y => y.TblTreats.Any(c => c.Therapist_Id == therapistid)).ToList();
            List<TblTreatment> room1 = treatsbyday.Where(u => u.Room_Num == 1).ToList(); //all treatments happenning TODAY in room 1
            List<TblTreatment> room2 = treatsbyday.Where(u => u.Room_Num == 2).ToList(); //all treatments happenning TODAY in room 2
            

            var lis = new Dictionary<int, string>(); 

            foreach (var treatment in treatsbydayandtherapist)
            {
                DateTime temp = (DateTime)treatment.StartTime;
                string stringstarttime = temp.ToShortTimeString();
                lis[treatment.Treatment_Id] = stringstarttime; //Dictionary ordered by Treatment number- the value is the string of the start time
            } //Inserts hours to dictionary. (Therapist+Day) Based on Treatment Id
            #region trash
            //foreach (var tre in lis)
            //{
            //    for (int i = 0; i < hours.Length; i++)
            //    {
            //        if (tre.Value == hours[i])
            //        {
            //            hours[i] = "Taken";
            //        }
            //    }
            //} //Checks if any of the hours are taken based on theapist+day
            ////proceed to check if rooms are available in chosen time.
            ///
               //foreach (var r1 in room1)
            //{
            //    DateTime t = (DateTime)r1.StartTime;
            //    string r1time = t.ToShortTimeString(); //a string of all hours hapenning in room 1 for given day

            //    for (int i = 0; i < hours.Length; i++)
            //    {
            //        if (hours[i] == "Taken")
            //        {
            //            continue;
            //        }

            //        if (hours[i] == r1time)
            //        {
            //            hours[i] = "Taken";
            //        }

            //        if (hours[i] != "Taken")
            //        {
            //            foreach (var d in freetreatment)
            //            {
            //                if(hours[i]==d.startTimetemp)
            //                {
            //                    d.Room_Num = 1;
            //                }
            //            }
            //        }            
            //    }

            //} //Checks if any of the hours are taken based on Room1 and day

            //foreach (var r2 in room2)
            //{
            //    DateTime t = (DateTime)r2.StartTime;
            //    string r2time = t.ToShortTimeString(); //a string of all hours hapenning in room 1 for given day

            //    for (int i = 0; i < hours.Length; i++)
            //    {
            //        if (hours[i] == "Taken")
            //        {
            //            continue;
            //        }

            //        if (hours[i] == r2time)
            //        {
            //            hours[i] = "Taken";
            //        }

            //        if(hours[i]!="Taken")
            //        {
            //            foreach (var d in freetreatment)
            //            {
            //                if (hours[i] == d.startTimetemp)
            //                {
            //                    d.Room_Num = 2;
            //                }
            //            }                  

            //        }
            //    }

            //}
            //return Ok(freetreatment);
            #endregion

            foreach (var tre in lis)
            {
                foreach (var free in freetreatment)
                {
                    if (tre.Value == free.startTimetemp)
                    {
                       free.available = "No";
                    }
                }
            } //Checks if any of the hours are taken based on theapist+day
              //proceed to check if rooms are available in chosen time.

            foreach (var r1 in room1)
            {
                DateTime t = (DateTime)r1.StartTime;
                string r1time = t.ToShortTimeString(); //a string of all hours hapenning in room 1 for given day

               foreach(var free in freetreatment)
                {
                    if (free.available != "No")
                    {
                        if (free.startTimetemp == r1time)
                        {
                            free.available = "Taken1";
                        }
                        else
                        {
                            free.Room_Num = 1;
                            free.available = "RoomFound";
                        }
                    }
                }

            } //Checks if any of the hours are taken based on Room1 and day

            foreach (var r2 in room2)
            {
                DateTime t = (DateTime)r2.StartTime;
                string r2time = t.ToShortTimeString(); //a string of all hours hapenning in room 1 for given day

                foreach (var free in freetreatment)
                {
                    if (free.available != "No")
                    {
                        if (free.available == "Taken1")
                        {
                            if (free.startTimetemp == r2time)
                            {
                                free.available = "Taken2";
                            }
                            else
                            {
                                free.Room_Num = 2;
                            }
                        }
                        if (free.available == "Y")
                        {
                            free.Room_Num = 2;
                        }
                    }
  
                }

            } //Checks if any of the hours are taken based on Room1 and day

            if(room1.Count==0)
            {
                foreach(var option in freetreatment)
                {
                    option.Room_Num = 1;
                }
            }
            else if(room2.Count==0)
            {
                foreach(var option in freetreatment)
                {
                    option.Room_Num = 2;
                }
            }

            TreatmentDto[] final = new TreatmentDto[0];
            final = freetreatment.Where(c => c.Room_Num != 0 || c.available== "Taken2").ToArray();
            return Ok(final);       
                   
        
            /////***NEED TO ADD: End times
        }


        // POST: api/Treatment
        [HttpPost]
        [Route("api/CreateTre")]
        public IHttpActionResult Post([FromBody] TreatmentDto value)
        {
            SafePlaceDbContextt db = new SafePlaceDbContextt();
            int temp = db.TblTreatment.Max(o => o.Treatment_Id) + 1;

            string date = value.TreatmentDate.ToShortDateString();
            string time = value.StartTime.ToShortTimeString();

            string dateAndTime = date.Trim() + ' ' + time.Trim();

            DateTime dattem = DateTime.Parse(dateAndTime);
            try
            {
                TblTreatment trea = new TblTreatment();

                trea.Treatment_Id = temp;
                trea.Treatment_Date = dattem;
                trea.StartTime = dattem;
                trea.EndTime = dattem.AddHours(1);
                trea.WasDone = value.WasDone;
                trea.Type_Id = value.Type_Id;
                trea.Room_Num = value.Room_Num;


                TblTreats tr = new TblTreats();

                tr.Patient_Id = "2";
                tr.Therapist_Id = "1";
                tr.Treatment_Id = temp;

                db.TblTreats.Add(tr);
                db.TblTreatment.Add(trea);
                //db.SaveChanges();

                return Ok();
            }

            catch (Exception e)
            {
                return BadRequest();
            }

        }


    }
}
