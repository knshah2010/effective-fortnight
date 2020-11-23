using System;

namespace AndroidAmcu.Areas.General.Models
{
    public class RequestFormat1
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
        public ContentType content { get; set; }
    }

    public class ContentType
    {
        public string mobileNo { get; set; } 
        public string otpCode { get; set; }
    }
}
