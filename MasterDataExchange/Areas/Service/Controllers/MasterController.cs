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
    public class MasterController : BaseController
    {
        [HttpPost]
        [Route("bmc")]
        public IActionResult Bmc([FromBody]object data)
        {
            MasterBal _bal = new MasterBal();
            return _bal.SaveBmc(data.ParseRequestList<Bmc>());
        }

        [HttpGet]
        [Route("bmclist")]
        public IActionResult BmcList()
        {
            SearchParam Param = new SearchParam()
            {
            };
            dynamic data = new ExpandoObject();
            data.data = Param;
            ListEngine _listEngine = new ListEngine();
            SetParam();
            return _listEngine.List(JObject.FromObject(data), FileName, "bmc");
        }

        [HttpPost]
        [Route("route")]
        public IActionResult Route([FromBody]object data)
        {
            MasterBal _bal = new MasterBal();
            return _bal.SaveRoute(data.ParseRequestList<Route>());
        }

        [HttpGet]
        [Route("routelist")]
        public IActionResult RouteList(string bmc_code)
        {
            SearchParam Param = new SearchParam()
            {
                Param = new List<ConditionParameter>()
                {
                     new ConditionParameter{PropertyName="bmc_code",PropertyValue=bmc_code},
                }
            };
            dynamic data = new ExpandoObject();
            data.data = Param;
            ListEngine _listEngine = new ListEngine();
            SetParam();
            return _listEngine.List(JObject.FromObject(data), FileName, "route");
        }

        [HttpPost]
        [Route("dcs")]
        public IActionResult Dcs([FromBody] object data)
        {
            MasterBal _bal = new MasterBal();
            return _bal.SaveMpp(data.ParseRequestList<Dcs>());
        }

        [HttpGet]
        [Route("dcslist")]
        public IActionResult DcsList(string bmc_code)
        {
            SearchParam Param = new SearchParam()
            {
                Param = new List<ConditionParameter>()
                {
                     new ConditionParameter{PropertyName="d.bmc_code",PropertyValue=bmc_code},
                }
            };
            dynamic data = new ExpandoObject();
            data.data = Param;
            ListEngine _listEngine = new ListEngine();
            SetParam();
            return _listEngine.List(JObject.FromObject(data), FileName, "dcs");
        }

        [HttpPost]
        [Route("member")]
        public IActionResult Member([FromBody] object data)
        {
            MasterBal _bal = new MasterBal();
            return _bal.SaveMember(data.ParseRequestList<Member>());
        }

        [HttpGet]
        [Route("memberlist")]
        public IActionResult MemberList(string dcs_code)
        {
            SearchParam Param = new SearchParam()
            {
                Param = new List<ConditionParameter>()
                {
                     new ConditionParameter{PropertyName="m.dcs_code",PropertyValue=dcs_code},
                }
            };
            dynamic data = new ExpandoObject();
            data.data = Param;
            ListEngine _listEngine = new ListEngine();
            SetParam();
            return _listEngine.List(JObject.FromObject(data), FileName, "member");
        }

        [HttpPost]
        [Route("customer")]
        public IActionResult Customer([FromBody] object data)
        {
            MasterBal _bal = new MasterBal();
            return _bal.SaveCustomer(data.ParseRequestList<CustomerMaster>());
        }

        [HttpGet]
        [Route("customerlist")]
        public IActionResult CustomerList(string bmc_code)
        {
            SearchParam Param = new SearchParam()
            {
                Param = new List<ConditionParameter>()
                {
                     new ConditionParameter{PropertyName="c.bmc_code",PropertyValue=bmc_code},
                }
            };
            dynamic data = new ExpandoObject();
            data.data = Param;
            ListEngine _listEngine = new ListEngine();
            SetParam();
            return _listEngine.List(JObject.FromObject(data), FileName, "customer");
        }

        [HttpPost]
        [Route("rate_applicability")]
        public IActionResult RateApplicability([FromBody] object data)
        {
            MasterBal _bal = new MasterBal();
            return _bal.PurchaseRateApplicability(data.ParseRequestList<PurchaseRateApplicability>());
        }


        [HttpPost]
        [Route("rate")]
        public IActionResult Create([FromForm] IFormFile File,[FromForm]int rate_type=0)
        {
            MasterBal _bal = new MasterBal();
            return _bal.Upload(File, rate_type);
        }
    }
}
