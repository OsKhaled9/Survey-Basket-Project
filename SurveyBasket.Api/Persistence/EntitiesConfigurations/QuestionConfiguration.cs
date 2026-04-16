namespace SurveyBasket.Api.Persistence.EntitiesConfigurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        // Unique Constraint on same question(content) with same poll
        builder.HasIndex(p => new { p.PollId, p.Content }).IsUnique();

        builder.Property(p => p.Content).HasMaxLength(1000);
    }
}
