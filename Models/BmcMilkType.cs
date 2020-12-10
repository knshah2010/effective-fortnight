using Framework.Models;
using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;
using System;
using Framework.Library.Helper;

namespace Models
{
    [Table("tbl_bmc_milk_type")]
    public class BmcMilkType : BaseModel
    {
        [ExplicitKey]
        public string bmc_code { get; set; }
        public int milk_type_code { get; set; }
        public bool is_active { get; set; } = true;
        [Computed]
        public new string flg_sentbox_entry { get; set; } = "N";
        public string originating_org_type { get; set; } = "portal";
        [Computed]
        public string milk_type_name { get; set; } = "N";
    }

    public class BmcMilkTypeValidator : AbstractValidator<BmcMilkType>
    {
        public BmcMilkTypeValidator()
        {
            RuleFor(d => d.bmc_code).Require();
            RuleFor(d => d.milk_type_code).Require();
            RuleFor(d => d.is_active).Require();
        }

    }
}
