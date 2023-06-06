using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ICSProject.App.Messages;
using ICSProject.App.Services;
using ICSProject.BL.Facades;
using ICSProject.BL.Models;

namespace ICSProject.App.ViewModels;

[QueryProperty(nameof(User), nameof(User))]
public partial class UserAddViewModel : ViewModelBase
{
    private readonly IUserFacade _userFacade;
    private readonly IAlertService _alertService;
    private readonly INavigationService _navigationService;

    public UserDetailModel User { get; set; } = UserDetailModel.Empty;

    public UserAddViewModel(
        IUserFacade userFacade,
        INavigationService navigationService,
        IMessengerService messengerService,
        IAlertService alertService)
        : base(messengerService)

    {
        _userFacade = userFacade;
        _navigationService = navigationService;
        _alertService = alertService;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (User.Name == "" || User.Surname == "")
        {
            await _alertService.DisplayAsync("Invalid Name", "Filed with Name or Surname has been left blank.");
        }
        else
        {
            await _userFacade.SaveAsync(User);
            MessengerService.Send(new UserEditMessage { UserId = User.Id });
            _navigationService.SendBackButtonPressed();
        }
    }
}
