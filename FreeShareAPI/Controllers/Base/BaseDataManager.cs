using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreeShareAPI.Controllers.Base
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="Converter"></typeparam>
    public class BaseDataManager<Converter>
        where Converter:class, new()
    {
        protected readonly Converter converter;

        public BaseDataManager() : base()
        {
            converter = new Converter();
        }
    }
}