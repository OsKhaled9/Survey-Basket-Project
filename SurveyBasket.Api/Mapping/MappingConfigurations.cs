using Mapster;
using SurveyBasket.Api.Contracts.Polls;

namespace SurveyBasket.Api.Mapping;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<IEnumerable<Poll>, IEnumerable<PollResponse>>();
        config.NewConfig<Poll, PollResponse>();
        config.NewConfig<PollRequest, Poll>();
    }
}
