using MediatR;
using Microsoft.EntityFrameworkCore;


namespace api.Application.Commands.ResetLeads
{
    public sealed class ResetLeadsHandler : IRequestHandler<ResetLeadsCommand, Unit>
    {
        private readonly LeadDbContext _db;

        public ResetLeadsHandler(LeadDbContext db)
        {
            _db = db;
        }

        public async Task<Unit> Handle(ResetLeadsCommand request, CancellationToken cancellationToken)
        {
            var leads = await _db.Leads.ToListAsync(cancellationToken);

            foreach (var lead in leads)
            {
                lead.Status = LeadStatus.New;
            }

            await _db.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
