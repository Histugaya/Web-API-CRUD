using System.Web.Http;

namespace FreeShareAPI.Controllers.Base
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="DataManager"></typeparam>
    public class BaseController<DataManager> : ApiController
        where DataManager:class, new()
    {
        protected readonly DataManager dataManager;

        /// <summary>
        /// default constructor
        /// </summary>
        public BaseController() : base()
        {
            dataManager = new DataManager();
        }

    }
}
