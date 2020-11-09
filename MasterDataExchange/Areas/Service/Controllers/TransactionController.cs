using DataExchange.Areas.Service.BAL;
using Framework.Controllers;
using Framework.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Collections.Generic;

namespace DataExchange.Areas.Service.Controllers
{
    [Area("Service")]
    [Route("{v:apiVersion}/Service/[controller]")]
    public class TransactionController : BaseController
    {

        [HttpPost]
        [Route("milk_collection")]
        public IActionResult List([FromBody] object data)
        {
            ListEngine _listEngine = new ListEngine();
            SetParam();
            return _listEngine.List(data, FileName, "milk_collection");
        }

        [HttpPost]
        [Route("milkcollection_ack")]
        public IActionResult Ack([FromBody] object data)
        {
            TransactionBal _bal = new TransactionBal();
            return _bal.SaveMilkAck(data.ParseRequestList<string>());
        }
        [HttpPost]
        [Route("bmccollection_ack")]
        public IActionResult BmcAck([FromBody] object data)
        {
            TransactionBal _bal = new TransactionBal();
            return _bal.SaveMilkAck(data.ParseRequestList<string>());
        }

    }
}
