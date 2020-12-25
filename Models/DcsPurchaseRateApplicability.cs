using Framework.Models;
using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;
using System;
using Framework.Library.Helper;
using Framework.CustomDataType;
using System.Collections.Generic;

namespace Models
{
    [Table("tbl_dcs_purchase_rate_applicability")]
    public class DcsPurchaseRateApplicability : BaseModel
    {
        [Key]
        [ExplicitKey]
		public int rate_app_code { get; set; }
		public bool is_active { get; set; }
		public DateTime wef_date { get; set; }
		public string dcs_code { get; set; }
		public string purchase_rate_code { get; set; }
		public int shift_code { get; set; }
		public string union_code { get; set; }
		public int rate_type { get; set; }
		public int rate_gen_method_code { get; set; }
		public DateTime download_date_time { get; set; } = DateHelper.CurrentDate();
		public int is_download { get; set; } = 1;
		public string applicable_code { get; set; }
		public string applicable_for { get; set; }
		[Computed]
		public new string flg_sentbox_entry { get; set; } = "N";
		public string originating_org_type { get; set; } = "portal";
		[Computed]
		public string applicability_unique_code { get; set; }

	}

	public class DcsPurchaseRateApplicabilityValidator : AbstractValidator<DcsPurchaseRateApplicability>
    {
		public DcsPurchaseRateApplicabilityValidator()
        {
			QueryParam range_Query = new QueryParam
			{
				Fields = "*",
				Table = typeof(DcsPurchaseRateApplicability).GetTableName(),
				Where = new List<ConditionParameter>
				{
					new ConditionParameter{PropertyName="purchase_rate_code" },
					new ConditionParameter{PropertyName="wef_date",direct_condition="@wef_date <= wef_date and @purchase_rate_code=purchase_rate_code and @applicable_code=applicable_code and @applicable_for=applicable_for" }
				},
			};

			RuleFor(e => e.wef_date).Range(range_Query);
		}
	}
}
