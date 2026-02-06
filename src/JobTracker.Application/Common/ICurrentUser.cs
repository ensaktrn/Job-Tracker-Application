namespace JobTracker.Application.Common;

public interface ICurrentUser
{
    string UserId { get; }
    string UserEmail { get; }
    bool IsAuthenticated { get; }
}