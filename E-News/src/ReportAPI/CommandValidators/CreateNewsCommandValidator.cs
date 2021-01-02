using FluentValidation;
using ReportAPI.Commands;

namespace ReportAPI.CommandValidators
{
    public class CreateNewsCommandValidator : AbstractValidator<CreateNewsCommand>
    {
        // TODO : use localization-service / constants for message(s)
        // Error Code(s) must be unique because of trace-ability
        public CreateNewsCommandValidator()
        {
            RuleFor(x => x.AgencyCode).NotNull().NotEmpty().WithMessage("AgentCode could not be null or empty!")
                .WithErrorCode("ER1001");
            RuleFor(x => x.NewsContent).NotNull().NotEmpty().WithMessage("NewContent could not be null or empty!")
                .WithErrorCode("ER1002");
        }
    }

}