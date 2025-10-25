using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetLeadsHandler : IRequestHandler<GetLeadsQuery, IEnumerable<ILeadProjection>>
{
    private readonly LeadDbContext _db;
    public GetLeadsHandler(LeadDbContext db) => _db = db;

    public async Task<IEnumerable<ILeadProjection>> Handle(GetLeadsQuery request, CancellationToken ct)
    {
        if (request.Status == LeadStatus.New)
        {
            return await _db.Leads.AsNoTracking()
                .Where(l => l.Status == LeadStatus.New)
                .OrderByDescending(l => l.CreatedAt)
                .Select(l => new LeadListItemDto(
                    l.Id,
                    l.ContactFirstName,
                    l.ContactFullName,
                    l.ContactPhone,
                    l.ContactEmail,
                    l.Suburb,
                    l.Category,
                    l.Description,
                    l.Price,
                    l.CreatedAt,
                    l.Status
                ))
                .ToListAsync(ct);
        }

        if (request.Status == LeadStatus.Accepted)
        {
            return await _db.Leads.AsNoTracking()
                .Where(l => l.Status == LeadStatus.Accepted)
                .OrderByDescending(l => l.CreatedAt)
                .Select(l => new LeadAcceptedDto(
                    l.Id,
                    l.ContactFirstName,
                    l.ContactFullName,
                    l.ContactPhone,
                    l.ContactEmail,
                    l.Suburb,
                    l.Category,
                    l.Description,
                    l.Price,
                    l.CreatedAt,
                    l.Status
                ))
                .ToListAsync(ct);
        }

        return await _db.Leads.AsNoTracking()
            .Where(l => l.Status == LeadStatus.Declined)
            .OrderByDescending(l => l.CreatedAt)
            .Select(l => new LeadListItemDto(
                l.Id,
                l.ContactFirstName,
                l.ContactFullName,
                l.ContactPhone,
                l.ContactEmail,
                l.Suburb,
                l.Category,
                l.Description,
                l.Price,
                l.CreatedAt,
                l.Status
            ))
            .ToListAsync(ct);
    }
}
