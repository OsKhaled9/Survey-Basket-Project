namespace SurveyBasket.Api.Persistence.EntitiesConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.OwnsMany(x => x.RefreshTokens)
               .ToTable("RefreshTokens")
               .WithOwner()
               .HasForeignKey("UserId");

        builder.Property(x => x.FirstName).HasMaxLength(100);
        builder.Property(x => x.LastName).HasMaxLength(100);

        // Default Data (Should assign Id even if it is auto generated)
        var passwordHasher = new PasswordHasher<ApplicationUser>();

        builder.HasData(new ApplicationUser()
        {
            Id = DefaultUsers.AdminId,
            FirstName = "Survey Basket",
            LastName = "Admin",
            UserName = DefaultUsers.AdminEmail,
            NormalizedUserName = DefaultUsers.AdminEmail.ToUpper(),
            Email = DefaultUsers.AdminEmail,
            NormalizedEmail = DefaultUsers.AdminEmail.ToUpper(),
            SecurityStamp = DefaultUsers.AdminSecurityStamp,
            ConcurrencyStamp = DefaultUsers.AdminConcurrencyStamp,
            EmailConfirmed = true,
            //PasswordHash = passwordHasher.HashPassword(null!, DefaultUsers.AdminPassword)
            PasswordHash = "AQAAAAIAAYagAAAAEDpo+gw0TS4X1EIYVBGrTuebRwo4nrn0aILem8WA5J1AXfhABl1t0HXIdpLPWVKm2w=="
        });
    }
}
