namespace SurveyBasket.Api.Errors;

public static class QuestionErrors
{
    public static readonly Error QuestionNotFound =
        new("Question.NotFound", "No question was found with that given Id");

    public static readonly Error DuplicatedQuestionContent =
        new("Question.DuplicatedContent", "Another question with same content is already exists");
}
