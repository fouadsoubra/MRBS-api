using FluentValidation;
using MeetingRoomBookingSystem.Resources;

namespace MeetingRoomBookingSystem.Validators
{
    public class SaveCompanyResourceValidator : AbstractValidator<SaveCompanyResource>
    {
       /* public SaveCompanyResourceValidator()
        {
            RuleFor(a => a.Name)
                .NotEmpty()
                .MaximumLength(50);
        }*/
    }
}
