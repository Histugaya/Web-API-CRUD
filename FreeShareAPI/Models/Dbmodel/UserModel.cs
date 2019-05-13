using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreeShareAPI.Models.Dbmodel
{
    public class UserModel
    {
        public int User_id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}