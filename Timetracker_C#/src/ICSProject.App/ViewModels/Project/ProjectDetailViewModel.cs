using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ICSProject.App.Messages;
using ICSProject.App.Services;
using ICSProject.BL.Facades;
using ICSProject.BL.Models;
using System.Collections.ObjectModel;
using ICSProject.BL.Mappers;

namespace ICSProject.App.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
[QueryProperty(nameof(User), nameof(User))]

public partial class ProjectDetailViewModel : ViewModelBase, IRecipient<ProjectEditMessage>
{
    private readonly IProjectFacade _projectFacade;
    private readonly IProjectModelMapper _projectModelMapper;
    private readonly IUserFacade _userFacade;
    private readonly IActivityFacade _activityFacade;

    private readonly INavigationService _navigationService;
    private readonly IAlertService _alertService;

    public Guid Id { get; set; }
    public UserDetailModel? User { get; set; }
    public ProjectDetailModel? Project { get; set; }

    public ObservableCollection<UserDetailModel> UsersInProject { get; set; } = new();
    public ObservableCollection<ActivityListModel> Activities { get; set; } = new();
    public ActivityListModel? SelectedActivity { get; set; }

    public ProjectDetailViewModel(
        IProjectFacade projectFacade,
       
        IProjectModelMapper projectModelMapper,
        IUserFacade userFacade,
        IActivityFacade activityFacade,
        INavigationService navigationService,
        IMessengerService messengerService,
        IAlertService alertService)
        : base(messengerService)

    {
        _projectFacade = projectFacade;
        _projectModelMapper = projectModelMapper;
        _userFacade = userFacade;
        _activityFacade = activityFacade;
        _navigationService = navigationService;
        _alertService = alertService;
    }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Project = await _projectFacade.GetProjectDetailsAsync(Id);

        UsersInProject.Clear();

        foreach (var activityInProject in Project.ProjActivities)
        {
            var activityUser = await _userFacade.GetAsync(activityInProject.UserId);
            if (activityUser is not null)
            {
                if (UsersInProject.Any(e => e.Id == activityUser.Id) is not true)
                {
                    UsersInProject.Add(activityUser);
                }
            }
        }
        Activities.Clear();
        if (User is not null)
        {
            var activities = User.UsrActivities;
            foreach (var activity in activities)
            {
                if (activity.ProjectId == null)
                {
                    Activities.Add(activity);
                }
            }
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (Project is not null)
        {
            if (Project.Name == "")
            {
                await _alertService.DisplayAsync("Name Error", "Name value not inserted");
            }
            else
            {
                await _projectFacade.UpdateAsync(Project);
                MessengerService.Send(new ProjectEditMessage { ProjectId = Project.Id });
                _navigationService.SendBackButtonPressed();
            }
        }
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (Project is not null)
        {
            await _projectFacade.DeleteAsync(Project.Id);

            MessengerService.Send(new ProjectDeleteMessage());

            _navigationService.SendBackButtonPressed();
        }
    }

    [RelayCommand]
    private async Task AddActivityToProjAsync()
    {
        if (Project is not null && User is not null && SelectedActivity is not null)
        {
            await _activityFacade.AddActivityToProjectAsync(SelectedActivity, User.Id, Project.Id);

            User.UsrProjects.Add(_projectModelMapper.MapToListModel(Project));
            MessengerService.Send(new ProjectEditMessage { ProjectId = Project.Id });
        }
    }

    public async void Receive(ProjectEditMessage message)
    {
        await LoadDataAsync();
    }
}
