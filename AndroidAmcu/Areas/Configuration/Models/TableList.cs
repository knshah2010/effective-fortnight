using Framework.Models;
using Dapper.Contrib.Extensions;

namespace AndroidAmcu.Areas.Configuration.Models
{
    [Table("tbl_table_list")]
    public class TableList : BaseModel
    {
        public int id { get; set; }
        public string process_name { get; set; }
        public string table_name { get; set; }
        public string primary_table { get; set; }
        public string key_field { get; set; }
        public string child_key { get; set; }
        public int is_main { get; set; }
        public string has_child { get; set; }
        public string primary_key { get; set; }
        public bool is_offline { get; set; }
    }
}
