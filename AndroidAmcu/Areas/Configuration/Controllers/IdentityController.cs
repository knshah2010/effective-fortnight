using AndroidAmcu.Areas.Configuration.BAL;
using AndroidAmcu.Areas.General.Models;
using Framework.Controllers;
using Framework.Extension;
using Microsoft.AspNetCore.Mvc;
using Models;


namespace AndroidAmcu.Areas.Configuration.Controllers
{
    [Area("Configuration")]
    [Route("{v:apiVersion}/android-dpu/")]
    public class IdentityController : BaseController
    {
        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] object data)
        {
            IdentityBal _bal = new IdentityBal(data.ParseRequestWithoutData<RequestFormat1>());
            return _bal.Registration();
        }        
        [HttpPost]
        [Route("verification")]
        public IActionResult Verification([FromBody] object data)
        {
            IdentityBal _bal = new IdentityBal(data.ParseRequestWithoutData<RequestFormat1>());
            return _bal.Verification();
        }
        [HttpPost]
        [Route("initialization")]
        public IActionResult Generate([FromBody] object data)
        {
            IdentityBal _bal = new IdentityBal(data.ParseRequestWithoutData<RequestFormat1>());
            return _bal.Generate(Request.Host.Value);
        }
    }
}
