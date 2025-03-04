using Application.DTOs;
using FluentValidation;

namespace Application.Validators
{
    public class UserDtoValidator : AbstractValidator<UserDTO>
    {
        public UserDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("FirstName Required.")
                .MaximumLength(128);

            RuleFor(x => x.LastName)
                .MaximumLength(128);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email Required.")
                .EmailAddress().WithMessage("Enter a valid email.");

            RuleFor(x => x.DateOfBirth)
               .NotEmpty().WithMessage("DateOfBirth Required.")
               .Must(BeAtLeast18YearsOld).WithMessage("You must be at least 18 years old.");

            RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("PhoneNumber Required.")
            .GreaterThan(0)
            .Must(HaveTenDigits)
            .WithMessage("The PhoneNumber must contain exactly 10 digits.");

        }
        private bool BeAtLeast18YearsOld(DateTime date)
        {
            return date <= DateTime.Today.AddYears(-18);
        }

        private bool HaveTenDigits(long number)
        {
            var numberAsString = number.ToString();
            return numberAsString.Length == 10;
        }
    }
}
