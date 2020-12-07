using AndroidAmcu.Areas.Configuration.BAL;
using Framework.Controllers;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace AndroidAmcu.Areas.Configuration.Controllers
{
    [Area("Configuration")]
    [Route("{v:apiVersion}/inbox/")]
    public class InboxController : BaseController
    {
        [HttpGet]
        [Route("inbox-parse")]
        public void ParseInbox()
        {
            InboxParserBal _bal = new InboxParserBal();
            _bal.ParseInbox();
        }
    }
}
