namespace SurveyBasket.Api.Persistence.EntitiesConfigurations;

public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        // Unique Constraint on same answer(content) with same question
        builder.HasIndex(p => new { p.QuestionId, p.Content }).IsUnique();

        builder.Property(p => p.Content).HasMaxLength(1000);
    }
}
