namespace Repository.Data;

public class SeedData
{
    public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        // if (await userManager.Users.AnyAsync()) return;

        // var userData = await System.IO.File.ReadAllTextAsync("data/userSeedData.json");
        // var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
        // if (users == null) return;

        if (await roleManager.Roles.AnyAsync()) return;
        
        foreach (var role in Enum.GetNames<AppRoleEnum>().Select(role => new AppRole(role)).ToList())
        {
            await roleManager.CreateAsync(role);
        }


        var memberRole = Enum.GetName(AppRoleEnum.User);
        var adminRole = Enum.GetName(AppRoleEnum.Admin);

        // foreach (var user in users)
        // {
        //     user.UserName = user.UserName.ToLower();
        //     // await userManager.CreateAsync(user, "P@ss10");
        //     // await userManager.AddToRoleAsync(user, memberRole);
        // }

        var admin = new AppUser
        {
            UserName = "admin"
        };
        await userManager.CreateAsync(admin, "P@ss10");
        await userManager.AddToRolesAsync(admin, (new[] {memberRole, adminRole}));
    }
}