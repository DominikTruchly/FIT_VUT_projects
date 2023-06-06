namespace ICSProject.App.Messages;

public record ActivityDeleteMessage
{
    public required Guid UserId { get; init; }
}
