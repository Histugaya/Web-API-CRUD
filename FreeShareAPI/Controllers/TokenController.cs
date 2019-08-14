using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using FreeShareAPI.Models;
using FreeShareAPI.Models.Dbmodel;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Security.Principal;
using System.Web;
using FreeShareAPI.CustomAttribute;
using FreeShareAPI.Converter;
using FreeShareAPI.DataManager;
using FreeShareAPI.Common;

namespace FreeShareAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("Api/Token")]
    public class TokenController : ApiController
    {
        private TokenDataManager token;
        private Security security;
        /// <summary>
        /// 
        /// </summary>
        public TokenController()
        {
            token = new TokenDataManager();
            security = new Security();
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="userModel"></param>
       /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public IHttpActionResult login([FromBody] UserModel userModel)
        {
            if (security.CheckUser(userModel))
            {
                bool admin = true;
                string token = security.GenerateToken(userModel.Username, admin);
                return Ok(token);
            }
            return Ok(false);
        }

        /// <summary>
        /// Register a new user with given credentials
        /// </summary>
        [HttpPost]
        [Route("Register")]
        public IHttpActionResult Register([FromBody] UserModel userModel)
        {
            try
            {
                if (userModel != null)
                {
                   User user = new UserConverter().ModelToEntity(userModel);
                   token.Register(user);
                   return Ok(true);
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            return NotFound();
        }

    }
}