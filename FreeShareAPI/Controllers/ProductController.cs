using FreeShareAPI.Common;
using FreeShareAPI.Controllers.Base;
using FreeShareAPI.CustomAttribute;
using FreeShareAPI.Models;
using FreeShareAPI.Models.Dbmodel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
//using System.Web.Mvc;

namespace FreeShareAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>

    [RoleAuthorize]
    [RoutePrefix("Api/Product")]
    public class ProductController : BaseApiController
    {
        private Security security;
        /// <summary>
        /// 
        /// </summary>
        public ProductController()
        {
            security = new Security();
        }

        /// <summary>
        /// Get all the product details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllProductDetails")]
        public IHttpActionResult GetAllProduct()
        {
            try
            {
                using (FreeShareEntities obj = new FreeShareEntities())
                {
                    List<Product> product = new List<Product>();
                    product = obj.Products.ToList();
                    return Ok(product);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        /// <summary>
        /// Insert the product details
        /// </summary>
        /// <param name="productModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertProductDetails")]
        public IHttpActionResult InsertProduct([FromBody] ProductModel productModel)
        {
            bool result = false;
            try
            {
                if (productModel != null)
                {
                    using (FreeShareEntities obj = new FreeShareEntities())
                    {
                        Product productobj = new Product();
                        productobj.ProductName = productModel.ProductName;
                        productobj.Deleted = productModel.Deleted;
                        obj.Products.Add(productobj);
                        obj.SaveChanges();
                        result = true;
                        return Ok(result);
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
            return BadRequest();
        }

        /// <summary>
        /// Delete product by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteProduct/{id}")]
        public IHttpActionResult DeleteProduct(int id)
        {
            bool result = false;
            try
            {
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
                            return Ok(result);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
            return BadRequest();

        }

        /// <summary>
        /// Get product details by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetProductById/{id}")]
        public IHttpActionResult GetProductById(int id)
        {
            try
            {
                if (id != 0)
                {
                    using (FreeShareEntities obj = new FreeShareEntities())
                    {
                        Product product = new Product();
                        product = obj.Products.FirstOrDefault(x => x.ProductId == id);
                        if (product != null)
                            return Ok(product);
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
            return BadRequest();
        }


        /// <summary>
        /// Update product details by Id
        /// </summary>
        /// <param name="productModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateProductDetails")]
        public IHttpActionResult UpdateProduct([FromBody] ProductModel productModel)
        {
            try
            {
                bool result = false;
                if (productModel.ProductId != 0 && !string.IsNullOrEmpty(productModel.ProductName))
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
                            return Ok(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
            return BadRequest();
        }

        //[HttpPost]
        //[Route("UploadImage")]
        //public IHttpActionResult UploadImage()
        //{
        //    //if (Request.Headers.Contains("Origin"))
        //    //{
        //    //    var values = Request.Headers.GetValues("Origin");
        //    //    // Do stuff with the values... probably .FirstOrDefault()
        //    //}
        //    HttpRequest Request = HttpContext.Current.Request;
        //    try
        //    {
        //        if (Request.Files.Count > 0)
        //        {
        //            using (FreeShareEntities obj = new FreeShareEntities())
        //            {
        //                foreach (string file in Request.Files)
        //                {
        //                    var postedFile = Request.Files[file];
        //                    var filePath = HttpContext.Current.Server.MapPath("~/Images/" + postedFile.FileName);
        //                    postedFile.SaveAs(filePath);

        //                    ImageDemo image = new ImageDemo();
        //                    image.Name = Request["username"];
        //                    image.Image = postedFile.FileName;
        //                    obj.ImageDemoes.Add(image);
        //                    obj.SaveChanges();
        //                }
        //            }
        //            return Ok(true);
        //        }
        //    }
        //    catch (DbEntityValidationException dbEx)
        //    {
        //        foreach (var validationErrors in dbEx.EntityValidationErrors)
        //        {
        //            foreach (var validationError in validationErrors.ValidationErrors)
        //            {
        //                Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
        //            }
        //        }
        //    }
        //    return NotFound();
        //}

        //public string ImageToByteArray(string imageData)
        //{
        //    string image = string.Empty;
        //    if (!String.IsNullOrEmpty(imageData))
        //    {
        //        using (var ms = new MemoryStream())
        //        {
        //            Image imageIn = Image.FromFile(HttpContext.Current.Server.MapPath("~/Images/" + imageData));
        //            imageIn.Save(ms, imageIn.RawFormat);
        //            return "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
        //        }
        //    }
        //    return image;

        //}

        //[HttpGet]
        //[Route("GetImageById/{id}")]
        //public IHttpActionResult GetImageById(int id)
        //{
        //    using (FreeShareEntities obj = new FreeShareEntities())
        //    {
        //        if (id != 0)
        //        {
        //            ImageDemo imageData = obj.ImageDemoes.FirstOrDefault(x => x.Id == id);
        //            if (imageData != null)
        //            {
        //                ImageDemo image = new ImageDemo();
        //                image.Name = imageData.Name;
        //                image.Image = ImageToByteArray(imageData.Image);
        //                return Ok(image);
        //            }
        //        }
        //    }
        //    return NotFound();
        //}

    }
}
