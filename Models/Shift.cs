using Dapper.Contrib.Extensions;
using System;

namespace Models
{
    [Table("tbl_shift")]
    public class Shift
    {
        public int id { get;set;}
        public string shift { get;set;}
        public TimeSpan shift_time { get;set;}
        public string short_name { get;set;}
      
    }
}
