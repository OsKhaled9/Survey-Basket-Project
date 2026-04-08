using Mapster;
using SurveyBasket.Api.Contracts.Requests;
using SurveyBasket.Api.Contracts.Responses;

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
