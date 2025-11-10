using Library.Api.Application.Commands.Library;
using Library.Api.Application.Queries.Library;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SuperLibrary.Web.Extensions;

namespace Library.Api.Controllers;

[ApiController]
[Route("libraries")]
public class LibraryController : ControllerBase
{
    private readonly IMediator _mediator;

    public LibraryController(IMediator mediator) 
        => _mediator = mediator;
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAsync(int id, CancellationToken cancellationToken) 
        => (await _mediator.Send(new LibraryQuery(id), cancellationToken)).ToActionResult();
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateLibraryCommand request, CancellationToken cancellationToken)
        => (await _mediator.Send(request, cancellationToken)).ToActionResult();
}