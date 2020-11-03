using Framework.Models;
using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;
using System;
using Framework.Library.Helper;

namespace Models
{
    [Table("tbl_route")]
    public class Route : BaseModel
    {
        [ExplicitKey]
        public string route_code { get; set; }
        public string route_code_ex { get; set; }
        public string route_name { get; set; }
        public string ref_code { get; set; }
        public string to_dest { get; set; }
        public string to_type { get; set; }
        public int capacity { get; set; } = 0;
        public string route_type { get; set; }
        public float route_length_kms { get; set; } 
        public string morning_start_time { get; set; }
        public string morning_end_time { get; set; }
        public string evening_start_time { get; set; }
        public string evening_end_time { get; set; }
        public string union_code { get; set; }
        public int vehicle_type_code { get; set; }
        public string x_col1 { get; set; }
        public string x_col2 { get; set; }
        public string x_col3 { get; set; }
        public string x_col4 { get; set; }
        public string x_col5 { get; set; }
        public bool is_active { get; set; }
        [Computed]
        public new string flg_sentbox_entry { get; set; } = "N";
    }
    public class RouteValidator : AbstractValidator<Route>
    {
        public RouteValidator()
        {
            RuleFor(d => d.route_code).Require();
            RuleFor(d => d.route_name).Require();
        }

    }
}
