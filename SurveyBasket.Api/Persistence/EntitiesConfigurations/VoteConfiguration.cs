namespace SurveyBasket.Api.Persistence.EntitiesConfigurations;

public class VoteConfiguration : IEntityTypeConfiguration<Vote>
{
    public void Configure(EntityTypeBuilder<Vote> builder)
    {
        // Unique Constraint on User on the same Poll
        builder.HasIndex(p => new { p.PollId, p.UserId }).IsUnique();

    }
}
