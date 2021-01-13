using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataExchange.Areas.Service.Models
{
    public class Download
    {
        public int no_of_sample { get; set; }
        public decimal qty { get; set; }
        public decimal fat { get; set; }
        public decimal snf { get; set; }
        public string station_code { get; set; }
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string collection_date { get; set; }
        public string shift { get; set; }
        public int data_post_status { get; set; }

    }
}
