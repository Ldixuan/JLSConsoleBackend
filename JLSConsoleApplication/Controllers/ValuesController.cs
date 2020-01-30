using JLSDataAccess;
using Microsoft.AspNetCore.Mvc;

namespace JLSMobileApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : Controller
    {
        private readonly JlsDbContext db;
        public ValuesController(JlsDbContext jlsDbContext)
        {
            db = jlsDbContext;
        }
        // GET api/values
        [HttpGet]
        public JsonResult  Get()
        {

            var result = db.ReferenceCategory;
            return Json(result);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
