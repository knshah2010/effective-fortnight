using Framework.Models;
using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;
using System;
using Framework.Library.Helper;

namespace Models
{
    [Table("tbl_milk_collection_config")]
    public class MilkCollectionConfig : BaseModel
    {
		[Key]
		[ExplicitKey]
		public int code { get; set; }
		public bool accept_milk { get; set; }
		public string based_on { get; set; }
		public string based_on_disp { get; set; }
		public int can_per_ltr { get; set; }
		public decimal can_warning_per { get; set; }
		public string collection_mode { get; set; }
		public int collection_quantity_mode { get; set; }
		public int bmc_collection_quantity_mode { get; set; }
		public int local_milk_sale_quantity_mode { get; set; }
		public int sample_milk_quantity_mode { get; set; }
		public bool default_snf { get; set; }
		public decimal default_snf_value { get; set; }
		public bool from_machine_clr { get; set; }
		public string based_on_local_sale { get; set; }
		public int no_disp_local_sale { get; set; }
		public int per_local_sale { get; set; }
		public bool input_clr { get; set; }
		public decimal lr1_for_clr { get; set; }
		public decimal lr2_for_clr { get; set; }
		public decimal ltr_to_kg { get; set; }
		public bool multi_entry_diff_milk_type { get; set; }
		public bool multi_entry_same_milk_type { get; set; }
		public int no { get; set; }
		public int no_disp { get; set; }
		public decimal sample_milk_size { get; set; }
		public bool seperate_can { get; set; }
		public int shift_code { get; set; }
		public int shift_code_disp { get; set; }
		public decimal variation_in_fat { get; set; }
		public bool variation_in_fat_block { get; set; }
		public decimal variation_in_qty { get; set; }
		public bool variation_in_qty_block { get; set; }
		public decimal variation_in_snf { get; set; }
		public bool variation_in_snf_block { get; set; }
		public string union_code { get; set; }
		public int weight_setting { get; set; }
		public int quality_setting { get; set; }
		public int dispatch_setting { get; set; }
		public string x_col1 { get; set; }
		public string x_col2 { get; set; }
		public string x_col3 { get; set; }
		public string x_col4 { get; set; }
		public string x_col5 { get; set; }
		public string mcc_plant_code { get; set; }
		public bool is_active { get; set; }
		public int dispatch_quantity_mode { get; set; }
		public string plant_code { get; set; }
		public string bmc_code { get; set; }
		public string dcs_code { get; set; }
	}

	public class MilkCollectionConfigValidator : AbstractValidator<MilkCollectionConfig>
    {
		public MilkCollectionConfigValidator()
        {

        }
	}
}
