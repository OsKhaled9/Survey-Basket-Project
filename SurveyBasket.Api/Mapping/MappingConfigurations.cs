namespace SurveyBasket.Api.Mapping;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<IEnumerable<Poll>, IEnumerable<PollResponse>>();
        config.NewConfig<Poll, PollResponse>();
        config.NewConfig<PollRequest, Poll>();

        config.NewConfig<QuestionRequest, Question>()
            .Map(dest => dest.Answers, src => src.Answers.Select(answer => new Answer() { Content = answer }));
    }
}
