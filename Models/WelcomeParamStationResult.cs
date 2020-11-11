using Framework.Models;
using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;
using Org.BouncyCastle.Math;

namespace Models
{
    [Table("welcome_param_station_result")]
    class WelcomeParamStationResult : BaseModel
    {
        [Key]
        [ExplicitKey]
        public BigInteger id { get; set; }
        public int welcome_param_id { get; set; }
        public string station_code { get; set; }
        public string param_value { get; set; }
    }
}
