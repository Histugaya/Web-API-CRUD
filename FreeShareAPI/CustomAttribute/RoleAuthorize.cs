using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FreeShareAPI.CustomAttribute
{
    /// <summary>
    /// 
    /// </summary>
    public class RoleAuthorize:AuthorizeAttribute
    {
        string secretkey = ConfigurationManager.AppSettings["Secret"];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            bool result = false;
            HttpContext httpContext = HttpContext.Current;
            string authHeader = httpContext.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("bearer", StringComparison.OrdinalIgnoreCase))
            {
                var tokenStr = authHeader.Substring("Bearer ".Length).Trim();
                result = ValidateToken(tokenStr);
            }
            if (result)
            {

            }
            else
            {

            }
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
    }
}