using CommunityToolkit.Mvvm.Input;
using ICSProject.App.Services;
using ICSProject.App.ViewModels;

namespace ICSProject.App;

public partial class AppShell
{
    private readonly INavigationService _navigationService;
    
    public AppShell(INavigationService navigationService)
    {
        _navigationService = navigationService;

        InitializeComponent();
    }

    [RelayCommand]
    private async Task GoToUsersAsync()
        => await _navigationService.GoToAsync<UserListViewModel>();

    /*[RelayCommand]
    private async Task GoToActivityAsync()
        => await _navigationService.GoToAsync<ActivityListViewModel>();

    [RelayCommand]
    private async Task GoToProjectAsync()
        => await _navigationService.GoToAsync<ProjectViewModel>();*/
}
