using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FreeShareAPI.Controllers.Base
{
    public class BaseController<DataManager> : ApiController
        where DataManager:class, new()
    {
        protected readonly DataManager dataManager;

        public BaseController() : base()
        {
            dataManager = new DataManager();
        }

    }
}
