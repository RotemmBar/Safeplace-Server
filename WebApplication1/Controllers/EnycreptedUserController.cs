using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
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
        SafePlaceDb db = new SafePlaceDb();


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
        public IHttpActionResult SignUp([FromBody] UsersDto model) ///Problem with the Super User!!!
        //GO OVER ONCE WE UPLOADED THE DATABASE
        ///***********NOT FINAL********************
        {
            try
            {
                int modelUserType = 2;

                if (model.user_type == "מטפל")
                {
                    modelUserType = 1;
                }
                else if (model.user_type == "מטופל")
                {
                    modelUserType = 0;
                }

                var newUser = new TblUsers
                {
                    Email = model.email,
                    Password = model.phone_number,
                    PhoneNumber = model.phone_number,
                    UserType = modelUserType
                };
                
                db.TblUsers.Add(newUser);
                db.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }


        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login([FromBody] UsersDto model, HttpContext context)
        {
            //Check if passowrd was Changed from the default
            var default_password = db.TblUsers.FirstOrDefault(u => u.Email == model.email);
            if (default_password.Password == model.phone_number)
            {
                //Default login wasn't changed
                return Content(HttpStatusCode.OK, "Change Password");
            }
            else
            {
                var hasdedPassword = db.TblUsers.FirstOrDefault(u => u.Email == model.email);
                string hasded = hasdedPassword.Password;
                if (IsPasswordMatch(model.password, hasded))
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
        }


        #endregion

        [HttpPost]
        [Route("SignUpUser")]
        public IHttpActionResult SignUp([FromBody] EnycreptedUserDto model) ///Problem with the Super User!!!
        //GO OVER ONCE WE UPLOADED THE DATABASE
        ///***********NOT FINAL********************
        {
            try
            {
                var login_credentials = db.TblUsers.FirstOrDefault(u => u.Email == model.email);

                if (login_credentials.UserType == 0)
                {
                    //Patient Login
                    string modelGender = "";

                    if (model.gender == "זכר") { modelGender = "M"; }

                    else { modelGender = "F"; };

                    var newPatient = new TblPatient
                    {
                        FirstName = model.firstname,
                        LastName = model.lastname,
                        BirthDate = model.birthdate,
                        StartDate = model.startdate,
                        Patient_Id = model.user_id
                    };
                    db.TblPatient.Add(newPatient);
                    db.SaveChanges();


                    var newUser = new TblUsers   ////need to go over
                    {
                        Email = model.email,
                        Password = EncryptPassword(model.password),
                        //Id = model.patient_Id,

                    };

                    db.TblUsers.Add(newUser);
                    db.SaveChanges();
                    return Ok();
                }

                else if (login_credentials.UserType == 1 || login_credentials.UserType == 2)
                {
                    //Therapist Login
                    string modelGender = "";

                    if (model.gender == "זכר") { modelGender = "M"; }

                    else { modelGender = "F"; };

                    var newTherapist = new TblTherapist
                    {
                        FirstName = model.firstname,
                        LastName = model.lastname,
                        BirthDate = model.birthdate,
                        StartDate = model.startdate,
                        YearsOfExperience = model.years_of_experience,
                        Therapist_Id = model.user_id
                    };
                    db.TblTherapist.Add(newTherapist);
                    db.SaveChanges();


                    var newUser = new TblUsers   ////need to go over
                    {
                        Email = model.email,
                        Password = EncryptPassword(model.password),
                        //Id = model.patient_Id,

                    };

                    db.TblUsers.Add(newUser);
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
    }
}
