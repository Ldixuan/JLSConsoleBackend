using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JLSDataAccess.Interfaces;
using JLSDataModel.Models;
using JLSDataModel.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace JLSConsoleApplication.Controllers
{
    [Route("api/[controller]/{action}/{id?}")]
    [ApiController]
    public class ReferenceController : Controller
    {
        private readonly IReferenceRepository _referenceRepository;

        public ReferenceController(IReferenceRepository referenceRepository)
        {
            _referenceRepository = referenceRepository;
        }
        [HttpGet]
        public async Task<JsonResult> GetAllReferenceItems(int intervalCount, int size, string orderActive, string orderDirection, string filter)
        {
            ApiResult result;
            try
            {
                List<ReferenceItemViewModel> data = await _referenceRepository.GetReferenceItemWithInterval(intervalCount, size, orderActive, orderDirection, filter);
                result = new ApiResult() { Success = true, Msg = "OK", Type = "200", Data = data  };
            }
            catch (Exception e)
            {
                result = new ApiResult() { Success = false, Msg = e.Message, Type = "500" };
            }

            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetAllReferenceCategory()
        {
            ApiResult result;
            try
            {
                List<ReferenceCategory> data = await _referenceRepository.GetAllReferenceCategory();
                result = new ApiResult() { Success = true, Msg = "OK", Type = "200", Data = data };
            }
            catch (Exception e)
            {
                result = new ApiResult() { Success = false, Msg = e.Message, Type = "500" };
            }

            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetAllValidityReferenceCategory()
        {
            ApiResult result;
            try
            {
                List<ReferenceCategory> data = await _referenceRepository.GetAllValidityReferenceCategory();
                result = new ApiResult() { Success = true, Msg = "OK", Type = "200", Data = data };
            }
            catch (Exception e)
            {
                result = new ApiResult() { Success = false, Msg = e.Message, Type = "500" };
            }

            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetReferenceItemsCount()
        {
            ApiResult result;
            try
            {
                int data = await _referenceRepository.GetReferenceItemsCount();
                result = new ApiResult() { Success = true, Msg = "OK", Type = "200", Data = data };
            }
            catch (Exception e)
            {
                result = new ApiResult() { Success = false, Msg = e.Message, Type = "500" };
            }

            return Json(result);
        }


        [HttpPost]
        public async Task<JsonResult> CreatorUpdateReferenceItem([FromForm]IFormCollection itemData)
        {
            StringValues itemInfo;
            StringValues langLabelInfo;

            itemData.TryGetValue("item", out itemInfo);
            itemData.TryGetValue("langLabel", out langLabelInfo);

            ReferenceItem item = JsonConvert.DeserializeObject<ReferenceItem>(itemInfo);
            List<ReferenceLabel> langLabels = JsonConvert.DeserializeObject<List<ReferenceLabel>>(langLabelInfo);

            int res = await this._referenceRepository.CreatorUpdateItem(item, langLabels);
            ApiResult result = new ApiResult() { Success = true, Msg = "OK", Type = "200" };
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> CreatorUpdateReferenceCategory([FromBody]ReferenceCategory category)
        {
            int res = await this._referenceRepository.CreatorUpdateCategory(category);
            ApiResult result = new ApiResult() { Success = true, Msg = "OK", Type = "200" };
            return Json(result);
        }
    }
}