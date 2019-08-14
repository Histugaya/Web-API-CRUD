using FreeShareAPI.Models;
using FreeShareAPI.Models.Dbmodel;
using FreeShareAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FreeShareAPI.Converter
{
    public class UserConverter
    {
        public User ModelToEntity(UserModel model)
        {
            User user = new User();
            if (model != null)
            {
                user.Username = model.Username;
                string secretPassword = new Security().HashPassword(model.Password);
                user.Password = secretPassword;
            }
            return user;
        }


        public User EntityToModel(User user)
        {
            UserModel model = new UserModel();
            if (user != null)
            {
                model.Username = user.Username;
                string secretPassword = new Security().HashPassword(user.Password);
                model.Password = secretPassword;
            }
            return user;
        }
    }
}