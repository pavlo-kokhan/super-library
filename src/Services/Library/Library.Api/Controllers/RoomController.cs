using Library.Api.Application.Queries.Room;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SuperLibrary.Web.Extensions;

namespace Library.Api.Controllers;

[ApiController]
[Route("rooms")]
public class RoomController
{
    private readonly IMediator _mediator;

    public RoomController(IMediator mediator) 
        => _mediator = mediator;

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAsync(int id, CancellationToken cancellationToken) 
        => (await _mediator.Send(new RoomQuery(id), cancellationToken)).ToActionResult();
}