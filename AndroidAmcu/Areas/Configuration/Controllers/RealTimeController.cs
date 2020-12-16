using AndroidAmcu.Areas.Configuration.BAL;
using AndroidAmcu.Areas.Configuration.Models;
using AndroidAmcu.Areas.General.Models;
using Framework.Controllers;
using Framework.Extension;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Collections.Generic;
using System.Text.Json;

namespace AndroidAmcu.Areas.Configuration.Controllers
{
    [Area("Configuration")]
    [Route("{v:apiVersion}/realtime-services/")]
    public class RealTimeController : BaseController
    {
        [HttpPost]
        [Route("member-download")]
        public IActionResult Member([FromBody] object data)
        {
            RealTimeBal _bal = new RealTimeBal();
            ListEngine _listEngine = new ListEngine();
            SetParam();
            List<TmpMember> list = _listEngine.List<TmpMember>(_bal.PrepareData(data.ParseRequestWithoutData<RequestFormat1>()), FileName, "member");
            return new CustomResult2(list);
        }
        //[HttpPost]
        //[Route("purchase-rate")]
        //public IActionResult PurchaseRate([FromBody] object data)
        //{
        //    RealTimeBal _bal = new RealTimeBal();
           
        //}
        [HttpPost]
        [Route("purchase-rate-detail")]
        public IActionResult PurchaseRateDetail([FromBody] object data)
        {
            RealTimeBal _bal = new RealTimeBal(data.ParseRequestWithoutData<RequestFormat1>());
            return _bal.GetRateDetail();
        }
        [HttpPost]
        [Route("download-acknowledgement")] 
        public IActionResult Acknowledgement([FromBody] object data)
        {
            RealTimeBal _bal = new RealTimeBal(data.ParseRequestWithoutData<RequestFormat1>());
            return _bal.UpdateAck();
        }
        [HttpPost]
        [Route("rate-download-acknowledgement")]
        public IActionResult RateAcknowledgement([FromBody] object data)
        {
            RealTimeBal _bal = new RealTimeBal(data.ParseRequestWithoutData<RequestFormat1>());
            return _bal.UpdateRateAck();
        }
    }
}
