using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ICSProject.App.Messages;
using ICSProject.App.Services;
using ICSProject.BL.Facades;
using ICSProject.BL.Models;

namespace ICSProject.App.ViewModels;

[QueryProperty(nameof(User), nameof(User))]
public partial class UserEditViewModel : ViewModelBase, IRecipient<UserActivityEditMessage>, IRecipient<UserActivityAddMessage>, IRecipient<UserActivityDeleteMessage>
{
    private readonly IUserFacade _userFacade;
    private readonly INavigationService _navigationService;

    public UserDetailModel User { get; set; } = UserDetailModel.Empty;


    public UserEditViewModel(
        IUserFacade userFacade,
        INavigationService navigationService,
        IMessengerService messengerService)
        : base(messengerService)
    {
        _userFacade = userFacade;
        _navigationService = navigationService;

    }


    [RelayCommand]
    private async Task SaveAsync()
    {
        await _userFacade.SaveAsync(User with { UsrActivities = default! });

        MessengerService.Send(new UserEditMessage { UserId = User.Id });

        _navigationService.SendBackButtonPressed();
    }

    public async void Receive(UserActivityEditMessage message)
    {
        await ReloadDataAsync();
    }

    public async void Receive(UserActivityAddMessage message)
    {
        await ReloadDataAsync();
    }

    public async void Receive(UserActivityDeleteMessage message)
    {
        await ReloadDataAsync();
    }

    private async Task ReloadDataAsync()
    {
        User = await _userFacade.GetAsync(User.Id)
                 ?? UserDetailModel.Empty;
    }
}

