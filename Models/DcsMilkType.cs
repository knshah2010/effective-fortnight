using Framework.Models;
using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;
using System;
using Framework.Library.Helper;

namespace Models
{
    [Table("tbl_dcs_milk_type")]
    class DcsMilkType : BaseModel
    {
        [Key]
        [ExplicitKey]
        public string dcs_code { get; set; }
        public int milk_type_code { get; set; }
        public bool is_active { get; set; }
        public decimal rtpl { get; set; }
        [Computed]
        public new string flg_sentbox_entry { get; set; } = "N";
        public string originating_org_type { get; set; } = "portal";
    }
}
