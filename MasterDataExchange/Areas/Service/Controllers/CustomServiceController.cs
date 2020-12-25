using DataExchange.Areas.Service.BAL;
using Framework.Controllers;
using Framework.CustomDataType;
using Framework.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Dynamic;

namespace DataExchange.Areas.Service.Controllers
{
    [Area("Service")]
    [Route("{v:apiVersion}/Service/[controller]")]
    public class CustomServiceController : BaseController
    {
        [HttpPost]
        [Route("farmer")]
        public IActionResult farmer([FromBody] object data)
        {
            CustomServiceBal _bal = new CustomServiceBal();
            return _bal.Savefarmer(data.ParseRequestList<Member>());
        }

        [HttpPost]
        [Route("rate_applicability")]
        public IActionResult RateApplicability([FromBody] object data)
        {
            CustomServiceBal _bal = new CustomServiceBal();
            return _bal.PurchaseRateApplicability(data.ParseRequestList<PurchaseRateApplicability>());
        }

    }
}
