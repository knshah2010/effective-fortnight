using Framework.Models;
using Dapper.Contrib.Extensions;


namespace AndroidAmcu.Areas.Configuration.Models
{
    [Table("tbl_android_installation")]
    public class AndroidInstallation : BaseModel
    {
        [ExplicitKey]
        public string android_installation_id { get; set; }
        public string application_installation_code { get; set; }
        public string organization_code { get; set; }
        public string organization_type { get; set; }
        public string module_code { get; set; }
        public string module_name { get; set; }
        [Computed]
        public new string flg_sentbox_entry { get; set; } = "N";
        [Computed]
        public new int originating_type { get; set; } = 0;
        [Computed]
        public new string originating_org_code { get; set; }
    }
}
