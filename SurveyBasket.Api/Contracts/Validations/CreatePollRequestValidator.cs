namespace SurveyBasket.Api.Contracts.Validations;

public class CreatePollRequestValidator : AbstractValidator<CreatePollRequest>
{
    public CreatePollRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Please, Add a {PropertyName}")
            .Length(3, 100)
            .WithMessage("Title should be at least {MinLength} and maximum {MaxLength}, you entered [{TotalLength}]");

        RuleFor(x => x.Title)
            .NotEmpty()
            .Length(3, 1000);
    }
}
