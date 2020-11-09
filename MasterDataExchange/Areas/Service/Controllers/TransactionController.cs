using DataExchange.Areas.Service.BAL;
using Framework.Controllers;
using Framework.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace DataExchange.Areas.Service.Controllers
{
    [Area("Service")]
    [Route("{v:apiVersion}/Service/[controller]")]
    public class TransactionController : BaseController
    {

        [HttpPost]
        [Route("list")]
        public IActionResult List([FromBody] object data)
        {
            ListEngine _listEngine = new ListEngine();
            SetParam();
            return _listEngine.List(data, FileName, "milk_collection");
        }
    }
}
