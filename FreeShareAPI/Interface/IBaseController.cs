using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace FreeShareAPI.Interface
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="ViewModel"></typeparam>
    public interface IBaseController<ViewModel>
        where ViewModel : class

    {
        IHttpActionResult GetAll();
        IHttpActionResult Create(ViewModel model);
        IHttpActionResult GetByID(int id);
        IHttpActionResult Edit(ViewModel model);
        IHttpActionResult Delete(int id);
    }

}
