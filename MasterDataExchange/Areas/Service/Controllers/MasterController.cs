using Framework.Controllers;
using Framework.Extension;
using Microsoft.AspNetCore.Mvc;

namespace DataExchange.Areas.Service.Controllers
{
    [Area("Service")]
    [ApiVersion("1.0")]
    [Route("{v:apiVersion}/Service/[controller]")]
    public class MasterController : BaseController
    {
        [HttpPost]
        [Route("bmc")]
        public IActionResult Bmc([FromBody]object data)
        {
            ListEngine listEngine = new ListEngine();
            SetParam();
            return listEngine.List(data, FileName, "vlcc_shift_report");
        }
    }
}
