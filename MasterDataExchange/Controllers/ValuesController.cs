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
using Framework.DataAccess.Dapper;
using Framework.Library.Helper;
using DataExchange.Areas.Service.Utility;

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
            return listEngine.List(JObject.FromObject(data), "/Config/List.json", "test");
        }

        [HttpGet]
        [Route("csv")]
        public IActionResult CSV()
        {
            DBRepository repo = new DBRepository();
            string tablename = NumericHelper.RandomNumber().ToString()+"tmp";
            repo.Add(new QueryParam{
                DirectQuery = $"create table {tablename}(id int not null AUTO_INCREMENT PRIMARY KEY,fat decimal(18,2),snf decimal(18,2),milk_type varchar(5),rtpl decimal(18,2),milk_type_code int null); LOAD DATA LOCAL INFILE  'O:/Roshani/Projects/DataExchangeAPI/FileServer/Import/Upload/rate_chart_comma_sqparated.csv' INTO TABLE {tablename} FIELDS TERMINATED BY ';'  IGNORE 1 ROWS (fat, snf,milk_type,rtpl); "
            });
            return null;
        }

        [HttpGet]
        [Route("consume")]
        public void consume()
        {
            ConsumeApi _api=new ConsumeApi("rmrd_collection");
            _api.Call();
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
