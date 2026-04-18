namespace SurveyBasket.Api.Persistence.EntitiesConfigurations;

public class VoteAnswerConfiguration : IEntityTypeConfiguration<VoteAnswer>
{
    public void Configure(EntityTypeBuilder<VoteAnswer> builder)
    {
        // Unique Constraint on Vote on the Question
        builder.HasIndex(p => new { p.VoteId, p.QuestionId }).IsUnique();

    }
}
