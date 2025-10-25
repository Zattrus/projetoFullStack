using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using api.Application.Commands.DeclineLead;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace api.Tests;

public class DeclineLeadCommandHandlerTests
{
    [Fact]
    public async Task Should_decline_only_when_new()
    {
        using var db = TestHelpers.BuildInMemoryDb(nameof(Should_decline_only_when_new));
        var lead = TestHelpers.SeedLeadNew();
        await db.Leads.AddAsync(lead);
        await db.SaveChangesAsync();

        var handler = new DeclineLeadHandler(db);

        await handler.Handle(new DeclineLeadCommand(lead.Id), CancellationToken.None);

        var saved = await db.Leads.AsNoTracking().SingleAsync(x => x.Id == lead.Id);
        saved.Status.Should().Be(LeadStatus.Declined);
    }

    [Fact]
    public async Task Should_throw_when_lead_missing_or_not_new()
    {
        using var db = TestHelpers.BuildInMemoryDb(nameof(Should_throw_when_lead_missing_or_not_new));
        var handler = new DeclineLeadHandler(db);

        var act = async () => await handler.Handle(
            new DeclineLeadCommand(Guid.NewGuid()), CancellationToken.None);

        await act.Should()
            .ThrowAsync<KeyNotFoundException>()
            .WithMessage("*not found*");
    }
}
