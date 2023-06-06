using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ICSProject.App.Messages;
using ICSProject.App.Services;
using ICSProject.BL.Facades;
using ICSProject.BL.Models;

namespace ICSProject.App.ViewModels;

[QueryProperty(nameof(User), nameof(User))]
public partial class UserProfileViewModel : ViewModelBase
{
    private readonly IUserFacade _userFacade;
    private readonly INavigationService _navigationService;
    private readonly IAlertService _alertService;

    public UserDetailModel User { get; set; }

    public UserProfileViewModel(
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

    protected override async Task LoadDataAsync()
    {
        User = await _userFacade.GetAsync(User.Id) ?? UserDetailModel.Empty;
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (User is not null)
        {
            await _userFacade.DeleteAsync(User.Id);
            MessengerService.Send(new UserDeleteMessage());
            _navigationService.SendBackButtonPressed();

            await _navigationService.GoToAsync("//users");
        }
    }
    
    [RelayCommand]
    private async Task SaveAsync()
    {
        if (User.Name == "" || User.Surname == "")
        {
            await _alertService.DisplayAsync("Name Error", "Name or Surname value not inserted");
        }
        else
        {
            await _userFacade.UpdateAsync(User);
            MessengerService.Send(new UserEditMessage { UserId = User.Id });
            _navigationService.SendBackButtonPressed();
        }
    }

    public async void Receive(ProjectAddMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(ProjectDeleteMessage message)
    {
        await LoadDataAsync();
    }
}

