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

namespace FreeShareAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("Api/Token")]
    public class TokenController : ApiController
    {
        private string secretkey;
        /// <summary>
        /// 
        /// </summary>
        public TokenController()
        {
            secretkey = ConfigurationManager.AppSettings["Secret"];
        }

        /// <summary>
        /// Authenticate user with given credentials
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        //public IHttpActionResult login([FromBody] UserModel userModel)
        public IHttpActionResult login(string username, string password)

        {
            if (CheckUser(username,password))
            {
                bool admin = false;
                string token = GenerateToken(username, admin);
                return Ok(token);
            }
            return Ok("login failed");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool CheckUser(string username, string password)
        {
            bool result = false;
            string decodePassword;
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                using (FreeShareEntities obj = new FreeShareEntities())
                {
                    decodePassword = HashPassword(password);
                    return obj.Users.Any(x => x.Username == username
                                                         && x.Password == decodePassword);
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="admin"></param>
        /// <param name="expireMinutes"></param>
        /// <returns></returns>
        public string GenerateToken(string username, bool admin, int expireMinutes = 2)
        {
            DateTime now = DateTime.UtcNow;
            DateTime expires = now.AddMinutes(Convert.ToInt32(expireMinutes));
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //JwtHeader header = new JwtHeader(credentials);

         //   var payload = new JwtPayload {
         //       {"username ", username},
         //       {"admin", admin},
         //       {"expiryDate", expires}
         //};

            var secToken = new JwtSecurityToken(
                 issuer: "",
                 audience: "",
                 claims: null ,
                 expires: expires,
                 signingCredentials: credentials
                //header, payload
                );
            secToken.Payload["username"] = username;
            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(secToken);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="authToken"></param>
        /// <returns></returns>
        public bool ValidateToken(string authToken)
        {
            try
            {
                var result = false;
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = GetValidationParameters();
                var lifeTime = new JwtSecurityTokenHandler().ReadToken(authToken).ValidTo;
                if (lifeTime > DateTime.UtcNow)
                {
                    SecurityToken validatedToken;
                    IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
                    result = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = true, // Because there is no expiration in the generated token
                ValidateAudience = false, // Because there is no audiance in the generated token
                ValidateIssuer = false,   // Because there is no issuer in the generated token
                ValidIssuer = "Sample",
                ValidAudience = "Sample",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey)) // The same key as the one that generate the token
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string HashPassword(string password)
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
        /// Register a new user with given credentials
        /// </summary>
        [HttpPost]
        [Route("Register")]
        public IHttpActionResult Register(string username, string password)
        {
            string secretPassword;
            try
            {
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                        using (FreeShareEntities obj = new FreeShareEntities())
                        {
                            User user = new User();
                            user.Username = username;
                            secretPassword = HashPassword(password);
                            user.Password = secretPassword;
                            obj.Users.Add(user);
                            obj.SaveChanges();
                            return Ok(true);
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

        /// <summary>
        /// Get all the product details
        /// </summary>
        /// <returns></returns>
        
        [HttpGet]
        [Route("GetAllProductDetails")]
        [RoleAuthorize]
        public IHttpActionResult GetAllProduct()
        {
            if (CheckRequestHeader())
            {
                using (FreeShareEntities obj = new FreeShareEntities())
                {
                    List<Product> product = new List<Product>();
                    product = obj.Products.ToList();
                    return Ok(product);
                }
            }
            return Ok("token expired");
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CheckRequestHeader()
        {
            bool result = false;
            HttpContext httpContext = HttpContext.Current;
            string authHeader = httpContext.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("bearer", StringComparison.OrdinalIgnoreCase))
            {
                var tokenStr = authHeader.Substring("Bearer ".Length).Trim();
                result = ValidateToken(tokenStr);
            }
            return result;
        }

    }
}