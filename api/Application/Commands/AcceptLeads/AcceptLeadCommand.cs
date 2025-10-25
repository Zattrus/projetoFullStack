using MediatR;

public record AcceptLeadCommand(Guid Id) : IRequest<Unit>;
