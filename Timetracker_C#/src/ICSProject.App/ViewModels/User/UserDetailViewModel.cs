using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ICSProject.App.Messages;
using ICSProject.App.Services;
using ICSProject.BL.Facades;
using ICSProject.BL.Mappers;
using ICSProject.BL.Models;
using ProjectICS.App.Messages;

namespace ICSProject.App.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class UserDetailViewModel : ViewModelBase, IRecipient<UserEditMessage>, IRecipient<ActivityEditMessage>,
    IRecipient<ProjectAddMessage>, IRecipient<ProjectEditMessage>,
    IRecipient<ProjectDeleteMessage>, IRecipient<ActivityDeleteMessage>
{
    private readonly INavigationService _navigationService;
    private readonly IAlertService _alertService;
    private readonly IMessengerService _messengerService;

    private readonly IProjectModelMapper _projectModelMapper;

    private readonly IUserFacade _userFacade;
    private readonly IProjectFacade _projectFacade;
    private readonly IActivityFacade _activityFacade;

    public Guid Id { get; set; }
    public UserDetailModel User { get; set; }
    public ObservableCollection<ProjectDetailModel> Projects { get; set; } = new();
    public IEnumerable<ActivityListModel> Activities { get; set; } = null!;

    public UserDetailViewModel(
        IUserFacade userFacade,
        IProjectFacade projectFacade,
        IActivityFacade activityFacade,
        IProjectModelMapper projectModelMapper,
        INavigationService navigationService,
        IMessengerService messengerService,
        IAlertService alertService)
        : base(messengerService)
    {
        _userFacade = userFacade;
        _projectFacade = projectFacade;
        _activityFacade = activityFacade;
        _projectModelMapper = projectModelMapper;
        _navigationService = navigationService;
        _messengerService = messengerService;
        _alertService = alertService;

    }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        User = await _userFacade.GetAsync(Id);

        foreach (var userActivity in User.UsrActivities)
        {
            if (userActivity.ProjectId is not null)
            {
                var tmpProj = await _projectFacade.GetAsync((Guid)userActivity.ProjectId);
                if (tmpProj is not null)
                {
                    if (User.UsrProjects.Contains(_projectModelMapper.MapToListModel(tmpProj)) is not true)
                    {
                        User.UsrProjects.Add(_projectModelMapper.MapToListModel(tmpProj));
                    }
                }
            }
        }
       
    }

    [RelayCommand]
    private void LogOut()
    {
        MessengerService.Send(new RefreshMessage());
        _navigationService.SendBackButtonPressed();
    }

    [RelayCommand]
    private async Task GoToProfileAsync()
    {
        if (User is not null)
        {
            await _navigationService.GoToAsync("/profile",
                new Dictionary<string, object?> { [nameof(UserProfileViewModel.User)] = User with { } });
        }
    }

    [RelayCommand]
    private async Task GoToAddActivityAsync()
    {
        if (User is not null)
        {
            await _navigationService.GoToAsync("/activityAdd",
                new Dictionary<string, object?> { [nameof(ActivityAddViewModel.User)] = User with { } });
        }
    }

    [RelayCommand]
    private async Task GoToAddProjectAsync()
    {
        if (User is not null)
        {
            await _navigationService.GoToAsync("/projectAdd",
                new Dictionary<string, object?> { [nameof(ProjectAddViewModel.User)] = User with { } });
        }
    }

    [RelayCommand]
    private async Task GoToActivityAsync(Guid id)
        => await _navigationService.GoToAsync<ActivityDetailViewModel>(
            new Dictionary<string, object?> { [nameof(ActivityDetailViewModel.Id)] = id });

    [RelayCommand]
    private async Task GoToActivityDetailAsync(Guid id)
        => await _navigationService.GoToAsync<ActivityDetailViewModel>(
            new Dictionary<string, object?> { [nameof(ActivityDetailViewModel.Id)] = id });

    [RelayCommand]
    private async Task GoToProjectDetailAsync(Guid id)
        => await _navigationService.GoToAsync<ProjectDetailViewModel>(
            new Dictionary<string, object?>
            {
                [nameof(ProjectDetailViewModel.Id)] = id,
                [nameof(ProjectDetailViewModel.User)] = User with { }
            });

    public async void Receive(ActivityEditMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(ActivityDeleteMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(ProjectAddMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(ProjectDeleteMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(UserEditMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(ProjectEditMessage message)
    {
        await LoadDataAsync();
    }
}

