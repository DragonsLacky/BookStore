using Model.Enums;

namespace Model.Entities.Identity;

public class AppUser : IdentityUser<int>
{
    public DateTime DateOfBirth { get; set; }
    public DateTime Created { get; set; } = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
    public DateTime LastActive { get; set; } = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
    public string? Country { get; set; }
    public string? City { get; set; }
    public Cart Cart { get; set; }
    public virtual ICollection<UserPhoto> Photos { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
    public virtual ICollection<LikedBook> LikedBooks { get; set; }
    public virtual ICollection<AppUserRole> UserRoles { get; set; }
    public virtual ICollection<Message> MessagesSent { get; set; }
    public virtual ICollection<Message> MessagesReceived { get; set; }
    public virtual ICollection<Book> OwnedBooks { get; set; }
}