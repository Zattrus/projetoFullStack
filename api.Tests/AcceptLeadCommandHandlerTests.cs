// AcceptLeadCommandHandlerTests.cs
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace api.Tests;

public class AcceptLeadCommandHandlerTests
{
    [Fact]
    public async Task Should_throw_when_lead_missing_or_not_new()
    {
        using var db = TestHelpers.BuildInMemoryDb(nameof(Should_throw_when_lead_missing_or_not_new));
        var email = new Mock<IEmailService>(MockBehavior.Strict);

        var handler = new AcceptLeadHandler(db, email.Object);

        var act = async () => await handler.Handle(
            new AcceptLeadCommand(Guid.NewGuid()),
            CancellationToken.None);

        await act.Should()
            .ThrowAsync<Exception>()
            .WithMessage("*not found*");
    }

    [Fact]
    public async Task Should_accept_lead_apply_discount_and_send_email()
    {
        using var db = TestHelpers.BuildInMemoryDb(nameof(Should_accept_lead_apply_discount_and_send_email));
        var lead = TestHelpers.SeedLeadNew(price: 650m);
        await db.Leads.AddAsync(lead);
        await db.SaveChangesAsync();

        var email = new Mock<IEmailService>();
        email.Setup(e => e.NotifyLeadCreatedAsync(It.IsAny<Lead>()))
             .Returns(Task.CompletedTask);

        var handler = new AcceptLeadHandler(db, email.Object);

        await handler.Handle(new AcceptLeadCommand(lead.Id), CancellationToken.None);

        var saved = await db.Leads.AsNoTracking().SingleAsync(x => x.Id == lead.Id);
        saved.Status.Should().Be(LeadStatus.Accepted);

        saved.Price.Should().Be(585m);

        email.Verify(e => e.NotifyLeadCreatedAsync(It.Is<Lead>(l => l.Id == lead.Id)), Times.Once);
    }
}
