namespace Model.Entities.Identity;

public class AppRole : IdentityRole<int>
{
    public virtual ICollection<AppUserRole> UserRoles { get; set; }

    public AppRole() : base() { }
    public AppRole(string roleName) : base(roleName) { }
}