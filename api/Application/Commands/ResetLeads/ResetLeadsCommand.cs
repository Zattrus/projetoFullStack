using MediatR;

namespace api.Application.Commands.ResetLeads
{
    public sealed record ResetLeadsCommand() : IRequest<Unit>;
}
