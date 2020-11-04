using Framework.Models;
using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;
using System;
using Framework.Library.Helper;

namespace Models
{
    [Table("users")]
    public class Users : BaseModel
    {
        [ExplicitKey]
        public string user_code { get; set; }
        public string user_name { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string mobile_no { get; set; }
        public string password { get; set; }
        public string address { get; set; }
        public string lat_long { get; set; }
        public string vendor_code { get; set; }
        public string version_no { get; set; }
        public string device_id { get; set; }
        public int otp_code { get; set; }
        public string user_type { get; set; }
        public string device_detail { get; set; }
        public bool is_active { get; set; }
        public string device_token { get; set; }
        public string reference_code { get; set; }
        public int department_code { get; set; }

        [Computed]
        public string token { get; set; }

    }

    public class UsersValidator : AbstractValidator<Users>
    {
        public UsersValidator()
        {
            // RuleFor(d => d.user_name).Require();
        }
    }
}
