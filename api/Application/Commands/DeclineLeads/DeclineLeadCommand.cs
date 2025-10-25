using MediatR;

namespace api.Application.Commands.DeclineLead
{
    public sealed record DeclineLeadCommand(Guid Id) : IRequest<Unit>;
}
