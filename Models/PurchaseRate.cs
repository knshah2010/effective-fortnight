using Framework.Models;
using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;
using System;
using Framework.Library.Helper;

namespace Models
{
    [Table("tbl_purchase_rate")]
    public class PurchaseRate : BaseModel
    {
        [ExplicitKey]
        public string purchase_rate_code { get; set; }
        public int shift_id { get; set; }
        public int shift_applicability { get; set; }
        public int rate_gen_method_code { get; set; }
        public string description { get; set; }
        public string union_code { get; set; }
        public string rate_category { get; set; }
        public string x_col1 { get; set; }
        public string x_col2 { get; set; }
        public string x_col3 { get; set; }
        public string x_col4 { get; set; }
        public string x_col5 { get; set; }
        public string rate_type { get; set; }
        public int rate_type_2 { get; set; }
        public decimal c_min_fat { get; set; }
        public decimal c_max_fat { get; set; }
        public decimal c_min_snf { get; set; }
        public decimal c_max_snf { get; set; }
        public decimal b_min_fat { get; set; }
        public decimal b_max_fat { get; set; }
        public decimal b_min_snf { get; set; }
        public decimal b_max_snf { get; set; }
        public decimal m_min_fat { get; set; }
        public decimal m_max_fat { get; set; }
        public decimal m_min_snf { get; set; }
        public decimal m_max_snf { get; set; }
        public int rate_digit { get; set; }
        public string download_status { get; set; }
        public DateTime wef_date { get; set; }
        [Computed]
        public new string flg_sentbox_entry { get; set; } = "N";
        public string originating_org_type { get; set; } = "portal";
    }

    public class PurchaseRateValidator : AbstractValidator<PurchaseRate>
    {
        public PurchaseRateValidator()
        {
            RuleFor(d => d.purchase_rate_code).Require();
        }
    }
}
