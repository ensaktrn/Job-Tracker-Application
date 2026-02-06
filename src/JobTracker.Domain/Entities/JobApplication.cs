using JobTracker.Domain.Enums;

namespace JobTracker.Domain.Entities;

public sealed class JobApplication
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string UserId { get; private set; } = string.Empty;
    public Guid JobPostingId { get; private set; }
    public JobPosting? JobPosting { get; private set; } // navigation

    public ApplicationStatus Status { get; private set; } = ApplicationStatus.Applied;
    public DateTimeOffset AppliedAt { get; private set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset LastUpdatedAt { get; private set; } = DateTimeOffset.UtcNow;

    private JobApplication() { }

    public JobApplication(string userId, Guid jobPostingId)
    {
        if (jobPostingId == Guid.Empty)
            throw new ArgumentException("JobPostingId cannot be empty.", nameof(jobPostingId));
        SetUser(userId);
        JobPostingId = jobPostingId;
    }
    private void SetUser(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("UserId is required.", nameof(userId));

        UserId = userId;
    }
    public void ChangeStatus(ApplicationStatus newStatus)
    {
        var isFinal = Status is ApplicationStatus.Rejected or ApplicationStatus.Withdrawn;

        if (isFinal)
            throw new InvalidOperationException("Final status cannot be changed.");

        Status = newStatus;
        LastUpdatedAt = DateTimeOffset.UtcNow;
    }
}