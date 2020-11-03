using Framework.Controllers;
using Framework.Extension;
using Test.Areas.Employee.BAL;
using Test.Areas.Employee.Models;
using Microsoft.AspNetCore.Mvc;

namespace Test.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Route("Employee/[controller]")]
    public class EmployeeController : BaseController
    {
        private EmployeeBal _EmployeeBal;

        [HttpPost]
        [Route("create")]
        public IActionResult Create([FromBody] object data)
        {
            _EmployeeBal = new EmployeeBal(data.ParseRequest<EmployeeMaster>());
            return _EmployeeBal.Save();
        }
    }
}
