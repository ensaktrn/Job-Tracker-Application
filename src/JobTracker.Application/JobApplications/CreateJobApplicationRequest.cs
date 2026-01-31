namespace JobTracker.Application.JobApplications;

public sealed record CreateJobApplicationRequest(
    Guid JobPostingId
);