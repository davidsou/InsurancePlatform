using Insurance.SharedKernel;
using Microsoft.AspNetCore.Mvc;
using ProposalService.Application.Ports.Inbound.Proposals;

namespace ProposalService.Api.Controllers;

[ApiController]
[Route("api/proposals")]
public sealed class ProposalsController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProposalRequest req, [FromServices] ICreateProposalUseCase uc, CancellationToken ct)
        => ToHttp(await uc.ExecuteAsync(req, ct), created: true);

    [HttpGet]
    public async Task<IActionResult> List([FromServices] IListProposalsUseCase uc, CancellationToken ct)
        => ToHttp(await uc.ExecuteAsync(ct), created: false);

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, [FromServices] IGetProposalUseCase uc, CancellationToken ct)
        => ToHttp(await uc.ExecuteAsync(id, ct), created: false);

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] ChangeProposalStatusRequest req, [FromServices] IChangeProposalStatusUseCase uc, CancellationToken ct)
        => ToHttp(await uc.ExecuteAsync(id, req, ct), created: false);

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
