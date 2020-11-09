using Framework.Models;
using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;
using System;
using Framework.Library.Helper;
using System.Collections.Generic;

namespace Models
{
    [Table("tbl_unions")]
    public class Unions : BaseModel
    {
        [ExplicitKey]
        public string union_code { get; set; }
        public string address { get; set; }
        public string bank_account_no { get; set; }
        public string city { get; set; }
        public string contact_person { get; set; }
        public string contact_person_email { get; set; }
        public string contact_person_mobile_no { get; set; }
        public string contact_person_pan_no { get; set; }
        public string contact_person_phone_no { get; set; }
        public string fax_no { get; set; }
        public string ifsc { get; set; }
        public bool is_active { get; set; }
        public string phone_no { get; set; }
        public string pincode { get; set; }
        public DateTime registration_date { get; set; }
        public string registration_no { get; set; }
        public string union_code_ex { get; set; }
        public string union_name { get; set; }
        public string upi_no { get; set; }
        public string bank_code { get; set; }
        public string branch_code { get; set; }
        public string district_code { get; set; }
        public string federation_code { get; set; }
        public string hamlet_code { get; set; }
        public string state_code { get; set; }
        public string sub_district_code { get; set; }
        public string village_code { get; set; }
        public string local_name { get; set; }
        public string local_address { get; set; }
        public string local_contact_person { get; set; }
        public string union_short_name { get; set; }
        public DateTime valid_from { get; set; }
        public string gst_no { get; set; }
        public string logo { get; set; }
        public bool has_bmc { get; set; }
        public bool has_mcc { get; set; }
        public bool allow_member_create { get; set; }
        public string x_col1 { get; set; }
        public string x_col2 { get; set; }
        public string x_col3 { get; set; }
        public string x_col4 { get; set; }
        public string x_col5 { get; set; }
        public string eipl_code { get; set; }
        public string eipl_token { get; set; }
        [Computed]
        public new string flg_sentbox_entry { get; set; } = "N";
        public string originating_org_type { get; set; } = "portal";
    }
    public class UnionsValidator : AbstractValidator<Unions>
    {
        public UnionsValidator()
        {
            
        }

    }
}
