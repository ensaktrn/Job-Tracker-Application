using JobTracker.Domain.Enums;

namespace JobTracker.Application.JobApplications;

public sealed record ChangeApplicationStatusRequest(
    ApplicationStatus Status
);