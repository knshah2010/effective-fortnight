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

        [HttpPost]
        [Route("dcs")]
        public IActionResult Dcs([FromBody] object data)
        {
            MasterBal _bal = new MasterBal();
            return _bal.SaveMpp(data.ParseRequestList<Dcs>());
        }

        [HttpPost]
        [Route("member")]
        public IActionResult Member([FromBody] object data)
        {
            MasterBal _bal = new MasterBal();
            return _bal.SaveMember(data.ParseRequestList<Member>());
        }

        [HttpPost]
        [Route("customer")]
        public IActionResult Customer([FromBody] object data)
        {
            MasterBal _bal = new MasterBal();
            return _bal.SaveCustomer(data.ParseRequestList<CustomerMaster>());
        }
              

        [HttpPost]
        [Route("rate")]
        public IActionResult Create([FromForm] IFormFile File)
        {
            MasterBal _bal = new MasterBal();
            return _bal.Upload(File);
        }
    }
}
