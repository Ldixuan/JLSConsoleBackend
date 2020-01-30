using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JLSDataAccess.Interfaces;
using JLSDataModel.Models.Product;
using JLSMobileApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace JLSMobileApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        [HttpPost("save")]
        public async Task<JsonResult> SaveProduct([FromForm]IFormCollection productData)
        {
            StringValues productInfo;
            productData.TryGetValue("product", out productInfo);
            Product product = JsonConvert.DeserializeObject<Product>(productInfo);
            IFormFile image = (IFormFile) productData.Files[0];

            int res = await this._productRepository.saveProduct(product, image);
            ApiResult result = new ApiResult() { Success = true, Msg = "OK", Type = "200" };
            return Json(result);
        }

    }

    
}