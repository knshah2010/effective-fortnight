using Framework.Models;
using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;
using System;
using Framework.Library.Helper;

namespace Models
{
    [Table("tbl_purchase_rate_based")]
    public class PurchaseRateBased : BaseModel
    {
        [ExplicitKey]
        public string rate_based_code { get; set; }
        public int deduction_type { get; set; }
        public decimal end_range { get; set; }
        public decimal fixed_point { get; set; }
        public bool is_active { get; set; }
        public decimal kg_rate { get; set; }
        public int ref_type { get; set; }
        public decimal start_range { get; set; }
        public float step { get; set; }
        public decimal value { get; set; }
        public string formula_code { get; set; }
        public int milk_quality_type_code { get; set; }
        public int milk_type_code { get; set; }
        public string purchase_rate_code { get; set; }
        public int quality_param_code { get; set; }
        public int rate_type_code { get; set; }
        public string rate_class { get; set; }
        public string x_col1 { get; set; }
        public string x_col2 { get; set; }
        public string x_col3 { get; set; }
        public string x_col4 { get; set; }
        public string x_col5 { get; set; }
        [Computed]
        public new string flg_sentbox_entry { get; set; } = "N";
        public string originating_org_type { get; set; } = "portal";
    }

    public class PurchaseRateBasedValidator : AbstractValidator<PurchaseRateBased>
    {
        public PurchaseRateBasedValidator()
        {
            RuleFor(e => e.rate_based_code).Require();
        }
    }
}
