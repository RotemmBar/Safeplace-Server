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
        SafePlaceDbContext


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
                var newUser = new TblUsers
                {
                    Email = model.email,
                    Password = model.email + model.phone_number,
                    PhoneNumber = model.phone_number,
                    UserType = model.user_type,
                    
                };
                
                db.TblUsers.Add(newUser);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }


        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login([FromBody] DecryptUserDto model, HttpContext context)
        {
            using (var db = new SafePlaceDbContext())
            {
                var hasdedPassword = db.TblUsers.FirstOrDefault(u => u.Email == model.email);
                string hasded = hasdedPassword.Password;
                if (IsPasswordMatch(model.password, hasded))
                {
                    ////Check for user IsSuperUser Level
                    //int userLevel = hasdedPassword.IsSuperUser;

                    //// Generate a Session Authentication token
                    //string sessionToken = Guid.NewGuid().ToString("N");

                    //// Set the token as a session variable
                    //context.Session["SessionToken"] = sessionToken;

                    //// Create an HTTP response
                    //context.Response.StatusCode = (int)HttpStatusCode.OK;
                    //context.Response.ContentType = "application/json";

                    //context.Response.Write("{\"sessionToken\":\"" + sessionToken + "\"}");

                    ////return Content(HttpStatusCode.OK, userLevel);
                    //return context;
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }


            }


        }


        #endregion

        //[HttpPost]
        //[Route("SignUp")]
        //public IHttpActionResult SignUp([FromBody] EnycreptedUserDto model) ///Problem with the Super User!!!
        ////GO OVER ONCE WE UPLOADED THE DATABASE
        /////***********NOT FINAL********************
        //{
        //    try
        //    {
        //        using (var db = new SafePlaceDbContext())
        //        {
        //            string modelGender = "";

        //            if (model.gender == "זכר")
        //            {
        //                modelGender = "M";
        //            }
        //            else
        //            {
        //                modelGender = "F";
        //            };

        //            var newPatient = new TblPatient
        //            {
        //                FirstName = model.firstname,
        //                LastName = model.lastname,
        //                BirthDate = model.birthdate,
        //                StartDate = model.startdate,
        //                Patient_Id = model.user_id
        //            };
        //            db.TblPatient.Add(newPatient);
        //            db.SaveChanges();

        //        }
        //        using (var db = new SafePlaceDbContext())
        //        {
        //            var newUser = new TblUsers   ////need to go over
        //            {
        //                Email = model.email,
        //                Password = EncryptPassword(model.password),
        //                //Id = model.patient_Id,

        //            };

        //            db.TblUsers.Add(newUser);
        //            db.SaveChanges();
        //            return Ok();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(HttpStatusCode.BadRequest, ex);
        //    }
        //}
    }
}
