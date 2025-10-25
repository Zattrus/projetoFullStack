using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using api.Application.Commands.DeclineLead;

namespace api.Application.Commands.DeclineLead
{
  public sealed class DeclineLeadHandler : IRequestHandler<DeclineLeadCommand, Unit>
  {
    private readonly LeadDbContext _db;

    public DeclineLeadHandler(LeadDbContext db)
    {
        _db = db;
    }

    public async Task<Unit> Handle(DeclineLeadCommand request, CancellationToken cancellationToken)
    {

      var lead = await _db.Leads
          .FirstOrDefaultAsync(l => l.Id == request.Id && l.Status == LeadStatus.New, cancellationToken);

      if (lead is null)
      {
          throw new KeyNotFoundException("Lead not found or not in 'New' status.");
      }

      lead.Status = LeadStatus.Declined;

      await _db.SaveChangesAsync(cancellationToken);
      return Unit.Value;
    }
  }
}
