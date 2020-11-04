using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataExchange.Areas.Service.Models
{
    public class CustomResponse
    {
        public string key_code { get; set; }
        public string status { get; set; } = "200";
        public string msg { get; set; } = "success";
                  
    }
}
