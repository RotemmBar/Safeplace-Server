﻿using System;
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
        public IHttpActionResult Getfreetreat(int year, int month, int day, string email)
        {
            #region
            //DateTime daytemp = DateTime.Today; //the day of the treatment 
            //DateTime start = daytemp.Date.AddHours(8); //Our earliest appoinment (8:00)
            //DateTime end = daytemp.Date.AddHours(-1); //Our latest appointment (23:00)
            //TreatmentDto[] freetreatment = new TreatmentDto[]
            //{
            //     new TreatmentDto
            //   {
            //       Room_Num=0,
            //       startTimetemp="08:00",
            //       available="Y"

            //   },
            //      new TreatmentDto
            //   {
            //       Room_Num=0,
            //       startTimetemp="09:00",
            //       available="Y"

            //   },
            //       new TreatmentDto
            //   {
            //       Room_Num=0,
            //       startTimetemp="10:00",
            //       available="Y"

            //   },
            //   new TreatmentDto
            //   {
            //       Room_Num=0,
            //       startTimetemp="11:00",
            //       available="Y"

            //   },
            //    new TreatmentDto
            //   {
            //      Room_Num=0,
            //      startTimetemp="12:00",
            //      available="Y"

            //   },
            //     new TreatmentDto
            //   {
            //       Room_Num=0,
            //       startTimetemp="13:00",
            //       available="Y"
            //   },

            //};
            #endregion

            TreatmentDto[] freetreatment = new TreatmentDto[15];

            for (int i = 0; i < 15; i++)
            {
                int hour = 8 + i;
                freetreatment[i] = new TreatmentDto
                {
                    Room_Num = 0,
                    startTimetemp = hour.ToString("00") + ":00",
                    available = "Y",
                    recommended = false,
                    StartTime = DateTime.Now
                };
            };

            DateTime udate = new DateTime(year, month, day);

            SafePlaceDbContextt db = new SafePlaceDbContextt();

            var patientId = db.TblPatient.Where(o => o.Email == email).Select(p => p.Patient_Id).FirstOrDefault();
            string therapistid = db.TblTreats.Where(u => u.Patient_Id == patientId).Select(p => p.Therapist_Id).FirstOrDefault();

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

            foreach (var tre in lis)
            {
                foreach (var free in freetreatment)
                {
                    //string startA = tre.Value.Split(' ')[0].Split(':')[0];
                    //string startB = free.startTimetemp.Split(':')[0];
                    //if (startB[0] == '0')
                    //    startB = startB.Substring(1);
                    if (IsSameHour(tre.Value, free.startTimetemp))
                    {
                        free.available = "N";
                        DateTime startTime;
                        if (DateTime.TryParse(tre.Value, out startTime))
                        {
                            free.StartTime = startTime;
                        }
                    }
                }
            } //Checks if any of the hours are taken based on theapist+day
              //proceed to check if rooms are available in chosen time.

            foreach (var r1 in room1)
            {
                DateTime t = (DateTime)r1.StartTime;
                string r1time = t.ToShortTimeString(); //a string of all hours hapenning in room 1 for given day

                foreach (var free in freetreatment)
                {
                    if (free.available != "N")
                    {
                        if (!IsSameHour(r1time, free.startTimetemp))
                        {
                            free.Room_Num = 1;
                            free.available = "RoomFound";
                        }
                        //if (IsSameHour(r1time, free.startTimetemp))

                        //{
                        //    free.available = "Taken1";
                        //}
                        //else
                        //{
                        //    free.Room_Num = 1;
                        //    free.available = "RoomFound";
                        //}
                    }
                }

            } //Checks if any of the hours are taken based on Room1 and day

            foreach (var r2 in room2)
            {
                DateTime t = (DateTime)r2.StartTime;
                string r2time = t.ToShortTimeString(); //a string of all hours hapenning in room 1 for given day

                foreach (var free in freetreatment)
                {
                    if (free.available != "N")
                    {

                        if (!IsSameHour(r2time, free.startTimetemp))
                        {
                            free.Room_Num = 2;
                            free.available = "RoomFound";
                        }
                        //if (free.available == "Taken1")
                        //{
                        //    if (IsSameHour(r2time, free.startTimetemp))
                        //    {
                        //        free.available = "Taken2";
                        //    }
                        //    else
                        //    {
                        //        free.Room_Num = 2;
                        //    }
                        //}
                        //if (free.available == "Y")
                        //{
                        //    free.Room_Num = 2;
                        //}
                    }

                }

            } //Checks if any of the hours are taken based on Room1 and day

            if (room1.Count == 0)
            {
                foreach (var option in freetreatment)
                {
                    option.Room_Num = 1;
                }
            }
            else if (room2.Count == 0)
            {
                foreach (var option in freetreatment)
                {
                    option.Room_Num = 2;
                }
            }


            TreatmentDto[] final = new TreatmentDto[0];
            final = freetreatment.Where(c => c.available != "N").ToArray();

            List<TblTreatment> alltreatment = db.TblTreatment.ToList(); ///a list of all treatments

            Dictionary<string, int> score = new Dictionary<string, int>(); // cancelled each hour

            foreach (var i in final)
            {
                if (!score.ContainsKey(i.startTimetemp))
                {
                    if (treatsbydayandtherapist.Count == 0)
                    {
                        score[i.startTimetemp] = 0;
                        continue;
                    }

                    foreach (var temp in treatsbydayandtherapist)
                    {
                        DateTime t = (DateTime)temp.StartTime;
                        string timetemp = t.ToShortTimeString();
                        int inttreatmenthour = int.Parse(timetemp.Split(':')[0]);

                        int freehour = int.Parse(i.startTimetemp.Split(':')[0]);

                        int point = Math.Abs(inttreatmenthour - freehour);
                        score[i.startTimetemp] = point;
                    }
                }
            }
            foreach(var i in alltreatment)
            {
                DateTime t = (DateTime)i.StartTime;
                string timetemp = t.ToShortTimeString();

                if (i.WasDone == "C")
                {
                    if (score.ContainsKey(timetemp))
                    {
                        score[timetemp] += 1;
                    } 
                }
            }

            int minumuncancel = score.Values.Min();

            foreach (var treatment in final)
            {
                if (score[treatment.startTimetemp] >= minumuncancel && score[treatment.startTimetemp] <= minumuncancel + 3)
                {
                    treatment.recommended = true;
                }
            }

            return Ok(final);


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
            string patienEmail = value.Patient_Email.ToString();
            string patientId = db.TblPatient.Where(o => o.Email == patienEmail).Select(p => p.Patient_Id).FirstOrDefault();
            string therapistId = db.TblTreats.Where(o => o.Patient_Id == patientId).Select(p => p.Therapist_Id).FirstOrDefault();
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

                tr.Patient_Id = patientId;
                tr.Therapist_Id = therapistId;
                tr.Treatment_Id = temp;

                db.TblTreats.Add(tr);
                db.TblTreatment.Add(trea);
                db.SaveChanges();

                return Ok();
            }

            catch (Exception e)
            {
                return BadRequest();
            }

        }

        static bool IsSameHour (string t1, string t2)
        {
            string startA = t1.Split(' ')[0].Split(':')[0];
            string startB = t2.Split(':')[0];
            if (startB[0] == '0')
                startB = startB.Substring(1);

            return startA.Equals(startB);
        }
        


    }
}
