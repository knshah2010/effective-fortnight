using Framework.Models;
using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;
using System;
using Framework.Library.Helper;

namespace Models
{
    [Table("tbl_dcs_purchase_rate")]
    public class DcsPurchaseRate : BaseModel
    {
        [ExplicitKey]
        public int purchase_rate_code { get; set; }
        public string description { get; set; }
        public int rate_gen_method_code { get; set; }
        public int shift_applicability { get; set; }
        public int shift_id { get; set; }
        public string union_code { get; set; }
        public int qty_mode { get; set; }
        public string reference_code { get; set; }
        public int is_process { get; set; }
        public string milk_purchase_rate_code { get; set; }
        public DateTime wef_date { get; set; }
        public bool is_active { get; set; }
        public bool is_delete { get; set; }
        [Computed]
        public new string flg_sentbox_entry { get; set; } = "N";
    }

    public class DcsPurchaseRateValidator : AbstractValidator<DcsPurchaseRate>
    {
        public DcsPurchaseRateValidator()
        {
            RuleFor(d => d.purchase_rate_code).Require();
        }
    }
}
