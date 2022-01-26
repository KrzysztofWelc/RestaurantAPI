using FluentValidation;
using RestaurantAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Models.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserDTO>
    {
        public RegisterUserValidator(RestaurantDBContext DBContext)
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).MinimumLength(6);
            RuleFor(x => x.ConfirmPassword).Equal(e => e.Password);
            RuleFor(x => x.Email).Custom((value, context) =>
              {
                  var emailInUse = DBContext.Users.Any(u => u.Email == value);
                  if (emailInUse)
                  {
                      context.AddFailure("Email", "Email already taken");
                  }
              });
        }
    }
}
