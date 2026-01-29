using JobTracker.Domain.Enums;

namespace JobTracker.Domain.Entities;

public sealed class Application
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid JobPostingId { get; private set; }
    public JobPosting? JobPosting { get; private set; } // navigation

    public ApplicationStatus Status { get; private set; } = ApplicationStatus.Applied;

    public DateTimeOffset AppliedAt { get; private set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset LastUpdatedAt { get; private set; } = DateTimeOffset.UtcNow;

    private Application() { }

    public Application(Guid jobPostingId)
    {
        if (jobPostingId == Guid.Empty)
            throw new ArgumentException("JobPostingId cannot be empty.", nameof(jobPostingId));

        JobPostingId = jobPostingId;
    }

    public void ChangeStatus(ApplicationStatus newStatus)
    {
        // Basit kural: Rejected/Withdrawn final; Offer final sayılabilir.
        var isFinal = Status is ApplicationStatus.Rejected or ApplicationStatus.Withdrawn;

        if (isFinal)
            throw new InvalidOperationException("Final status cannot be changed.");

        Status = newStatus;
        LastUpdatedAt = DateTimeOffset.UtcNow;
    }
}