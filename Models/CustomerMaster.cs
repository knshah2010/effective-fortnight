﻿using Framework.Models;
using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;
using System;
using Framework.Library.Helper;
using System.Collections.Generic;
using Newtonsoft.Json;
using Framework.Library;

namespace Models
{
    [Table("tbl_customer_master")]
    public class CustomerMaster : BaseModel
    {
        [ExplicitKey]
        public string customer_code { get; set; }
        public string customer_code_ex { get; set; }
        public string ref_code { get; set; }
        public string customer_name { get; set; }
        public string gst_no { get; set; }
        public string customer_type { get; set; }
        public string route_code { get; set; }
        public string local_name { get; set; }
        public string address { get; set; }
        public string local_address { get; set; }
        public string state_code { get; set; }
        public string district_code { get; set; }
        public string sub_district_code { get; set; }
        public string village_code { get; set; }
        public string hamlet_code { get; set; }
        public string sap_code { get; set; }
        public string union_code { get; set; }
        public string refference_code { get; set; }
        public string plant_code { get; set; }
        public string mcc_plant_code { get; set; }
        public string bmc_code { get; set; }      
        public bool is_active { get; set; } = true;
        public string x_col1 { get; set; }
        public string x_col2 { get; set; }
        public string x_col3 { get; set; }
        public string x_col4 { get; set; }
        public string x_col5 { get; set; }
        public string mobile_no { get; set; }
        [Computed]
        public int allow_multiple_milktype { get; set; } = 1;
        [Computed]
        public string customer_unique_code { get; set; }
        [Computed]
        public new string flg_sentbox_entry { get; set; } = "N";
        public string originating_org_type { get; set; } = "portal";
    }

    public class CustomerMasterValidator : AbstractValidator<CustomerMaster>
    {
        public CustomerMasterValidator()
        {
            RuleFor(d => d.customer_code).Require();
            //RuleFor(d => d.customer_code_ex).Unique().WithMessage("member_code_already_exist");
            RuleFor(d => d.ref_code).Unique();
            RuleFor(d => d.customer_unique_code).Require();
            RuleFor(d => d.customer_name).Require();
            RuleFor(d => d.customer_type).Require();
            RuleFor(d => d.is_active).Require();
            RuleFor(d => d.bmc_code).Require().CheckAvailable("tbl_bmc");
            RuleFor(d => d.route_code).Require().CheckAvailable("tbl_route");

            List<string> customer_type_condition = new List<string> { "VENDOR","BULKVEN","VLCCVEN" };
            RuleFor(d => d.customer_type).Require().Must(d => customer_type_condition.Contains(d))
                    .WithMessage("Please only use: " + String.Join(",", customer_type_condition));
        }

    }
}
