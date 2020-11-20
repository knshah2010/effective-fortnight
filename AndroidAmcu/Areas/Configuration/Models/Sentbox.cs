using Framework.Models;
using Dapper.Contrib.Extensions;
using System;

namespace AndroidAmcu.Areas.Configuration.Models
{
    [Table("tbl_sentbox")]
    public class Sentbox : BaseModel
    {
        [ExplicitKey]
        public string uuid { get; set; }
        public string sync_status { get; set; }
        public string source_org_type { get; set; }
        public string source_org_id { get; set; }
        public string dest_org_type { get; set; }
        public string dest_org_id { get; set; }
        public string message_type { get; set; }
        public string table_name { get; set; }
        public string operation { get; set; }
        public string json_text { get; set; }
        public string error_log { get; set; }
        public int sequence_no { get; set; }
        public string originating_org_id { get; set; }
        public string originating_org_type { get; set; }
        public DateTime posting_timestamp { get; set; }
        public DateTime sync_timestamp { get; set; }
        public string source_device_mac { get; set; }
        public string version_no { get; set; }
        public string device_id { get; set; }
        public DateTime error_timestamp { get; set; }
        public string queue_name { get; set; }
        public int status { get; set; }
        public DateTime pick_datetime { get; set; }
        public DateTime response_datetime { get; set; }
        [Computed]
        public new string flg_sentbox_entry { get; set; } = "N";
        [Computed]
        public new int originating_type { get; set; } = 0;
        [Computed]
        public new string originating_org_code { get; set; }
    }
}
