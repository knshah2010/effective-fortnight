using Framework.Models;
using Dapper.Contrib.Extensions;
using System;

namespace AndroidAmcu.Areas.Configuration.Models
{
    [Table("tbl_android_installation_details")]
    public class AndroidInstallationDetails : BaseModel
    {
        [Key]
        [ExplicitKey]
        public int android_installation_details_id { get; set; }
        public string android_installation_id { get; set; }
        public string mobile_no { get; set; }
        public int otp_code { get; set; }
        public string hash_key { get; set; }
        public bool is_active { get; set; } = false;
        public bool is_expired { get; set; } = false;
        public string device_id { get; set; }
        public string device_type { get; set; }
        public string db_path { get; set; }
        public string use_for { get; set; }
        public string lat { get; set; }
        public string lat_long { get; set; }

        public string imei_no { get; set; }
        public string sync_key { get; set; }
        public bool sync_active { get; set; } = false;
        public string db_version { get; set; }
        public int installation_type { get; set; } = 0;
        public string version_no { get; set; }
        public int version_code { get; set; }
        public string password { get; set; }
        public DateTime password_date { get; set; }
        public int d2d_request { get; set; }
        [Computed]
        public new string flg_sentbox_entry { get; set; } = "N";
        [Computed]
        public new int originating_type { get; set; } = 0;
        [Computed]
        public new string originating_org_code { get; set; }

    }
}
