using FreeShareAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FreeShareAPI.Controllers
{
    [RoutePrefix("Api/Product")]
    public class ProductController : ApiController
    {

        [HttpGet]
        [Route("GetAllProductDetails")]
        public List<Product> GetAllProduct()
        {
            using (FreeShareEntities obj = new FreeShareEntities())
            {
                List<Product> product = new List<Product>();
                product = obj.Products.ToList();
                return product;
            }
        }

        [HttpPost]
        [Route("InsertProductDetails")]
        public bool InsertProduct(string productName)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(productName))
            //if(product!=null)
            {
                using (FreeShareEntities obj = new FreeShareEntities())
                {
                    Product productobj = new Product();
                    productobj.ProductName = productName;
                    obj.Products.Add(productobj);
                    obj.SaveChanges();
                    result = true;
                }
            }
            return result;
        }

        [HttpDelete]
        [Route("DeleteProduct")]
        public bool DeleteProduct(int id)
        {
            bool result = false;
            if (id != 0)
            {
                using (FreeShareEntities obj = new FreeShareEntities())
                {
                    Product product = obj.Products.FirstOrDefault(x => x.ProductId == id);
                    if (product != null)
                    {
                        obj.Products.Remove(product);
                        obj.SaveChanges();
                        result = true;
                    }
                }
            }
            return result;
        }

        [HttpPut]
        [Route("UpdateProductDetails")]
        public bool UpdateProduct(int id, string productName)
        {
            bool result = false;
            if (id != 0 && !string.IsNullOrEmpty(productName))
            {
                using (FreeShareEntities obj = new FreeShareEntities())
                {
                    Product product = obj.Products.FirstOrDefault(x => x.ProductId == id);
                    if (product != null)
                    {
                        product.ProductName = productName;
                        obj.SaveChanges();
                        result = true;
                    }
                }
            }
            return result;
        }
    }
}
