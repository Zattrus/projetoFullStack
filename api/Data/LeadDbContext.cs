using Microsoft.EntityFrameworkCore;

public class LeadDbContext : DbContext {
  public LeadDbContext(DbContextOptions<LeadDbContext> opt) : base(opt) {}
  public DbSet<Lead> Leads => Set<Lead>();

  protected override void OnModelCreating(ModelBuilder b) {
    b.Entity<Lead>(e => {
        e.Property(p => p.Price).HasColumnType("decimal(10,2)");
        e.Property(p => p.Category).HasMaxLength(100);
        e.Property(p => p.Suburb).HasMaxLength(100);
        e.Property(p => p.Status).HasConversion<int>();
        e.HasIndex(p => p.Status);
        e.HasData(Seed());
    });
  }

  private static IEnumerable<Lead> Seed() {
    var now = DateTime.UtcNow;
      return new[] {
          new Lead
          {
            Id = Guid.NewGuid(),
            ContactFirstName = "Alice Johnson",
            ContactLastName = "Johnson",
            ContactPhone = "+31 6 1234 5678",
            ContactEmail = "alice@test.com",
            Category = "Electrical",
            Suburb = "Amsterdam",
            Description = "Install new lighting",
            Price = 1200,
            CreatedAt = now.AddDays(-3),
            Status = LeadStatus.New
          },
          new Lead
          {
            Id = Guid.NewGuid(),
            ContactFirstName = "Bob Smith",
            ContactLastName = "Smith",
            ContactPhone = "+31 6 8765 4321",
            ContactEmail = "bob@test.com",
            Category = "Plumbing",
            Suburb = "Rotterdam",
            Description = "Fix leaking sink",
            Price = 300,
            CreatedAt = now.AddDays(-2),
            Status = LeadStatus.New
          },
      };
  }
}
