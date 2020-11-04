using Framework.Models;
using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;

namespace Models
{
    [Table("tbl_vehicle_type")]
    public class VehicleType : BaseModel
    {
        [ExplicitKey]
        public int vehicle_type_code { get; set; }
        public string vehicle_type_name { get; set; }
        [Computed]
        public new string flg_sentbox_entry { get; set; } = "N";
    }

    public class VehicleTypeValidator : AbstractValidator<VehicleType>
    {
        public VehicleTypeValidator()
        {
            RuleFor(d => d.vehicle_type_code).Require();
            RuleFor(d => d.vehicle_type_name).Require();
        }

    }
}
