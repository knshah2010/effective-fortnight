using Newtonsoft.Json;
using System;

namespace AndroidAmcu.Areas.Configuration.Models
{
    public class TmpRateApplicability
    {
        [JsonProperty("rateAppCode")]
        public string rate_app_code { get; set; }
        [JsonProperty("shiftCode")]
        public int shift_code { get; set; }
        [JsonProperty("isDownload")]
        public int is_download { get; set; }
        [JsonProperty("wefDate")]
        public DateTime wef_date { get; set; }
        [JsonProperty("downloadDateTime")]
        public DateTime download_date_time { get; set; }
        [JsonProperty("unionCode")]
        public string union_code { get; set; }
        [JsonProperty("dcs_code")]
        public string dcs_code { get; set; }
        [JsonProperty("purchaseRateCode")]
        public string purchase_rate_code { get; set; }
        [JsonProperty("xCol1")]
        public string x_col1 { get; set; }
        [JsonProperty("xCol2")]
        public string x_col2 { get; set; }
        [JsonProperty("xCol3")]
        public string x_col3 { get; set; }
        [JsonProperty("xCol4")]
        public string x_col4 { get; set; }
        [JsonProperty("xCol5")]
        public string x_col5 { get; set; }
        [JsonProperty("isActive")]
        public bool is_active { get; set; }
        [JsonProperty("reteType")]
        public string rete_type { get; set; }
        [JsonProperty("refCode")]
        public string ref_code { get; set; }
        [JsonProperty("rateGenMethodCode")]
        public string rate_gen_method_code { get; set; }
        [JsonProperty("applicableCode")]
        public string applicable_code { get; set; }
        [JsonProperty("applicableFor")]
        public string applicable_for { get; set; }
        [JsonProperty("createdAt")]
        public DateTime? created_at { get; set; }
        [JsonProperty("createdBy")]
        public string created_by { get; set; }
        [JsonProperty("updatedAt")]
        public DateTime? updated_at { get; set; }
        [JsonProperty("updatedBy")]
        public string updated_by { get; set; }
    }
}
