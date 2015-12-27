using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DailySentenceService.Models
{
    public class User
    {
        public User()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Phonenumber { get; set; }
    }
}