using FreeShareAPI.Models;
using FreeShareAPI.Models.Dbmodel;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web;

namespace FreeShareAPI.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class Security
    {

        private string secretkey;
        /// <summary>
        /// 
        /// </summary>
        public Security()
        {
            secretkey = ConfigurationManager.AppSettings["Secret"];
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
       /// 
       /// </summary>
       /// <param name="userModel"></param>
       /// <returns></returns>
        public bool CheckUser(UserModel userModel)
        {
            bool result = false;
            string decodePassword;
            if (userModel != null)
            {
                using (FreeShareEntities obj = new FreeShareEntities())
                {
                    decodePassword = HashPassword(userModel.Password);
                    return obj.Users.Any(x => x.Username == userModel.Username
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
        public string GenerateToken(string username, bool admin, int expireMinutes = 20)
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

            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new[]
            //            {
            //            new Claim(ClaimTypes.Name, username),
            //        }),

            //    Expires = now.AddMinutes(Convert.ToInt32(expireMinutes)),

            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            //};

            var secToken = new JwtSecurityToken(
                 issuer: "",
                 audience: "",
                 claims: null,
                 expires: expires,
                 signingCredentials: credentials
                //header, payload
                );
            secToken.Payload["username"] = username;
            secToken.Payload["admin"] = admin;
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