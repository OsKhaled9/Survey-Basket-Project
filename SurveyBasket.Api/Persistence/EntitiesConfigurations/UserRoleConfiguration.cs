namespace SurveyBasket.Api.Persistence.EntitiesConfigurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        // Default Data (Should assign Id even if it is auto generated)
        builder.HasData(
            new IdentityUserRole<string>()
            {
                UserId = DefaultUsers.Admin.Id,
                RoleId = DefaultRoles.Admin.Id
            }
        );
    }
}
