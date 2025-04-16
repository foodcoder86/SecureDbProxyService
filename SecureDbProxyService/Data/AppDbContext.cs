//using Microsoft.EntityFrameworkCore;

//public class AppDbContext : DbContext
//{
//    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

//    public DbSet<Customer> Customers { get; set; }
//}


using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<UserAppTelemetry> UserAppTelemetry { get; set; }
    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserAppTelemetry>()
            .HasKey(u => u.Id);  // Ensure EF knows the primary key

        modelBuilder.Entity<UserAppTelemetry>()
            .Property(u => u.EntryDateTime)
            .HasDefaultValueSql("GETDATE()"); // SQL default timestamp

        base.OnModelCreating(modelBuilder);
    }
}
