using Newtonsoft.Json;
using System;


namespace AndroidAmcu.Areas.Configuration.Models
{
    public class TmpMember
    {
        [JsonProperty("memberCode")]
        public string member_code { get; set; }
        [JsonProperty("exMemberCode")]
        public string ex_member_code { get; set; }
        [JsonProperty("memberName")]
        public string member_name { get; set; }
        public string surname { get; set; }
        [JsonProperty("animalTypeCode")]
        public int animal_type_code { get; set; }
        [JsonProperty("genderCode")]
        public string gender_code { get; set; }
        [JsonProperty("mobileNo")]
        public string mobile_no { get; set; }
        public string email { get; set; }
        [JsonProperty("adharNo")]
        public string adhar_no { get; set; }
        [JsonProperty("panNo")]
        public string pan_no { get; set; }
        [JsonProperty("bankAccountNo")]
        public string bank_account_no { get; set; }
        public string ifsc { get; set; }
        [JsonProperty("dcsCode")]
        public string dcs_code { get; set; }
        [JsonProperty("rateClass")]
        public string rate_class { get; set; }
        [JsonProperty("isActive")]
        public bool is_active { get; set; }
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
        [JsonProperty("refCode")]
        public string ref_code { get; set; }
        [JsonProperty("createdAt")]
        public DateTime? created_at { get; set; }
        [JsonProperty("createdBy")]
        public string created_by { get; set; }
        [JsonProperty("updatedAt")]
        public DateTime? updated_at { get; set; }
        [JsonProperty("updatedBy")]
        public string updated_by { get; set; }
        [JsonProperty("originatingOrgCode")]
        public string originating_org_code { get; set; }
        [JsonProperty("originatingOrgType")]
        public string originating_org_type { get; set; }
        [JsonProperty("originatingType")]
        public int originating_type { get; set; }

    }
}
