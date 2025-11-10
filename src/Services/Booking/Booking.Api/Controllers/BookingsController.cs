using Booking.Api.Application.Commands.Booking;
using Booking.Api.Application.Queries.Booking;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SuperLibrary.Web.Extensions;

namespace Booking.Api.Controllers;

[ApiController]
[Route("bookings")]
public class BookingsController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public BookingsController(IMediator mediator) 
        => _mediator = mediator;
    
    [HttpGet("{roomId:int}")]
    public async Task<IActionResult> GetBookingsAsync(int roomId, CancellationToken cancellationToken)
        => (await _mediator.Send(new BookingsQuery(roomId), cancellationToken)).ToActionResult();

    [HttpPost("event-driven-approach")]
    public async Task<IActionResult> CreateAsync(CreateBookingCommandEventDrivenApproach request, CancellationToken cancellationToken)
        => (await _mediator.Send(request, cancellationToken)).ToActionResult();
    
    [HttpPost("synchronous-approach")]
    public async Task<IActionResult> CreateAsync(CreateBookingCommandSynchronousApproach request, CancellationToken cancellationToken)
        => (await _mediator.Send(request, cancellationToken)).ToActionResult();
    
    [HttpPut]
    public async Task<IActionResult> UpdateAsync(UpdateBookingCommand request, CancellationToken cancellationToken)
        => (await _mediator.Send(request, cancellationToken)).ToActionResult();
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
        => (await _mediator.Send(new DeleteBookingCommand(id), cancellationToken)).ToActionResult();
}