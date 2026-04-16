namespace SurveyBasket.Api.Errors;

public static class PollErrors
{
    public static readonly Error PollNotFound =
        new("Poll.NotFound", "No poll was found with that given Id");

    public static readonly Error DuplicatedPollTitle =
        new("Poll.DuplicatedPollTitle", "Another poll with same title is already exists");
}
