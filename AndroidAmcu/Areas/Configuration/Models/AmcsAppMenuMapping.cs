using Framework.Models;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;

namespace AndroidAmcu.Areas.Configuration.Models
{
    [Table("tbl_amcs_app_menu_mapping")]
    public class AmcsAppMenuMapping : BaseModel
    {
        [Key]
        [ExplicitKey]
        public int action_mapping_code { get; set; }
        public int action_code { get; set; }
        public string action_name { get; set; }
        public string application_type { get; set; }
        public string union_code { get; set; }
        public string user_code { get; set; }
    }
}
