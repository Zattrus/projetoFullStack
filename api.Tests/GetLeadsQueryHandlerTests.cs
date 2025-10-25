using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace api.Tests;

public class GetLeadsQueryHandlerTests
{
    [Fact]
    public async Task Should_return_only_new_leads_when_status_not_provided()
    {
        using var db = TestHelpers.BuildInMemoryDb(nameof(Should_return_only_new_leads_when_status_not_provided));
        db.Leads.AddRange(
            TestHelpers.SeedLeadNew(firstName: "Bill"),
            new Lead { Status = LeadStatus.Accepted, ContactFirstName="Ana", Suburb="Utrecht", Category="Electrical", Description="x", Price=450m }
        );
        await db.SaveChangesAsync();

        var handler = new GetLeadsHandler(db);
        var result = await handler.Handle(new GetLeadsQuery(LeadStatus.New), CancellationToken.None);

        var list = result.Should()
            .BeAssignableTo<IEnumerable<LeadListItemDto>>()
            .Subject.ToList();

        list.Should().HaveCount(1);
        list.Single().ContactFirstName.Should().Be("Bill");
    }

    [Fact]
    public async Task Should_return_accepted_projection_when_status_accepted()
    {
        using var db = TestHelpers.BuildInMemoryDb(nameof(Should_return_accepted_projection_when_status_accepted));
        var lead = TestHelpers.SeedLeadNew(firstName: "Ana", price: 600m);
        lead.Status = LeadStatus.Accepted;
        lead.ContactEmail = "ana@test.com";
        lead.ContactPhone = "123";
        lead.Category = "Plumbing";
        await db.Leads.AddAsync(lead);
        await db.SaveChangesAsync();

        var handler = new GetLeadsHandler(db);
        var result = await handler.Handle(new GetLeadsQuery(LeadStatus.Accepted), CancellationToken.None);

        var list = result.Should()
            .BeAssignableTo<IEnumerable<LeadAcceptedDto>>()
            .Subject.ToList();

        list.Should().HaveCount(1);
        var item = list.Single();
        item.ContactFullName.Should().NotBeNullOrEmpty();
        item.Price.Should().Be(600m);
        item.Category.Should().Be("Plumbing");
    }
}
