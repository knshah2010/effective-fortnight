using AndroidAmcu.Areas.Configuration.BAL;
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
        [HttpGet]
        [Route("Generate")]
        public IActionResult Generate()
        {
            IdentityBal _bal = new IdentityBal();
            return new CustomResult("success");
        }
        
            [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] object data)
        {
            IdentityBal _bal = new IdentityBal();
            return new CustomResult("success");
        }
    }
}
