using FreeShareAPI.Models;
using FreeShareAPI.Models.Dbmodel;
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
        public IHttpActionResult GetAllProduct()
        {
            using (FreeShareEntities obj = new FreeShareEntities())
            {
                List<Product> product = new List<Product>();
                product = obj.Products.ToList();
                return Ok(product);
            }
        }

        [HttpPost]
        [Route("InsertProductDetails")]
        public IHttpActionResult InsertProduct([FromBody] ProductModel productModel )
        {
            bool result = false;
            if (!string.IsNullOrEmpty(productModel.ProductName))
            //if(product!=null)
            {
                using (FreeShareEntities obj = new FreeShareEntities())
                {
                    Product productobj = new Product();
                    productobj.ProductName = productModel.ProductName;
                    obj.Products.Add(productobj);
                    obj.SaveChanges();
                    result = true;
                }
            }
            return Ok(result);
        }

        [HttpDelete]
        [Route("DeleteProduct/{id}")]
        public IHttpActionResult DeleteProduct(int id)
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
            return Ok(result);
        }

        [HttpGet]
        [Route("GetProductById/{id}")]
        public IHttpActionResult GetProductById(int id)
        {
            using (FreeShareEntities obj = new FreeShareEntities())
            {
                Product product = new Product();
                product = obj.Products.FirstOrDefault(x => x.ProductId == id);
                return Ok(product);
            }
        }

        [HttpPut]
        [Route("UpdateProductDetails")]
        public IHttpActionResult UpdateProduct([FromBody] ProductModel productModel)
        {
            bool result = false;
            if ( productModel.ProductId!= 0 && !string.IsNullOrEmpty(productModel.ProductName))
            {
                using (FreeShareEntities obj = new FreeShareEntities())
                {
                    Product product = obj.Products.FirstOrDefault(x => x.ProductId == productModel.ProductId);
                    if (product != null)
                    {
                        product.ProductName = productModel.ProductName;
                        product.Deleted = productModel.Deleted;
                        obj.SaveChanges();
                        result = true;
                    }
                }
            }
            return Ok(result);
        }
    }
}
