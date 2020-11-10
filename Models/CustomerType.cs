using Framework.Models;
using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;
using System;
using Framework.Library.Helper;
using System.Collections.Generic;

namespace Models
{
    [Table("tbl_customer_type")]
    public class CustomerType : BaseModel
    {
        [Key]
        [ExplicitKey]
        public int customer_type_code { get; set; }
        public string customer_type { get; set; }
        public string customer_desc { get; set; }
        public string code_prefix { get; set; }
        public int code_length { get; set; }
        public int is_organisation { get; set; }
        public string union_code { get; set; }
        public bool is_active { get; set; }
        public bool is_product_sale { get; set; }
        public bool is_product_req { get; set; }
        public bool is_bmc_dispatch { get; set; }
        public bool is_collection { get; set; }
        public bool is_applicability { get; set; }
    }
}
