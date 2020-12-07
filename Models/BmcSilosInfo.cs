using Framework.Models;
using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;
using System;
using Framework.Library.Helper;

namespace Models
{
    [Table("tbl_bmc_silos_info")]
    public class BmcSilosInfo : BaseModel
    {
		[ExplicitKey]
		public int bmc_silos_info_code { get; set; }
		public string silo_no { get; set; }
		public string description { get; set; }
		public int manufacturer_code { get; set; }
		public string model { get; set; }
		public DateTime wef_date { get; set; }
		public int storage_capacity { get; set; }
		public int chilling_capacity { get; set; }
		public string owning_type { get; set; }
		public int milk_type_code { get; set; }
		public bool is_active { get; set; } = true;
		public string module_name { get; set; }
		public string module_code { get; set; }
		public string union_code { get; set; }
		public string x_col1 { get; set; }
		public string x_col2 { get; set; }
		public string x_col3 { get; set; }
		public string x_col4 { get; set; }
		public string x_col5 { get; set; }
		public string plant_code { get; set; }
		public string mcc_plant_code { get; set; }
		public string bmc_code { get; set; }
		[Computed]
		public new string flg_sentbox_entry { get; set; } = "N";
		public string originating_org_type { get; set; } = "portal";
	}

	public class BmcSilosInfoValidator : AbstractValidator<BmcSilosInfo>
    {
		public BmcSilosInfoValidator()
        {

        }
	}
}
