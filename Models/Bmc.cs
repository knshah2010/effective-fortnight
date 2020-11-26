using Framework.Models;
using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;
using System;
using Framework.Library.Helper;

namespace Models
{
    [Table("tbl_bmc")]
    public class Bmc : BaseModel
    {
        [ExplicitKey]
        public string bmc_code { get; set; }
        public string bmc_code_ex { get; set; }
        public string bmc_name { get; set; }
        public string mcc_plant_code { get; set; }
        public string plant_code { get; set; }
        public string ref_code { get; set; }
        public string model { get; set; }
        public int capacity { get; set; } = 0;
        public DateTime valid_from { get; set; } = DateHelper.CurrentDate();
        public string union_code { get; set; }
        public int bmc_type_code { get; set; } = 2;
        public int bmc_milk_type { get; set; } = 1;
        public int manufacturer_code { get; set; } = 1;
        public bool is_mcc { get; set; }
        public bool is_active { get; set; } = true;
        public string x_col1 { get; set; }
        public string x_col2 { get; set; }
        public string x_col3 { get; set; }
        public string x_col4 { get; set; }
        public string x_col5 { get; set; }
        public string bmc_incharge_name { get; set; }
        public bool is_weight_manual { get; set; } = true;
        public bool is_quality_manual { get; set; } = true;
        public string contact_no { get; set; }
        [Computed]
        public int milk_type { get; set; }
        [Computed]
        public new string flg_sentbox_entry { get; set; } = "N";
        public string originating_org_type { get; set; } = "portal";
    }
    public class BmcValidator : AbstractValidator<Bmc>
    {
        public BmcValidator()
        {
            RuleFor(d => d.bmc_code).Require();
            RuleFor(d => d.bmc_name).Require();
            RuleFor(d => d.is_active).Require();
        }

    }
}
