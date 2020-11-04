using Framework.Models;
using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;
using System;
using Framework.Library.Helper;

namespace Models
{
    [Table("user_organization_mapping")]
    public partial class UserOrganizationMapping : BaseModel
    {
        [Key]
        [ExplicitKey]
        public int user_organization_mapping_code { get; set; }
        public string user_code { get; set; }
        public string company_code { get; set; }
        public string plant_code { get; set; }
        public string mcc_code { get; set; }
        public string bmc_code { get; set; }
        public string mpp_code { get; set; }
        public bool is_active { get; set; }
        public string org_type { get; set; }
    }

    public class UserOrganizationMappingValidator : AbstractValidator<UserOrganizationMapping>
    {
        public UserOrganizationMappingValidator()
        {
            RuleFor(x => x.user_code).Require();
            RuleFor(x => x.org_type).Require();
        }
    }

    public partial class OrgMapping
    {
        public string user_code { get; set; }
        public string company_code { get; set; }
        public string company_name { get; set; }
        public string plant_code { get; set; }
        public string plant_name { get; set; }
        public string mcc_code { get; set; }
        public string mcc_name { get; set; }
        public string bmc_code { get; set; }
        public string bmc_name { get; set; }
        public string mpp_code { get; set; }
        public string mpp_name { get; set; }
        public string org_type { get; set; }

    }
}
