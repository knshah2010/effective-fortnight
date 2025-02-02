﻿using Framework.Models;
using Dapper.Contrib.Extensions;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AndroidAmcu.Areas.Configuration.Models
{
    [Table("sentbox")]   
    public class Sentbox : BaseModel
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
        public string table_names { get; set; }
        [JsonProperty("operation")]
        public string operation { get; set; }
        [JsonProperty("jsonText")]
        public string json_text { get; set; }
        [JsonProperty("errorLog")]
        public string error_log { get; set; }
        [JsonProperty("sequenceNo")]
        public int sequence_no { get; set; }
        [JsonProperty("originatingOrgId")]
        public string originating_org_id { get; set; }
        [JsonProperty("originatingOrgType")]
        public string originating_org_type { get; set; }
        [JsonProperty("postingTimestamp")]
        public DateTime posting_timestamp { get; set; }
        [JsonProperty("syncTimestamp")]
        public DateTime sync_timestamp { get; set; }
        [JsonProperty("sourceDeviceMac")]
        public string source_device_mac { get; set; }
        [JsonProperty("versionNo")]
        public string version_no { get; set; }
        [JsonProperty("deviceId")]
        public string device_id { get; set; }
       
        //public DateTime error_timestamp { get; set; }
        //public string queue_name { get; set; }
        //public int status { get; set; }
        //public DateTime pick_datetime { get; set; }
        //public DateTime response_datetime { get; set; }
        [Computed]
        [JsonIgnore]
        public new string flg_sentbox_entry { get; set; } = "N";
        [Computed]
        [JsonIgnore]
        public new int originating_type { get; set; } = 0;
        [Computed]
        [JsonIgnore]
        public new string originating_org_code { get; set; }
    }
}
