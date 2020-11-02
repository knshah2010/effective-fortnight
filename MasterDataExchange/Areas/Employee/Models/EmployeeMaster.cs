using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;
using Framework.Library.Attribute;
using Framework.Models;
using System;
using System.Collections.Generic;

namespace Test.Areas.Employee.Models
{
    [Table("employee_master")]
    public class EmployeeMaster : BaseModel
    {
        [Key]
        [ExplicitKey]
        public int employee_code { get; set; }
        public string employee_name { get; set; }
        public string employee_lastname { get; set; }
    }

    public class EmployeeMasterValidator : AbstractValidator<EmployeeMaster>
    {
        public EmployeeMasterValidator()
        {
            RuleFor(e => e.employee_name).Require();
        }
    }
}
