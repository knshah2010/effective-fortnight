using Newtonsoft.Json;
using System;

namespace AndroidAmcu.Areas.Configuration.Models
{   
    public class TmpPurchaseRate 
    {
        [JsonProperty("purchaseRateCode")]
        public string purchase_rate_code { get; set; }
        [JsonProperty("shiftId")]
        public int shift_id { get; set; }
        [JsonProperty("shiftApplicability")]
        public int shift_applicability { get; set; }
        [JsonProperty("rateGenMethodCode")]
        public int rate_gen_method_code { get; set; }
        public string description { get; set; }
        [JsonProperty("unionCode")]
        public string union_code { get; set; }
        [JsonProperty("qtyMode")]
        public int qty_mode { get; set; }
        [JsonProperty("referenceCode")]
        public string reference_code { get; set; }
        [JsonProperty("isActive")]
        public bool is_active { get; set; }
        [JsonProperty("isDefault")]
        public bool is_default { get; set; }
        [JsonProperty("isDelete")]
        public bool is_delete { get; set; }
        [JsonProperty("forMember")]
        public bool for_member { get; set; }
        [JsonProperty("isProcess")]
        public int is_process { get; set; }
        [JsonProperty("wefDate")]
        public DateTime wef_date { get; set; }
        [JsonProperty("createdAt")]
        public DateTime created_at { get; set; }
        [JsonProperty("createdBy")]
        public string created_by { get; set; }
        [JsonProperty("originatingOrgCode")]
        public string originating_org_code { get; set; }
        [JsonProperty("originatingOrgType")]
        public string originating_org_type { get; set; }
        [JsonProperty("originatingType")]
        public int originating_type { get; set; }
    }
}
