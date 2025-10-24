using Booking.Api.Application.Commands.Booking;
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

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateBookingCommand request, CancellationToken cancellationToken)
        => (await _mediator.Send(request, cancellationToken)).ToActionResult();
}