namespace JobTracker.Domain.Entities;

public sealed class Company
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Name { get; private set; } = string.Empty;
    public string? Website { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;

    // EF Core için
    private Company() { }

    public Company(string name, string? website)
    {
        SetName(name);
        SetWebsite(website);
    }

    public void SetName(string name)
    {
        name = (name ?? string.Empty).Trim();

        if (name.Length is < 2 or > 200)
            throw new ArgumentException("Company name must be between 2 and 200 characters.", nameof(name));

        Name = name;
    }

    public void SetWebsite(string? website)
    {
        website = string.IsNullOrWhiteSpace(website) ? null : website.Trim();

        if (website is not null && website.Length > 500)
            throw new ArgumentException("Website must be 500 characters or less.", nameof(website));

        Website = website;
    }
}