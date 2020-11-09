using Framework.Models;
using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;
using System;
using Framework.Library.Helper;
using System.Numerics;

namespace Models
{
    [Table("tbl_purchase_rate_details")]
    public class PurchaseRateDetails :BaseModel
    {
        [ExplicitKey]
        public int code { get; set; }
        public float fat { get; set; }
        public float rtpl { get; set; }
        public float snf { get; set; }
        public int milk_quality_type_code { get; set; }
        public int milk_type_code { get; set; }
        public string purchase_rate_code { get; set; }
        public int rate_type_code { get; set; }
        public string rate_class { get; set; }
        public bool is_active { get; set; }
        [Computed]
        public new string flg_sentbox_entry { get; set; } = "N";
        public string originating_org_type { get; set; } = "portal";
    }

    public class PurchaseRateDetailsValidator : AbstractValidator<PurchaseRateDetails>
    {
        public PurchaseRateDetailsValidator()
        {
        }
    }
}
