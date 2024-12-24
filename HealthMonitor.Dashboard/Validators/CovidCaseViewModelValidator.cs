using FluentValidation;
using HealthMonitor.Dashboard.ViewModels;

namespace HealthMonitor.Dashboard.Validators
{
    public class CovidCaseViewModelValidator: AbstractValidator<CovidCaseViewModel>
    {
        public CovidCaseViewModelValidator()
        {
            RuleFor(item => item.State).NotEmpty()
                .WithMessage("State is required")
                .MaximumLength(2)
                .WithMessage("Only two characters allowed")
                .Matches("^[a-zA-Z]+$")
                .WithMessage("State must contain only letters.");
        }
    }
}
