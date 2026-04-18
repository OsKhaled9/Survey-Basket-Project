namespace SurveyBasket.Api.Errors;

public static class PollErrors
{
    public static readonly Error PollNotFound =
        new("Poll.NotFound", "No poll was found with that given Id", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedPollTitle =
        new("Poll.DuplicatedTitle", "Another poll with same title is already exists", StatusCodes.Status409Conflict);
}
