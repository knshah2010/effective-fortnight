using Framework.Models;
using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;
using System;
using Framework.Library.Helper;

namespace Models
{
    [Table("tbl_dcs")]
    public class Dcs : BaseModel
    {
        [ExplicitKey]
        public string dcs_code { get; set; }
        public string address { get; set; }
        public string bank_account_no { get; set; }
        public string contact_person { get; set; }
        public string dcs_code_ex { get; set; }
        public string dcs_name { get; set; }
        public string destination_code { get; set; } = "0";
        public int destination_type { get; set; } = 0;
        public string email { get; set; }
        public string ifsc { get; set; }
        public bool is_active { get; set; } = true;
        public int is_bmc { get; set; } = 0;
        public string mobile_no { get; set; }
        public string pan_no { get; set; }
        public string phone_no { get; set; }
        public string upi_no { get; set; }
        public string route_code { get; set; }
        public string union_code { get; set; }
        public string bmc_code { get; set; }
        public string mcc_plant_code { get; set; }
        public string plant_code { get; set; }
        public string x_col1 { get; set; }
        public string x_col2 { get; set; }
        public string x_col3 { get; set; }
        public string x_col4 { get; set; }
        public string x_col5 { get; set; }
        public string ref_code { get; set; }
        public string originating_org_type { get; set; } = "portal";
        public string version_no { get; set; } = "V5.0_010520";
        public bool is_dispatch_mandate { get; set; }
        public bool is_weight_manual { get; set; }
        public bool is_quality_manual { get; set; }
        public bool is_name_request { get; set; }
        [Computed]
        public int milk_type { get; set; }
        [Computed]
        public new string flg_sentbox_entry { get; set; } = "N";
        [Computed]
        public int allow_multiple_milktype { get; set; } = 1;
    }

    public class DcsValidator : AbstractValidator<Dcs>
    {
        public DcsValidator()
        {
            RuleFor(d => d.dcs_code).Require();
            RuleFor(d => d.dcs_name).Require();
            RuleFor(d => d.is_active).Require();
            RuleFor(d => d.bmc_code).Require().CheckAvailable("tbl_bmc");
            RuleFor(d => d.route_code).Require().CheckAvailable("tbl_route");
        }

    }
}
