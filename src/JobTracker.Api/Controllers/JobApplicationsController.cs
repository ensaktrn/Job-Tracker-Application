using JobTracker.Application.Common;
using JobTracker.Application.JobApplications;
using Microsoft.AspNetCore.Mvc;
using JobTracker.Domain.Enums;

namespace JobTracker.Api.Controllers;

[ApiController]
[Route("api/applications")]
public sealed class JobApplicationsController : ControllerBase
{
    private readonly IJobApplicationService _service;

    public JobApplicationsController(IJobApplicationService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<PagedResult<JobApplicationDto>>> Get(
        [FromQuery] ApplicationStatus? status,
        [FromQuery] Guid? companyId,
        [FromQuery] DateTimeOffset? from,
        [FromQuery] DateTimeOffset? to,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sort = "appliedAt_desc",
        CancellationToken ct = default)
    {
        var query = new GetJobApplicationsQuery(status, companyId, from, to, page, pageSize, sort);
        return Ok(await _service.GetAsync(query, ct));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<JobApplicationDto>> GetById(Guid id, CancellationToken ct)
    {
        var app = await _service.GetByIdAsync(id, ct);
        return app is null ? NotFound() : Ok(app);
    }

    [HttpPost]
    public async Task<ActionResult<JobApplicationDto>> Create([FromBody] CreateJobApplicationRequest request, CancellationToken ct)
    {
        var created = await _service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}/status")]
    public async Task<ActionResult<JobApplicationDto>> ChangeStatus(Guid id, [FromBody] ChangeApplicationStatusRequest request, CancellationToken ct)
    {
        var updated = await _service.ChangeStatusAsync(id, request, ct);
        return Ok(updated);
    }
}