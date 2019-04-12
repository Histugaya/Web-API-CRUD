using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreeShareAPI.Models.Dbmodel
{
    public class ProductModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int Deleted { get; set; }
    }
}