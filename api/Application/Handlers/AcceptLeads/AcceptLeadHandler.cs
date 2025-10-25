using MediatR;
using Microsoft.EntityFrameworkCore;

public class AcceptLeadHandler : IRequestHandler<AcceptLeadCommand, Unit>
{
    private readonly LeadDbContext _db;
    private readonly IEmailService _email;

    public AcceptLeadHandler(LeadDbContext db, IEmailService email)
    {
        _db = db;
        _email = email;
    }

    public async Task<Unit> Handle(AcceptLeadCommand request, CancellationToken cancellationToken)
    {
        var lead = await _db.Leads.FirstOrDefaultAsync(l => l.Id == request.Id && l.Status == LeadStatus.New);
        if (lead is null) throw new Exception("Lead not found or already processed.");

        if (lead.Price > 500m) lead.Price = Math.Round(lead.Price * 0.9m, 2, MidpointRounding.AwayFromZero);

        lead.Status = LeadStatus.Accepted;
        await _db.SaveChangesAsync(cancellationToken);

        await _email.NotifyLeadCreatedAsync(lead);

        return Unit.Value;
    }
}
