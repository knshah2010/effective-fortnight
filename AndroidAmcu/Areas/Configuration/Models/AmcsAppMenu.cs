using Framework.Models;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;

namespace AndroidAmcu.Areas.Configuration.Models
{
    [Table("tbl_amcs_app_menu")]
    public class AmcsAppMenu : BaseModel
    {
        [Key]
        [ExplicitKey]
        public int action_code { get; set; }
        public string action_name { get; set; }
        public string description { get; set; }
        public bool is_active { get; set; }
        public int sequence_no { get; set; }
        public int parent_code { get; set; }
        public string parent_name { get; set; }
    }
}
