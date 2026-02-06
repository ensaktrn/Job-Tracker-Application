namespace JobTracker.Domain.Entities;

public sealed class JobPosting
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string UserId { get; private set; } = string.Empty;
    public Guid CompanyId { get; private set; }
    public Company? Company { get; private set; } // navigation

    public string Title { get; private set; } = string.Empty;
    public string Url { get; private set; } = string.Empty;
    public string? Notes { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;

    private JobPosting() { }

    public JobPosting(string userId, Guid companyId, string title, string url, string? notes)
    {
        if (companyId == Guid.Empty)
            throw new ArgumentException("CompanyId cannot be empty.", nameof(companyId));

        CompanyId = companyId;
        SetUser(userId);
        SetTitle(title);
        SetUrl(url);
        SetNotes(notes);
    }
    private void SetUser(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("UserId is required.", nameof(userId));

        UserId = userId;
    }
    public void SetTitle(string title)
    {
        title = (title ?? string.Empty).Trim();

        if (title.Length is < 2 or > 200)
            throw new ArgumentException("Title must be between 2 and 200 characters.", nameof(title));

        Title = title;
    }

    public void SetUrl(string url)
    {
        url = (url ?? string.Empty).Trim();

        if (url.Length is < 5 or > 1000)
            throw new ArgumentException("Url must be between 5 and 1000 characters.", nameof(url));

        // Basit bir kontrol; ileri aşamada Uri.TryCreate ile de doğrulayabiliriz.
        Url = url;
    }

    public void SetNotes(string? notes)
    {
        notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();

        if (notes is not null && notes.Length > 2000)
            throw new ArgumentException("Notes must be 2000 characters or less.", nameof(notes));

        Notes = notes;
    }
}