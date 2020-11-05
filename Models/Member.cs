using Framework.Models;
using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;
using System;
using Framework.Library.Helper;


namespace Models
{
    [Table("tbl_member")]
    public class Member : BaseModel
    {
        [ExplicitKey]
        public string member_code { get; set; }
        public string ex_member_code { get; set; }
        public string member_name { get; set; }
        public string surname { get; set; }
        public int animal_type_code { get; set; }
        public string gender_code { get; set; } = "0";
        public string mobile_no { get; set; }
        public string email { get; set; }
        public string adhar_no { get; set; }
        public string pan_no { get; set; }
        public string bank_account_no { get; set; }
        public string ifsc { get; set; }
        public string dcs_code { get; set; }
        public string rate_class { get; set; }
        public bool is_active { get; set; } = true;
        public string x_col1 { get; set; }
        public string x_col2 { get; set; }
        public string x_col3 { get; set; }
        public string x_col4 { get; set; }
        public string x_col5 { get; set; }

        [Computed]
        public new string flg_sentbox_entry { get; set; } = "N";
        public string originating_org_type { get; set; } = "portal";
    }

    public class MemberValidator : AbstractValidator<Member>
    {
        public MemberValidator()
        {
            RuleFor(d => d.member_code).Require();
            RuleFor(d => d.member_name).Require();
        }

    }
}
