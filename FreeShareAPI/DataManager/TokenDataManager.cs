using FreeShareAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FreeShareAPI.DataManager
{
    /// <summary>
    /// 
    /// </summary>
    public class TokenDataManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        public void Register(User user)
        {
            using (FreeShareEntities obj = new FreeShareEntities())
            {
                obj.Users.Add(user);
                obj.SaveChanges();
            }
        }
    }
}