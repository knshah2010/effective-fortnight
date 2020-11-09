using Framework.Models;
using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;
using System;
using Framework.Library.Helper;
using System.Numerics;

namespace Models
{
    [Table("tbl_purchase_rate_applicability")]
    public class PurchaseRateApplicability : BaseModel
    {
        [ExplicitKey]
        public BigInteger rate_app_code { get; set; }
        public int shift_code { get; set; }
        public int is_download { get; set; }
        public DateTime wef_date { get; set; }
        public DateTime download_date_time { get; set; }
        public string union_code { get; set; }
        public string dcs_code { get; set; }
        public string purchase_rate_code { get; set; }
        public string x_col1 { get; set; }
        public string x_col2 { get; set; }
        public string x_col3 { get; set; }
        public string x_col4 { get; set; }
        public string x_col5 { get; set; }
        public bool is_active { get; set; }
        [Computed]
        public new string flg_sentbox_entry { get; set; } = "N";
        public string originating_org_type { get; set; } = "portal";
    }

    public class PurchaseRateApplicabilityValidator : AbstractValidator<PurchaseRateApplicability>
    {
        public PurchaseRateApplicabilityValidator()
        {
            
        }
    }
}
