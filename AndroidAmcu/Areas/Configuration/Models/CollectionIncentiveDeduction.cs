using System;

namespace AndroidAmcu.Areas.Configuration.Models
{
    public class CollectionIncentiveDeduction
    {		
		public string dcs_code { get; set; }
		public string from_time { get; set; }
		public string to_time { get; set; }
		public int scheme_type { get; set; }
		public int shift_code { get; set; }
		public string amount { get; set; }
		public string from_date { get; set; }
		public string to_date { get; set; }
	}
}
