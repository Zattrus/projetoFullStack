using api.Application.Commands.DeclineLead;
using api.Application.Commands.ResetLeads;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class LeadsController : ControllerBase
{
  private readonly IMediator _mediator;

  public LeadsController(IMediator mediator)
  {
      _mediator = mediator;
  }

  [HttpGet]
  public async Task<IActionResult> Get([FromQuery] LeadStatus status = LeadStatus.New)
  {
      var leads = await _mediator.Send(new GetLeadsQuery(status));
      return Ok(leads);
  }

  [HttpPost("{id:guid}/Accept")]
  public async Task<IActionResult> Accept(Guid id)
  {
      await _mediator.Send(new AcceptLeadCommand(id));
      return NoContent();
  }

  [HttpPost("{id:guid}/Decline")]
  public async Task<IActionResult> Decline(Guid id)
  {
      try
      {
          await _mediator.Send(new DeclineLeadCommand(id));
          return NoContent();
      }
      catch (KeyNotFoundException)
      {
          return NotFound();
      }
  }
  [HttpPost("Reset")]
  public async Task<IActionResult> Reset() 
  {
    await _mediator.Send(new ResetLeadsCommand());
    return NoContent();
  }
}
