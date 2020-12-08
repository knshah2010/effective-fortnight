using Dapper.Contrib.Extensions;

namespace Models
{
    [Table("tbl_union_ratechart_range")]
    public class UnionRatechartRange 
    {
        public int ratechart_range_code { get; set; }
        public int animal_type_code { get; set; }
        public decimal min_fat { get; set; }
        public decimal max_fat { get; set; }
        public decimal min_snf { get; set; }
        public decimal max_snf { get; set; }
        public decimal min_clr { get; set; }
        public decimal max_clr { get; set; }
        public string union_code { get; set; }

        public string config_for { get; set; }
    }
}
