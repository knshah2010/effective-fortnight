using Framework.Models;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AndroidAmcu.Areas.Configuration.Models
{
    [Table("tbl_inbox")]
    public class Inbox : BaseModel
    {
        [ExplicitKey]
        public string uuid { get; set; }
        [JsonProperty("syncStatus")]
        public string sync_status { get; set; }
        [JsonProperty("sourceOrgType")]
        public string source_org_type { get; set; }
        [JsonProperty("sourceOrgId")]
        public string source_org_id { get; set; }
        [JsonProperty("destOrgType")]
        public string dest_org_type { get; set; }
        [JsonProperty("destOrgId")]
        public string dest_org_id { get; set; }
        [JsonProperty("messageType")]
        public string message_type { get; set; }
        [JsonProperty("tableName")]
        public string table_name { get; set; }       
        public string operation { get; set; }
        [JsonProperty("jsonText")]
        public string json_text { get; set; }
       
        public string error_log { get; set; }
        [JsonProperty("sequenceNo")]
        public int sequence_no { get; set; }
        [JsonProperty("originatingOrgId")]
        public string originating_org_id { get; set; }
        [JsonProperty("originatingOrgType")]
        public string originating_org_type { get; set; }
        [JsonProperty("postingTimestamp")]
        public DateTime posting_timestamp { get; set; }
        public DateTime sync_timestamp { get; set; }
        [JsonProperty("sourceDeviceMac")]
        public string source_device_mac { get; set; }
        [JsonProperty("versionNo")]
        public string version_no { get; set; }
        public string device_id { get; set; }
        public DateTime error_timestamp { get; set; }
        [Computed]
        public new string flg_sentbox_entry { get; set; } = "N";
        [Computed]
        public new int originating_type { get; set; } = 0;
        [Computed]
        public new string originating_org_code { get; set; }     
        [Computed]
        public new DateTime? created_at { get; set; }       
        [Computed]
        public new string created_by { get; set; }       
        [Computed]
        public new DateTime? updated_at { get; set; }       
        [Computed]
        public new string updated_by { get; set; }
    }

    public class RequestFormat3
    {
        public string token { get; set; }
        public string imei { get; set; }
        public string deviceId { get; set; }
        public DateTime requestTime { get; set; }
        public string identity_code { get; set; }
        public string organizationCode { get; set; }
        public string organizationType { get; set; }
        public string syncKey { get; set; }
        public string versionNo { get; set; }
        public List<Inbox> content { get; set; }
    }

   
}
