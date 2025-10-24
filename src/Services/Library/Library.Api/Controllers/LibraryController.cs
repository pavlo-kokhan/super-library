using Library.Api.Application.Queries.Library;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SuperLibrary.Web.Extensions;

namespace Library.Api.Controllers;

[ApiController]
[Route("libraries")]
public class LibraryController
{
    private readonly IMediator _mediator;

    public LibraryController(IMediator mediator) 
        => _mediator = mediator;
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAsync(int id, CancellationToken cancellationToken) 
        => (await _mediator.Send(new LibraryQuery(id), cancellationToken)).ToActionResult();
}