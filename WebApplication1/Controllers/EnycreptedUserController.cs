using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;
using DATA;
using WebApplication1.Dto;

namespace WebApplication1.Controllers
{
    [RoutePrefix("api/SignInUser")]
    public class EnycreptedUserController : ApiController
    {
        SafePlaceDbContextt db = new SafePlaceDbContextt();


        ///General Functions
        #region general
        public static string EncryptPassword1(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return null;
            }
            else
            {
                byte[] storePassword = ASCIIEncoding.ASCII.GetBytes(password);
                string encryptedPassword = Convert.ToBase64String(storePassword);
                return encryptedPassword;
            }
        }

        public static string DecryptPassword1(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return null;
            }
            else
            {
                byte[] encryptedPassword = ASCIIEncoding.ASCII.GetBytes(password);
                string decryptedPassword = Convert.ToBase64String(encryptedPassword);
                return decryptedPassword;
            }
        }

        public static string EncryptPassword(string password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] encrypted = md5.ComputeHash(bytes);
            return Encoding.UTF8.GetString(encrypted);
        }

        public static bool IsPasswordMatch(string password, string hashedPassword)
        {
            string encryptedPassword = EncryptPassword(password);
            return encryptedPassword.Equals(hashedPassword);
        }

        #endregion

        ///DB API'S
        #region API
        [HttpPost]
        [Route("SignUp")]
        public IHttpActionResult SignUp([FromBody] UsersDto model)    //activated by NewRegister- the manager page
        {
            try
            {
                int modelUserType = 0;


                if (model.UserType == "מטפל")
                {
                    modelUserType = 1;
                }
                else if (model.UserType == "מטופל")
                {
                    modelUserType = 0;
                }
                else
                { modelUserType = 2; }


                var newUser = new TblUsers
                {
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    UserType = modelUserType,
                    Password = model.PhoneNumber
                };

                var theramil = model.TherEmail;
                var therid = db.TblTherapist.Where(o => o.Email == theramil).Select(p => p.Therapist_Id).FirstOrDefault();
                int demoid = db.TblTreatment.Any() ? db.TblTreatment.Max(s => s.Treatment_Id) + 1 : 1;
                var treat = new TblTreats();
                var nepatient = new TblPatient();

                var temptreat = new TblTreatment();
                if(therid!=null)
                {
                    temptreat = new TblTreatment
                    {
                        Treatment_Id = demoid,
                        Treatment_Date = DateTime.Today,
                        StartTime = DateTime.Now,
                        EndTime = DateTime.Now.AddHours(1),
                        WasDone = "N",
                        Type_Id = 1,
                        Room_Num = 3
                    };

                    db.TblUsers.Add(newUser);
                    db.TblTreatment.Add(temptreat);
                    db.SaveChanges();

                    nepatient = new TblPatient
                    {
                        Patient_Id = model.PhoneNumber,
                        PhoneNumber=model.PhoneNumber
                    };

                    treat = new TblTreats
                      {
                          Patient_Id = model.PhoneNumber,
                          Therapist_Id = therid,
                          Treatment_Id = demoid
                      };

                }

                db.TblPatient.Add(nepatient);
                db.TblTreats.Add(treat);
                db.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPost]
        [Route("Changepa")]
        public IHttpActionResult Changepa([FromBody] FirstLoginDto change)    //recieve email and Password, update db
        {
            try
            {
                var temp = db.TblUsers.Where(o => o.Email == change.Email).FirstOrDefault();
                temp.Password = EncryptPassword(change.Password);



                db.Entry(temp).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }

        //[HttpPost]
        //[Route("SignUpManager")]
        //public IHttpActionResult ManagerSignUp([FromBody] UsersDto model) 

        //{
        //    try
        //    {
        //        int modelUserType = 0;

        //        if (model.UserType == "מטפל")
        //        {
        //            modelUserType = 1;
        //        }
        //        else if (model.UserType == "מטופל")
        //        {
        //            modelUserType = 0;
        //        }
        //        else
        //        { modelUserType = 2; }

        //        var newUser = new TblUsers
        //        {
        //            Email = model.Email,
        //            PhoneNumber = model.PhoneNumber,
        //            UserType = modelUserType,
        //            Password = model.PhoneNumber
        //        };

        //        db.TblUsers.Add(newUser);
        //        db.SaveChanges();

        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(HttpStatusCode.BadRequest, ex);
        //    }
        //}

        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login ([FromBody] FirstLoginDto model) //activated by login
        {
            try
            {
                //Check if passowrd was Changed from the default
                var default_password = db.TblUsers.FirstOrDefault(u => u.Email == model.Email);
                if (default_password.Password == model.Password && model.Password == default_password.PhoneNumber)
                {
                    //Default login wasn't changed
                    return Content(HttpStatusCode.Created, $"Change Password {default_password.UserType}");
                }
                else
                {
                    var hasdedPassword = db.TblUsers.FirstOrDefault(u => u.Email == model.Email);
                    string hasded = hasdedPassword.Password;
                    if (IsPasswordMatch(model.Password, hasded))
                    {
                        if (hasdedPassword.UserType == 0)
                        {
                            return Ok();
                        }

                        if (hasdedPassword.UserType == 1)
                        {

                            return Content(HttpStatusCode.Accepted, "pleae");

                        }
                        if(hasdedPassword.UserType== 2)
                        {
                            return Content((HttpStatusCode)226, "Admin login successful");
                        }

                        else
                        {
                            return BadRequest();
                        }
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            catch(Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e);
            }
        }


        [HttpPost]
        [Route("SignUpPatient")]
        public IHttpActionResult SignUpPatient([FromBody] PatientRegisterDto patientRegisterDto) 
  
        {
            try
            {
                var login_credentials = db.TblUsers.FirstOrDefault(u => u.Email == patientRegisterDto.Email);
                string modelGender = "";

                if (patientRegisterDto.Gender == "זכר") { modelGender = "M"; }

                else { modelGender = "F"; };

                if (login_credentials.UserType == 0)
                {
                    //Patient Login

                    var newUser = db.TblUsers.Where(o => o.Email == login_credentials.Email).FirstOrDefault();

                    newUser.Password = EncryptPassword(patientRegisterDto.Password);


                    var newPatient = db.TblPatient.Where(o => o.PhoneNumber == login_credentials.PhoneNumber).FirstOrDefault();
                    newPatient.FirstName = patientRegisterDto.FirstName;
                    newPatient.LastName = patientRegisterDto.LastName;
                    newPatient.BirthDate = patientRegisterDto.BirthDate;
                    newPatient.StartDate = patientRegisterDto.StartDate;
                    newPatient.Patient_Id = login_credentials.PhoneNumber;
                    newPatient.Email = login_credentials.Email;
                    newPatient.Gender = modelGender;

                    //{
                    //    FirstName = patientRegisterDto.FirstName,
                    //    LastName = patientRegisterDto.LastName,
                    //    BirthDate = patientRegisterDto.BirthDate,
                    //    StartDate = patientRegisterDto.StartDate,
                    //    Patient_Id = login_credentials.PhoneNumber,
                    //    PhoneNumber = login_credentials.PhoneNumber,
                    //    Email = newUser.Email ,
                    //    Gender=modelGender
                    //};



                    db.Entry(newUser).State = System.Data.Entity.EntityState.Modified;
                    db.Entry(newPatient).State = System.Data.Entity.EntityState.Modified;

                    db.SaveChanges();
                    return Ok();
                }

                if (login_credentials.UserType == 1 || login_credentials.UserType == 2)
                {
                    var newTherapist = new TblTherapist
                    {
                        FirstName = patientRegisterDto.FirstName,
                        LastName = patientRegisterDto.LastName,
                        BirthDate = patientRegisterDto.BirthDate,
                        Gender = modelGender,
                        StartDate = patientRegisterDto.StartDate,
                        YearsOfExperience = patientRegisterDto.YearsOfExperience,
                        Therapist_Id = login_credentials.PhoneNumber,
                        PhoneNumber=login_credentials.PhoneNumber,
                        Email=login_credentials.Email
                    };
                    db.TblTherapist.Add(newTherapist);

                    var newUser = db.TblUsers.Where(o => o.Email == login_credentials.Email).FirstOrDefault();

                    newUser.Password = EncryptPassword(patientRegisterDto.Password);


                    db.Entry(newUser).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return Ok();
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, "Error with values entered");
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }




        [HttpGet]
        [Route("userexist")]
        public IHttpActionResult UserExist(string email)
        {
            try
            {
                SafePlaceDbContextt db = new SafePlaceDbContextt();
                var u = db.TblUsers.Where(o => o.Email == email).FirstOrDefault();
                if(u==null)
                {
                    return Content(HttpStatusCode.BadRequest, "you cant");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }

        }

        #endregion

    }
}
