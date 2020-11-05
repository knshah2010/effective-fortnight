using Framework.Models;
using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;
using System;
using Framework.Library.Helper;
using System.Collections.Generic;

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
        public int capacity { get; set; } = 1;
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
        public string route_supervisor_name { get; set; }
        public string contact_no { get; set; }

        [Computed]
        public new string flg_sentbox_entry { get; set; } = "N";
        public string originating_org_type { get; set; } = "portal";
    }
    public class RouteValidator : AbstractValidator<Route>
    {
        public RouteValidator()
        {
            RuleFor(d => d.route_code).Require();
            RuleFor(d => d.route_name).Require();

            List<string> route_type_condition = new List<string> { "Can", "Tanker"};
            RuleFor(d => d.route_type).Must(d => route_type_condition.Contains(d))
                    .WithMessage("Please only use: " + String.Join(",", route_type_condition));
        }

    }
}
