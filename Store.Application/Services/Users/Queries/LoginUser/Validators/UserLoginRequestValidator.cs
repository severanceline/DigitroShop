using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Users.Queries.LoginUser.Validators
{
    public class UserLoginRequestValidator :  AbstractValidator<UserLoginRequest>
    {
        public UserLoginRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("لطفاً ایمیل را وارد کنید")
                .EmailAddress().WithMessage("فرمت ایمیل وارد شده صحیح نیست");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("لطفاً رمز عبور را وارد کنید")
                .Length(8, 20).WithMessage("رمز عبور باید بین ۸ تا ۲۰ کاراکتر باشد")
                .Matches(@"[A-Z]").WithMessage("رمز عبور باید حداقل شامل یک حرف بزرگ باشد")
                .Matches(@"[a-z]").WithMessage("رمز عبور باید حداقل شامل یک حرف کوچک باشد")
                .Matches(@"[0-9]").WithMessage("رمز عبور باید حداقل شامل یک عدد باشد");
        }
    }
}
