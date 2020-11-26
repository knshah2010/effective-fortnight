using Framework.Models;
using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;
using System;
using Framework.Library.Helper;

namespace Models
{
    [Table("tbl_dcs_milk_type")]
    public class DcsMilkType : BaseModel
    {
        [ExplicitKey]
        public string dcs_code { get; set; }
        public int milk_type_code { get; set; }
        public bool is_active { get; set; }
        public decimal rtpl { get; set; }
        [Computed]
        public new string flg_sentbox_entry { get; set; } = "N";
        public string originating_org_type { get; set; } = "portal";
    }

    public class DcsMilkTypeValidator : AbstractValidator<DcsMilkType>
    {
        public DcsMilkTypeValidator()
        {
            RuleFor(d => d.dcs_code).Require();
            RuleFor(d => d.milk_type_code).Require();
            RuleFor(d => d.is_active).Require();
        }

    }
}
