namespace SurveyBasket.Api.Errors;

public record VoteErrors
{
    public static readonly Error InvalidQuestions =
        new("Vote.InvalidQuestions", "Invalid questions", StatusCodes.Status400BadRequest);

    public static readonly Error DuplicatedVote =
        new("Vote.DuplicatedVote", "This user is already voted before for this poll", StatusCodes.Status409Conflict);
}
