using Framework.Models;
using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;
using System;
using Framework.Library.Helper;
using System.Numerics;

namespace Models
{
    [Table("tbl_dcs_purchase_rate_details")]
    public class DcsPurchaseRateDetails :BaseModel
    {
        [ExplicitKey]
        public BigInteger code { get; set; }
        public float fat { get; set; }
        public float rtpl { get; set; }
        public float snf { get; set; }
        public int milk_quality_type_code { get; set; }
        public int milk_type_code { get; set; }
        public string purchase_rate_code { get; set; }
        public int rate_type_code { get; set; }
        [Computed]
        public new string flg_sentbox_entry { get; set; } = "N";
    }

    public class DcsPurchaseRateDetailsValidator : AbstractValidator<DcsPurchaseRateDetails>
    {
        public DcsPurchaseRateDetailsValidator()
        {
        }
    }
}
