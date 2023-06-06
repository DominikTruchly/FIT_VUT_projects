namespace ICSProject.App.Messages;

public record UserEditMessage
{
    public required Guid UserId { get; init; }
}
