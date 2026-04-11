namespace SurveyBasket.Api;

public static class UserSeeder
{
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
    {
        var usersCount = await userManager.Users.CountAsync();

        if (usersCount <= 0)
        {
            var defaultUser = new ApplicationUser()
            {
                UserName = "admin",
                Email = "admin@test.com",
                EmailConfirmed = false,
                FirstName = "Osama",
                LastName = "Khaled"
            };

            await userManager.CreateAsync(defaultUser, "Admin@123");
        }
    }
}
