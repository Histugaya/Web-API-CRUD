using FreeShareAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace FreeShareAPI.CustomAttribute
{
    /// <summary>
    /// 
    /// </summary>
    public class RoleAuthorize : AuthorizeAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                bool response = false;
                response = new Security().CheckRequestHeader();
                if (!response)
                {
                    actionContext.Response =
                        actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid API call");
                }
            }
            catch (Exception ex)
            {
                actionContext.Response =
                                actionContext.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Server Error");
            }
        }
    }
}
