using JobTracker.Application.JobPostings;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.Api.Controllers;

[ApiController]
[Route("api/job-postings")]
public sealed class JobPostingsController : ControllerBase
{
    private readonly IJobPostingService _service;

    public JobPostingsController(IJobPostingService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<JobPostingDto>>> GetAll(CancellationToken ct)
        => Ok(await _service.GetAllAsync(ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<JobPostingDto>> GetById(Guid id, CancellationToken ct)
    {
        var posting = await _service.GetByIdAsync(id, ct);
        return posting is null ? NotFound() : Ok(posting);
    }

    [HttpPost]
    public async Task<ActionResult<JobPostingDto>> Create([FromBody] CreateJobPostingRequest request, CancellationToken ct)
    {
        var created = await _service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
}