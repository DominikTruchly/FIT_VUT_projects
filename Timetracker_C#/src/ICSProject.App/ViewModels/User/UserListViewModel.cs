﻿using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ICSProject.App.Messages;
using ICSProject.App.Services;
using ICSProject.BL.Facades;
using ICSProject.BL.Models;

namespace ICSProject.App.ViewModels;

public partial class UserListViewModel : ViewModelBase, IRecipient<UserEditMessage>, IRecipient<UserDeleteMessage>
{
    private readonly IUserFacade _userFacade;
    private readonly INavigationService _navigationService;

    public IEnumerable<UserListModel> Users { get; set; } = null!;

    public UserListViewModel(
        IUserFacade userFacade,
        INavigationService navigationService,
        IMessengerService messengerService)
        : base(messengerService)
    { 
        _userFacade = userFacade;
        _navigationService = navigationService;
    }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Users = await _userFacade.GetAsync();
    }
    
    [RelayCommand]
    private async Task GoToDetailAsync(Guid id)
        => await _navigationService.GoToAsync<UserDetailViewModel>(
            new Dictionary<string, object?> { [nameof(UserDetailViewModel.Id)] = id });
    
    [RelayCommand]
    private async Task GoToCreateAsync()
    {
        await _navigationService.GoToAsync("/userAdd");
    }

    public async void Receive(UserEditMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(UserDeleteMessage message)
    {
        await LoadDataAsync();
    }
}
