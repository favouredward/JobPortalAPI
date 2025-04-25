using FluentValidation;
using JobPortalAPI.Models;

public class JobValidator : AbstractValidator<Job>
{
    public JobValidator()
    {
        RuleFor(j => j.Title).NotEmpty().WithMessage("Job title is required.");
        RuleFor(j => j.EmployerId).GreaterThan(0).WithMessage("Employer ID must be valid.");
        RuleFor(j => j.Deadline).GreaterThan(DateTime.UtcNow).WithMessage("Deadline must be in the future.");
    }
}
//builder.Services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<JobValidator>());


