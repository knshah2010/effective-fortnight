using Framework.Models;
using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;

namespace Models
{
    [Table("tbl_union_config_result")]
    public class UnionConfigResult : BaseModel
    {
        [Key]
        [ExplicitKey]
        public int config_txn_code { get; set; }
        public int config_code { get; set; }
        public string config_name { get; set; }
        public string config_key { get; set; }
        public int config_result_code { get; set; }
        public string config_result_key { get; set; }
        public string config_result { get; set; }
        public string union_code { get; set; }
        public string x_col1 { get; set; }
        public string x_col2 { get; set; }
        public string x_col3 { get; set; }
        public string x_col4 { get; set; }
        public string x_col5 { get; set; }
        public string config_for { get; set; }
        [Computed]
        public new string flg_sentbox_entry { get; set; } = "N";
        public string originating_org_type { get; set; } = "portal";
    }

    public class PlantUnionConfigResultValidator : AbstractValidator<UnionConfigResult>
    {
        public PlantUnionConfigResultValidator()
        {
        }
    }
}
