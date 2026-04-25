namespace SurveyBasket.Api.Errors;

public record QuestionErrors
{
    public static readonly Error QuestionNotFound =
        new("Question.NotFound", "No question was found with that given Id", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedQuestionContent =
        new("Question.DuplicatedContent", "Another question with same content is already exists", StatusCodes.Status409Conflict);
}
