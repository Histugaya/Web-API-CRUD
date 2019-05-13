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
                bool admin = true;
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
                    if (user!=null)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        public string GenerateToken(string username, bool admin, int expireMinutes = 2)
        {
            var symmetricKey = Convert.FromBase64String(secretkey);

            //string issuer, audience;

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            //SigningCredentials signingKey = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature);
            DateTime now = DateTime.UtcNow;
            DateTime expires = now.AddMinutes(Convert.ToInt32(expireMinutes));

            JwtSecurityToken token = new JwtSecurityToken();

            //JwtSecurityToken token = new JwtSecurityToken(issuer = null, audience = null,
            //                                              System.Collections.IEnumerable<Claim> claims = null, 
            //                                              now, expires, signingKey);
            token.Payload["username"] = username;
            token.Payload["admin"] = admin;
            token.Payload["expiryDate"] = expires;

            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new[]
            //            {
            //            new Claim(ClaimTypes.Name, username),
            //        }),

            //    Expires = expires,

            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            //};

            //var stoken = tokenHandler.CreateToken(token);
            var tokenSecure = tokenHandler.WriteToken(token);

            return tokenSecure;
        }

        //public bool validateToken(string token)
        //{
        //    bool result = false;

        //    var symmetricKey = Convert.FromBase64String(secretkey);
        //    SigningCredentials signingKey = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature);

        //    SecurityToken validatedToken;

        //    TokenValidationParameters validationParameters =
        //         new TokenValidationParameters
        //         {
        //             IssuerSigningKeys = signingKey;
        //         };
        //    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        //    ClaimsPrincipal principal = tokenHandler.ValidateToken(token, TokenValidationParameters,out validatedToken);
        //    return result;
        //}

        //public bool validateSignature(string token)
        //{
        //    string[] tokenValue = token.Split('.');
        //    string header = tokenValue[0];
        //    string data = tokenValue[1];

        //    bool result = false;

        //    SecretKeySpec secret = new SecretKeySpec(secretKey.getBytes(), "HmacSHA256");
        //    HMAC mac = HMAC.getInstance("HmacSHA256");
        //    mac.init(secret);

        //    String body = header + "." + data;
        //    byte[] hmacDataBytes = mac.doFinal(body.getBytes(StandardCharsets.UTF_8.name()));
        //    String hmacData = Base64.getUrlEncoder().encodeToString(hmacDataBytes);

        //    if (hmacData.equals(signature))
        //        result = true;

        //    return result;
        //}

        //public bool validateSignature(string token)
        //{
        //    var parts = token.Split('.');
        //    var header = parts[0];
        //    var payload = parts[1];
        //    byte[] crypto = Base64UrlDecode(parts[2]);

        //    var headerJson = Encoding.UTF8.GetString(Base64UrlDecode(header));
        //    var headerData = JObject.Parse(headerJson);
        //    var payloadJson = Encoding.UTF8.GetString(Base64UrlDecode(payload));
        //    var payloadData = JObject.Parse(payloadJson);

        //    if (verify)
        //    {
        //        var bytesToSign = Encoding.UTF8.GetBytes(string.Concat(header, ".", payload));
        //        var keyBytes = Encoding.UTF8.GetBytes(key);
        //        var algorithm = (string)headerData["alg"];

        //        var signature = HashAlgorithms[GetHashAlgorithm(algorithm)](keyBytes, bytesToSign);
        //        var decodedCrypto = Convert.ToBase64String(crypto);
        //        var decodedSignature = Convert.ToBase64String(signature);

        //        if (decodedCrypto != decodedSignature)
        //        {
        //            throw new ApplicationException(string.Format("Invalid signature. Expected {0} got {1}", decodedCrypto, decodedSignature));
        //        }
        //    }

        //    return payloadData.ToString();
        //}

      
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
