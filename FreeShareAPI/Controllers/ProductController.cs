using FreeShareAPI.Common;
using FreeShareAPI.Controllers.Base;
using FreeShareAPI.CustomAttribute;
using FreeShareAPI.DataManager;
using FreeShareAPI.Interface;
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

namespace FreeShareAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>

    [RoleAuthorize]
    [RoutePrefix("Api/Product")]
    public class ProductController : BaseController<ProductDataManager>,IBaseController<ProductModel>
    {

        /// <summary>
        /// Get all the product details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllProductDetails")]
        public IHttpActionResult GetAll()
        {
            try
            {
                List<ProductModel> model = new List<ProductModel>();
                model=dataManager.GetAll();        
                return Ok(model);
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
        public IHttpActionResult Create([FromBody] ProductModel productModel)
        {
            try
            {
                dataManager.Add(productModel);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        /// <summary>
        /// Delete product by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteProduct/{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
              bool response=dataManager.Delete(id);
                if (response)
                {
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        /// <summary>
        /// Update product details by Id
        /// </summary>
        /// <param name="productModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateProductDetails")]
        public IHttpActionResult Edit([FromBody] ProductModel productModel)
        {
            try
            {
                dataManager.Edit(productModel);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        /// <summary>
        /// Get product details by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetProductById/{id}")]
        public IHttpActionResult GetByID(int id)
        {
            try
            {
                ProductModel model = new ProductModel();
                model=dataManager.GetByID(id);
                if (model != null)
                {
                    return Ok(model);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
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
