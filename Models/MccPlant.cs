using Framework.Models;
using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;
using System;
using Framework.Library.Helper;

namespace Models
{
    [Table("tbl_mcc_plant")]
    public class MccPlant : BaseModel
    {
        [ExplicitKey]
        public string mcc_plant_code { get; set; }
        public string mcc_plant_code_ex { get; set; }
        public string plant_code { get; set; }
        public string ref_code { get; set; }
        public string contact_person { get; set; }
        public string name { get; set; }
        public string mobile_no { get; set; }
        public string email { get; set; }
        public string description { get; set; }
        public int capacity { get; set; }
        public DateTime valid_from { get; set; }
        public string union_code { get; set; }
        public bool is_plant { get; set; }
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

    public class MccPlantValidator : AbstractValidator<MccPlant>
    {
        public MccPlantValidator()
        {
            RuleFor(d => d.mcc_plant_code).Require();

        }

    }
}
