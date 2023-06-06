using CommunityToolkit.Mvvm.Messaging;

namespace ICSProject.App.Services;

public interface IMessengerService
{
    IMessenger Messenger { get; }

    void Send<TMessage>(TMessage message)
        where TMessage : class;
}
