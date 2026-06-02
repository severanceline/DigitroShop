using FluentValidation;
using Store.Application.Services.Users.Commands.RegisterUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Users.Commands.RegisterUser.Validators
{
    public class RequestRegisterUserDtoValidator : AbstractValidator<RequestRegisterUserDto>
    {
        public RequestRegisterUserDtoValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("لطفاً نام و نام خانوادگی را وارد کنید")
                .MinimumLength(3).WithMessage("نام و نام خانوادگی باید حداقل ۳ کاراکتر باشد");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("لطفاً ایمیل را وارد کنید")
                .EmailAddress().WithMessage("فرمت ایمیل وارد شده صحیح نیست");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("لطفاً رمز عبور را وارد کنید")
                .Length(8, 20).WithMessage("رمز عبور باید بین ۸ تا ۲۰ کاراکتر باشد")
                .Matches(@"[A-Z]").WithMessage("رمز عبور باید حداقل شامل یک حرف بزرگ باشد")
                .Matches(@"[a-z]").WithMessage("رمز عبور باید حداقل شامل یک حرف کوچک باشد")
                .Matches(@"[0-9]").WithMessage("رمز عبور باید حداقل شامل یک عدد باشد");

            RuleFor(x => x.RePassword)
                .Equal(x => x.Password).WithMessage("رمز عبور و تکرار آن مطابقت ندارند");

            RuleFor(x => x.Roles)
                .NotEmpty().WithMessage("حداقل یک نقش برای کاربر انتخاب کنید")
                .Must(x => x != null && x.Count > 0).WithMessage("لیست نقش‌ها نمی‌تواند خالی باشد");

            RuleForEach(x => x.Roles).ChildRules(role =>
            {
                role.RuleFor(r => r.Id)
                    .GreaterThan(0).WithMessage("شناسه نقش وارد شده معتبر نیست");
            });
        }
    }
}
