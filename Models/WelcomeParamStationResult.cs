using Framework.Models;
using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;
using Org.BouncyCastle.Math;

namespace Models
{
    [Table("welcome_param_station_result")]
    public class WelcomeParamStationResult : BaseModel
    {
        [Key]
        [ExplicitKey]
        public int id { get; set; }
        public int welcome_param_id { get; set; }
        public string station_code { get; set; }
        public string param_value { get; set; }
        [Computed]
        public string param_name { get; set; }
        [Computed]
        public new string flg_sentbox_entry { get; set; } = "N";
        [Computed]
        public new int originating_type { get; set; } = 0;
        [Computed]
        public new string originating_org_code { get; set; }
    }
}
