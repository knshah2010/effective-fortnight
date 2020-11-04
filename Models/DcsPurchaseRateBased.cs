using Framework.Models;
using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;
using System;
using Framework.Library.Helper;

namespace Models
{
    [Table("tbl_dcs_purchase_rate_based")]
    public class DcsPurchaseRateBased : BaseModel
    {
        [ExplicitKey]
        public string rate_based_code { get; set; }
        public int deduction_type { get; set; }
        public decimal end_range { get; set; }
        public decimal fixed_point { get; set; }
        public bool is_delete { get; set; }
        public decimal kg_rate { get; set; }
        public int ref_type { get; set; }
        public decimal start_range { get; set; }
        public int step { get; set; }
        public decimal value { get; set; }
        public string formula_code { get; set; }
        public int milk_quality_type_code { get; set; }
        public int milk_type_code { get; set; }
        public string purchase_rate_code { get; set; }
        public int quality_param_code { get; set; }
        public int rate_type { get; set; }
        public string formula { get; set; }
    }

    public class DcsPurchaseRateBasedValidator : AbstractValidator<DcsPurchaseRateBased>
    {
        public DcsPurchaseRateBasedValidator()
        {
            RuleFor(e => e.rate_based_code).Require();
        }
    }
}
