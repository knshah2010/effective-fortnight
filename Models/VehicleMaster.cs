using Framework.Models;
using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;
using System;
using System.Numerics;

namespace Models
{
    [Table("tbl_vehicle_master")]
    public class VehicleMaster :BaseModel
    {
        [ExplicitKey]
        public string vehicle_code { get; set; }
        public int vehicle_type_code { get; set; }
        public int capacity_code { get; set; }
        public string registration_no { get; set; }
        public string applicable_rto { get; set; }
        public string driver_name { get; set; }
        public string driver_contact_no { get; set; }
        public DateTime wef_date { get; set; }
        public string driving_license_number { get; set; }
        public string transporter_code { get; set; }
        public string mapped_route { get; set; }
        public bool pollution_certificate { get; set; }
        public bool insurance { get; set; }
        public string rc_book_no { get; set; }
        public DateTime expiry_date { get; set; }
        public BigInteger rent { get; set; }
        public string average { get; set; }
        public string union_code { get; set; }
        public bool is_active { get; set; }
        public BigInteger bmc_fuel_type_codecode { get; set; }
        public string parsing_no { get; set; }
        public DateTime licence_expiry_date { get; set; }
        public string bmc_code { get; set; }
        public string x_col1 { get; set; }
        public string x_col2 { get; set; }
        public string x_col3 { get; set; }
        public string x_col4 { get; set; }
        public string x_col5 { get; set; }
        public int capacity { get; set; }
        public string billing_method { get; set; }
        [Computed]
        public new string flg_sentbox_entry { get; set; } = "N";
    }

    public class VehicleMasterValidator : AbstractValidator<VehicleMaster>
    {
        public VehicleMasterValidator()
        {
            RuleFor(d => d.vehicle_code).Require();
        }

    }
}
