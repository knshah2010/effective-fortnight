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
        [Computed]
        public new string flg_sentbox_entry { get; set; } = "N";
        [Computed]
        public int allow_multiple_milktype { get; set; }
    }

    public class DcsValidator : AbstractValidator<Dcs>
    {
        public DcsValidator()
        {
            RuleFor(d => d.dcs_code).Require();
            RuleFor(d => d.dcs_name).Require();
        }

    }
}
