using FluentValidation;

namespace DTOs
{
    public class ContactDetailValidator : AbstractValidator<ContactDetailDTO>
    {
        public ContactDetailValidator()
        {
            RuleFor(x => x.Mobile)
                .NotNull().WithMessage("MobileNumber cannot be null.")
                .Length(10).WithMessage("MobileNumber should be of 10 digit.");
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Name cannot be null.")
                .NotEmpty().WithMessage("Name cannot be empty.");

        }
    }
}
