using Framework.Models;
using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;
using System;
using Framework.Library.Helper;
using System.Numerics;
using Framework.CustomDataType;
using System.Collections.Generic;

namespace Models
{
    [Table("tbl_purchase_rate_applicability")]
    public class PurchaseRateApplicability : BaseModel
    {
        [ExplicitKey]
        public string rate_app_code { get; set; }
        public int shift_code { get; set; }
        public int is_download { get; set; } = 0;
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
        public bool is_active { get; set; } = true;
        public string ref_code { get; set; }
        [Computed]
        public string applicability_unique_code { get; set; }
        [Computed]
        public new string flg_sentbox_entry { get; set; } = "N";
        public string originating_org_type { get; set; } = "portal";
        [Computed]
        public string module_code { get; set; }
        [Computed]
        public string module_name { get; set; }
        [Computed]
        public string rate_for { get; set; }
        [Computed]
        public string shift { get; set; } = "M";
    }

    public class PurchaseRateApplicabilityValidator : AbstractValidator<PurchaseRateApplicability>
    {
        public PurchaseRateApplicabilityValidator()
        {
            QueryParam Query = new QueryParam
            {
                Fields = "wef_date",
                Table = "tbl_purchase_rate_applicability",
                Where = new List<ConditionParameter>
                {
                     new ConditionParameter{PropertyName="purchase_rate_code"},
                     new ConditionParameter{PropertyName="wef_date"},
                }
            };
            QueryParam range_Query = new QueryParam
            {
                Fields = "*",
                Table = typeof(PurchaseRateApplicability).GetTableName(),
                Where = new List<ConditionParameter>
                {
                    new ConditionParameter{PropertyName="purchase_rate_code" },
                    new ConditionParameter{PropertyName="wef_date",direct_condition="@wef_date <= wef_date and @purchase_rate_code=purchase_rate_code and @dcs_code=dcs_code" }
                },

            };


            RuleFor(e => e.module_name).Require();
            RuleFor(e => e.module_code).Require();
            RuleFor(e => e.applicability_unique_code).Require();
            List<string> rate_for_condition = new List<string> { "farmer_collection", "rmrd_collection" };
            RuleFor(d => d.rate_for).Require().Must(d => rate_for_condition.Contains(d))
                    .WithMessage("Please only use: " + String.Join(",", rate_for_condition));

            RuleFor(e => e.wef_date).Range(range_Query);

        }
    }
}
