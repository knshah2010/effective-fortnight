using Newtonsoft.Json;
using System;

namespace AndroidAmcu.Areas.Configuration.Models
{
    public class TmpPurchaseRateBase
    {
        [JsonProperty("rateBasedCode")]
        public string rate_based_code { get; set; }
        [JsonProperty("deductionType")]
        public int deduction_type { get; set; }
        [JsonProperty("endRange")]
        public decimal end_range { get; set; }
        [JsonProperty("fixedPoint")]
        public decimal fixed_point { get; set; }
        [JsonProperty("isActive")]
        public bool is_active { get; set; }
        [JsonProperty("isDelete")]
        public bool is_delete { get; set; }
        [JsonProperty("kgRate")]
        public decimal kg_rate { get; set; }
        [JsonProperty("refType")]
        public int ref_type { get; set; }
        [JsonProperty("startRange")]
        public decimal start_range { get; set; }
        public float step { get; set; }
        public decimal value { get; set; }
        [JsonProperty("formulaCode")]
        public string formula_code { get; set; }
        public string formula { get; set; }
        [JsonProperty("milkQualityTypeCode")]
        public int milk_quality_type_code { get; set; }
        [JsonProperty("milkTypeCode")]
        public int milk_type_code { get; set; }
        [JsonProperty("purchaseRateCode")]
        public string purchase_rate_code { get; set; }
        [JsonProperty("qualityParamCode")]
        public int quality_param_code { get; set; }
        [JsonProperty("rateTypeCode")]
        public int rate_type_code { get; set; }
        [JsonProperty("rateType")]
        public int rate_type { get; set; }
        [JsonProperty("rateClass")]
        public string rate_class { get; set; }
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
