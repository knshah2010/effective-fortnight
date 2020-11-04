using Framework.Models;
using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;
using System;
using Framework.Library.Helper;

namespace Models
{
    [Table("tbl_customer_master")]
    public class CustomerMaster : BaseModel
    {
        [ExplicitKey]
        public string customer_code { get; set; }
        public string customer_code_ex { get; set; }
        public string ref_code { get; set; }
        public string customer_name { get; set; }
        public string gst_no { get; set; }
        public string customer_type { get; set; }
        public string route_code { get; set; }
        public string local_name { get; set; }
        public string address { get; set; }
        public string local_address { get; set; }
        public string state_code { get; set; }
        public string district_code { get; set; }
        public string sub_district_code { get; set; }
        public string village_code { get; set; }
        public string hamlet_code { get; set; }
        public string sap_code { get; set; }
        public string union_code { get; set; }
        public string refference_code { get; set; }
        public string plant_code { get; set; }
        public string mcc_plant_code { get; set; }
        public string bmc_code { get; set; }
        public bool is_active { get; set; }
        public string x_col1 { get; set; }
        public string x_col2 { get; set; }
        public string x_col3 { get; set; }
        public string x_col4 { get; set; }
        public string x_col5 { get; set; }
        public string mobile_no { get; set; }
        [Computed]
        public new string flg_sentbox_entry { get; set; } = "N";
    }

    public class CustomerMasterValidator : AbstractValidator<CustomerMaster>
    {
        public CustomerMasterValidator()
        {
            RuleFor(d => d.customer_code).Require();
            RuleFor(d => d.customer_name).Require();
        }

    }
}
