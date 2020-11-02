using Microsoft.AspNetCore.Mvc;
using Framework.BAL;
using Framework.CustomDataType;
using Framework.DataAccess.Dapper;
using System;
using System.Collections.Generic;
using Test.Areas.Employee.Models;
using Framework.Library.Helper;

namespace Test.Areas.Employee.BAL
{
    public class EmployeeBal : BaseBal<EmployeeMaster>
    {
        private List<ModelParameter> Data;

        public EmployeeBal(EmployeeMaster EmployeeModel)
        {
            Data = new List<ModelParameter>
            {
                new ModelParameter { ValidateModel = new EmployeeMasterValidator(), SaveModel = EmployeeModel }
            };
        }

        public IActionResult Save()
        {
            return SaveData(Data);
        }
    }
}
