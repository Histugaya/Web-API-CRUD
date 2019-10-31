using FreeShareAPI.Interface;
using FreeShareAPI.Models;
using FreeShareAPI.Models.Dbmodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreeShareAPI.Converter
{
    public class ProductConverter : IConverter<Product, ProductModel>
    {
        public Product ConvertToEntity(ProductModel self)
        {
            Product model = new Product();
            if (self != null)
            {
                model.Id = self.ProductId;
                model.ProductName = self.ProductName;
                model.Preview = self.Deleted;
            }
            return model;
        }

        public ProductModel ConvertToModel(Product self)
        {
            ProductModel model = new ProductModel();
            if (self != null)
            {
                model.ProductId = self.Id;
                model.ProductName = self.ProductName;
                model.Deleted = self.Preview;
            }
            return model;
        }
    }
}