using System;
using System.Collections.Generic;

namespace AndroidAmcu.Areas.Configuration.Models
{
    public class Acknowledgemen
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
            public List<string> uuid { get; set; }           
        }
   
}
