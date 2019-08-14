using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeShareAPI.Interface
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="EntityModel"></typeparam>
    /// <typeparam name="ViewModel"></typeparam>
    public interface IConverter<EntityModel, ViewModel>
        where EntityModel : class
        where ViewModel : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        EntityModel ConvertToEntity(ViewModel self);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        ViewModel ConvertToModel(EntityModel self);
    }

}
