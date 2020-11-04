using DataExchange.Areas.Service.BAL;
using Framework.Controllers;
using Framework.Extension;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace DataExchange.Areas.Service.Controllers
{
    [Area("Service")]   
    [Route("{v:apiVersion}/Service/[controller]")]
    public class MasterController : BaseController
    {
        [HttpPost]
        [Route("bmc")]
        public IActionResult Bmc([FromBody]object data)
        {
            MasterBal _bal = new MasterBal();
            return _bal.SaveBmc(data.ParseRequestList<Bmc>());
        }
        [HttpPost]
        [Route("route")]
        public IActionResult Route([FromBody]object data)
        {
            MasterBal _bal = new MasterBal();
            return _bal.SaveRoute(data.ParseRequestList<Route>());
        }
    }
}
