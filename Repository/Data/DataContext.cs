namespace Repository.Data;

public class DataContext : IdentityDbContext<AppUser, AppRole, int,
    IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
    IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public DbSet<Comment> Comments { get; set; }
    public DbSet<LikedBook> LikedBooks { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Connection> Connections { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppUser>()
            .HasMany(user => user.UserRoles)
            .WithOne(userRole => userRole.User)
            .HasForeignKey(userRole => userRole.UserId)
            .IsRequired();

        builder.Entity<AppRole>()
            .HasMany(role => role.UserRoles)
            .WithOne(userRole => userRole.Role)
            .HasForeignKey(userRole => userRole.RoleId)
            .IsRequired();

        builder.Entity<Message>()
            .HasOne(message => message.Recipient)
            .WithMany(user => user.MessagesReceived)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Message>()
            .HasOne(message => message.Sender)
            .WithMany(user => user.MessagesSent)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Entity<LikedBook>()
            .HasKey(k => new {k.UserId, k.BookId});

        builder.Entity<LikedBook>()
            .HasOne(lb => lb.Book)
            .WithMany(b => b.LikedBy)
            .HasForeignKey(b => b.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<LikedBook>()
            .HasOne(lb => lb.User)
            .WithMany(b => b.LikedBooks)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Comment>()
            .HasOne(c => c.Book)
            .WithMany(b => b.Comments)
            .HasForeignKey(c => c.BookId);

        builder.Entity<Comment>()
            .HasOne(c => c.Commenter)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.CommenterId);
    }
}