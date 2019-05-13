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

namespace FreeShareAPI.Controllers
{
    [RoutePrefix("Api/Token")]
    public class TokenController : ApiController
    {
        private string secretkey;

        public TokenController()
        {
            secretkey = ConfigurationManager.AppSettings["Secret"];
        }

        /// <summary>
        /// Get token for given credentials
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public IHttpActionResult login([FromBody] UserModel userModel)
        {
            if (CheckUser(userModel))
            {
                bool admin = false;
                string token = GenerateToken(userModel.Username, admin);
                return Ok(token);
            }

            return Ok(false);
        }

        public bool CheckUser(UserModel userModel)
        {
            bool result = false;
            string decodePassword;
            if (userModel != null)
            {
                using (FreeShareEntities obj = new FreeShareEntities())
                {
                    decodePassword = hashPassword(userModel.Password);
                    User user = obj.Users.FirstOrDefault(x => x.Username == userModel.Username
                                                         && x.Password == decodePassword);
                    if (user != null)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        public string GenerateToken(string username, bool admin, int expireMinutes = 2)
        {
            DateTime now = DateTime.UtcNow;
            DateTime expires = now.AddMinutes(Convert.ToInt32(expireMinutes));
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            JwtHeader header = new JwtHeader(credentials);

            var payload = new JwtPayload {
                {"username ", username},
                {"admin", admin},
                {"expiryDate", expires}
         };

            var secToken = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();

            return handler.WriteToken(secToken);

        }

        /// <summary>
        ///Validate token with secret key
        /// </summary>
        /// <param name="authToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ValidateToken")]
        public bool ValidateToken(string authToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = GetValidationParameters();

                SecurityToken validatedToken;
                IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
                return true;
            }catch(Exception ex)
            {
                return false;
            }

            
        }

        public TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = false, // Because there is no expiration in the generated token
                ValidateAudience = false, // Because there is no audiance in the generated token
                ValidateIssuer = false,   // Because there is no issuer in the generated token
                ValidIssuer = "Sample",
                ValidAudience = "Sample",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey)) // The same key as the one that generate the token
            };
        }

        public string hashPassword(string password)
        {

            string hash;
            var sha1 = new SHA256Managed();
            byte[] temp = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < temp.Length; i++)
            {
                sb.Append(temp[i].ToString("x2"));
            }

            hash = sb.ToString();
            return hash;

        }

        /// <summary>
        /// Register a new user
        /// </summary>
        [HttpPost]
        [Route("Register")]
        public IHttpActionResult Register([FromBody] UserModel userModel)
        {
            //bool result = false;
            string secretPassword;
            try
            {
                if (userModel != null)
                {
                    {
                        using (FreeShareEntities obj = new FreeShareEntities())
                        {
                            User user = new User();
                            user.Username = userModel.Username;
                            secretPassword = hashPassword(userModel.Password);
                            user.Password = secretPassword;
                            obj.Users.Add(user);
                            obj.SaveChanges();
                            return Ok(true);
                        }
                    }
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