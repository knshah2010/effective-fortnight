using Framework.Controllers;
using Framework.Extension;
using Microsoft.AspNetCore.Mvc;
using DataExchange.Areas.Service.BAL;
using Models;

namespace DataExchange.Areas.Service.Controllers
{
    [Area("Service")]
    [ApiVersion("1.0")]
    [Route("{v:apiVersion}/Service/[controller]")]
    public class UsersController :BaseController
    {
        public UsersBal _UsersBal;

        [HttpPost]
        [Route("mobile_login")]
        public IActionResult MobileLogin([FromBody] object data)
        {
            _UsersBal = new UsersBal();
            dynamic token = _UsersBal.MobileLogin(data.ParseRequest<Users>());
            if (token.GetType().ToString() == "System.String")
            {
                Response.Headers["authorization"] = token;
                return new CustomResult();
            }
            return token;
        }
    }
}
