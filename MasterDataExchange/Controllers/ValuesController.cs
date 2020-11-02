using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework.Controllers;
using Microsoft.AspNetCore.Mvc;
using Framework.Extension;
using Framework.CustomDataType;
using System.Dynamic;
using Newtonsoft.Json.Linq;

namespace Test.Controllers
{
    [Route("test_api/[controller]")]
    
    public class ValuesController : BaseController
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("test")]
        public IActionResult test()
        {
            SearchParam search = new SearchParam
            {               
            };
            
            dynamic data = new ExpandoObject();
            data.data = search;

            ListEngine listEngine = new ListEngine();
            SetParam();
            return listEngine.List(JObject.FromObject(data), "\\Config\\List.json", "test");
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
