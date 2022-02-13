namespace Model.Extensions;

public static class ClaimsPrincipleExtension
{
    public static string GetUsername(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.Name)?.Value ??
               throw new Exception("Could not find a user with that username");
    }

    public static int GetUserId(this ClaimsPrincipal user)
    {
        var userdId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.Parse(userdId ?? throw new Exception("Could not find a used id"));
    }
}