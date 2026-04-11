namespace SurveyBasket.Api.Contracts.Polls;

public class LoginRequestValidator : AbstractValidator<PollRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Please, Add a {PropertyName}")
            .Length(3, 100)
            .WithMessage("Title should be at least {MinLength} and maximum {MaxLength}, you entered [{TotalLength}]");

        RuleFor(x => x.Summary)
            .NotEmpty()
            .Length(3, 1500);

        RuleFor(x => x.StartsAt)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));

        RuleFor(x => x.EndsAt)
           .NotEmpty();

        RuleFor(x => x)
            .Must(HasValidDates)
            .WithName(nameof(PollRequest.EndsAt))
            .WithMessage("{PropertyName} should be greater than or equals to start date");
    }

    private bool HasValidDates(PollRequest pollRequest)
    {
        return pollRequest.EndsAt >= pollRequest.StartsAt;
    }
}
