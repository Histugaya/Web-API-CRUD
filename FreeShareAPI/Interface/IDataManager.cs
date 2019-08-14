using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeShareAPI.Interface
{
    public interface IDataManager<ViewModel>
        where ViewModel:class
    {
        void Add(ViewModel model);

        ViewModel GetByID(int id);

        void Edit(ViewModel model);

        void Delete(int id);

        List<ViewModel> GetAll();


    }
}
