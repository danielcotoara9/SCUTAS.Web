using Microsoft.AspNetCore.Mvc;
using SCUTAS.Core.ContributorAggregate;
using SCUTAS.UseCases.Contributors;
using SCUTAS.UseCases.Contributors.Create;
using SCUTAS.UseCases.Contributors.Delete;
using SCUTAS.UseCases.Contributors.Get;
using SCUTAS.UseCases.Contributors.List;
using SCUTAS.UseCases.Contributors.Update;
using SCUTAS.Web.Contributors;

using FromBodyAttribute = Microsoft.AspNetCore.Mvc.FromBodyAttribute;
using FromQueryAttribute = Microsoft.AspNetCore.Mvc.FromQueryAttribute;
using HttpDeleteAttribute = Microsoft.AspNetCore.Mvc.HttpDeleteAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using HttpPutAttribute = Microsoft.AspNetCore.Mvc.HttpPutAttribute;

namespace SCUTAS.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContributorsController(IMediator mediator) : ControllerBase
{
  [HttpGet]
  public async Task<IActionResult> List(
    [FromQuery] int page = 1,
    [FromQuery] int perPage = UseCases.Constants.DEFAULT_PAGE_SIZE,
    CancellationToken cancellationToken = default)
  {
    var result = await mediator.Send(new ListContributorsQuery(page, perPage));
    if (!result.IsSuccess)
      return BadRequest(result.Errors);

    return Ok(result.Value);
  }

  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
  {
    var result = await mediator.Send(new GetContributorQuery(ContributorId.From(id)), cancellationToken);

    return result.Status switch
    {
      ResultStatus.Ok => Ok(new ContributorRecord(result.Value.Id.Value, result.Value.Name.Value, result.Value.PhoneNumber.ToString())),
      ResultStatus.NotFound => NotFound(),
      _ => Problem(detail: string.Join("; ", result.Errors), statusCode: StatusCodes.Status400BadRequest)
    };
  }

  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CreateContributorRequest request, CancellationToken cancellationToken = default)
  {
    if (!ModelState.IsValid)
      return BadRequest(ModelState);

    var result = await mediator.Send(new CreateContributorCommand(ContributorName.From(request.Name!), request.PhoneNumber), cancellationToken);

    return result.Status switch
    {
      ResultStatus.Ok => CreatedAtAction(nameof(GetById), new { id = result.Value.Value }, new { id = result.Value.Value, name = request.Name }),
      ResultStatus.Invalid => BadRequest(result.ValidationErrors),
      _ => Problem(detail: string.Join("; ", result.Errors), statusCode: StatusCodes.Status400BadRequest)
    };
  }

  [HttpPut("{id:int}")]
  public async Task<IActionResult> Update(int id, [FromBody] UpdateContributorRequest request, CancellationToken cancellationToken = default)
  {
    if (!ModelState.IsValid)
      return BadRequest(ModelState);

    var result = await mediator.Send(new UpdateContributorCommand(ContributorId.From(id), ContributorName.From(request.Name!)), cancellationToken);

    return result.Status switch
    {
      ResultStatus.Ok => Ok(new ContributorRecord(result.Value.Id.Value, result.Value.Name.Value, result.Value.PhoneNumber.ToString())),
      ResultStatus.NotFound => NotFound(),
      _ => Problem(detail: string.Join("; ", result.Errors), statusCode: StatusCodes.Status400BadRequest)
    };
  }

  [HttpDelete("{id:int}")]
  public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
  {
    var result = await mediator.Send(new DeleteContributorCommand(ContributorId.From(id)), cancellationToken);

    return result.Status switch
    {
      ResultStatus.Ok => NoContent(),
      ResultStatus.NotFound => NotFound(),
      _ => Problem(detail: string.Join("; ", result.Errors), statusCode: StatusCodes.Status400BadRequest)
    };
  }
}
