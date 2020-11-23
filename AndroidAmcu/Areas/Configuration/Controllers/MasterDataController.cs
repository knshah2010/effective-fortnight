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
    [Route("{v:apiVersion}/master-data/")]
    public class MasterDataController : BaseController
    {
        [HttpPost]
        [Route("sentbox")]
        public IActionResult SentBox([FromBody] object data)
        {
            MasterDataBal _bal = new MasterDataBal();
            ListEngine _listEngine = new ListEngine();
            SetParam();
            List<Sentbox> list=_listEngine.List<Sentbox>(_bal.PrepareData(data.ParseRequestWithoutData<RequestFormat1>()), FileName, "sentbox");
            return new CustomResult2(list);
        }

        [HttpPost]
        [Route("inbox")]
        public IActionResult Inbox([FromBody] object data)
        {
            MasterDataBal _bal = new MasterDataBal();
            return _bal.SaveInbox(data.ParseRequestWithoutData<RequestFormat3>());
        }
    }
}
