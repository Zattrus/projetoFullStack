using System;
using Microsoft.EntityFrameworkCore;

namespace api.Tests;

public static class TestHelpers
{
    public static LeadDbContext BuildInMemoryDb(string dbName)
    {
        var options = new DbContextOptionsBuilder<LeadDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .EnableSensitiveDataLogging()
            .Options;

        var db = new LeadDbContext(options);

        db.Database.EnsureCreated();
        db.Leads.RemoveRange(db.Leads);
        db.SaveChanges();

        
        return db;
    }

    public static Lead SeedLeadNew
    (
        string firstName = "Bill",
        decimal price = 62m,
        string suburb = "Yanderra 2574",
        string category = "Painters",
        string description = "Sample description",
        DateTime? createdAt = null
    )
    {
        return new Lead
        {
            Id = Guid.NewGuid(),
            ContactFirstName = firstName,
            ContactLastName = "",
            ContactPhone = "+61 2 5555 000",
            ContactEmail = "demo@example.com",
            Description = "Sample description",
            Suburb = suburb,
            Category = category,
            Price = price,
            CreatedAt = DateTime.UtcNow.AddMinutes(-5),
            Status = LeadStatus.New
        };
    }
}
