using MediatR;

public record GetLeadsQuery(LeadStatus Status) : IRequest<IEnumerable<ILeadProjection>>;
