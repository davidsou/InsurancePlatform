using Insurance.SharedKernel;
using Microsoft.AspNetCore.Mvc;
using UnderwritingService.Application.Ports.Inbound.Underwritings;

namespace UnderwritingService.Api.Controllers;

[ApiController]
[Route("api/underwritings")]
public sealed class UnderwritingsController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> ManualCreate([FromBody] CreateUnderwritingManualRequest req, [FromServices] ICreateUnderwritingManualUseCase uc, CancellationToken ct)
        => ToHttp(await uc.ExecuteAsync(req, ct), created: true);

    [HttpGet]
    public async Task<IActionResult> List([FromServices] IListUnderwritingsUseCase uc, CancellationToken ct)
        => ToHttp(await uc.ExecuteAsync(ct), created: false);

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, [FromServices] IGetUnderwritingUseCase uc, CancellationToken ct)
        => ToHttp(await uc.ExecuteAsync(id, ct), created: false);

    [HttpGet("by-proposal/{proposalId:guid}")]
    public async Task<IActionResult> GetByProposal(Guid proposalId, [FromServices] IGetByProposalUseCase uc, CancellationToken ct)
        => ToHttp(await uc.ExecuteAsync(proposalId, ct), created: false);

    private IActionResult ToHttp(OperationResult result, bool created)
        => result.IsSuccess ? (created ? StatusCode(201) : Ok()) : Problem(result.Error!.Message, statusCode: Map(result.Error.Code));

    private IActionResult ToHttp<T>(OperationResult<T> result, bool created)
        => result.IsSuccess ? (created ? StatusCode(201, result.Value) : Ok(result.Value))
                            : Problem(result.Error!.Message, statusCode: Map(result.Error.Code));

    private static int Map(string code) => code switch
    {
        "validation" => 400,
        "not_found" => 404,
        "conflict" => 409,
        "business_rule" => 422,
        _ => 500
    };
}
