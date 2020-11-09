using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;
using Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    [Table("user_token")]
    public partial class UserToken : BaseModel
    {
        [Key]
        [ExplicitKey]
        public int user_token_code { get; set; }
        public string user_code { get; set; }
        public string token { get; set; }
        public DateTime login_time { get; set; }
        public DateTime? logout_time { get; set; }
        public bool is_active { get; set; }
    }

    public partial class UserTokenValidator : AbstractValidator<UserToken>
    {
        public UserTokenValidator()
        {
            RuleFor(d => d.user_code).Require();
        }
    }
}
