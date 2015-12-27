using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DailySentenceService.Models;

namespace DailySentenceService.Controllers
{
    [RoutePrefix("users")]
    public class UsersController : ApiController
    {
        private DailySentenceContext db = new DailySentenceContext();

        [Route("register")]
        [HttpPost]
        public HttpResponseMessage Register(User userModel)
        {
            var userExists = db.Users.Any(u => u.Phonenumber == userModel.Phonenumber);

            if (userExists) return Request.CreateResponse(HttpStatusCode.BadRequest);

            userModel.Password = HashPassword(userModel.Password);

            db.Users.Add(userModel);
            db.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [Route("login")]
        [HttpPost]
        public HttpResponseMessage Login(User userModel)
        {
            var user = db.Users.FirstOrDefault(u => u.Phonenumber == userModel.Phonenumber);

            if (user == null) return Request.CreateResponse(HttpStatusCode.BadRequest);

            if (user.Password == HashPassword(userModel.Password))
                return Request.CreateResponse(HttpStatusCode.OK);

            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        public string HashPassword(string originalPassword)
        {
            HashAlgorithm hash = new SHA256Managed();
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(originalPassword);
            var hashBytes = hash.ComputeHash(plainTextBytes);
            var hashValue = Convert.ToBase64String(hashBytes);
            return hashValue;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}