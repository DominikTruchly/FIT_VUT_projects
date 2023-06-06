using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using ICSProject.App.Messages;
using ICSProject.App.Services;
using ICSProject.BL.Facades;
using ICSProject.BL.Mappers;
using ICSProject.BL.Models;

namespace ICSProject.App.ViewModels;

[QueryProperty(nameof(User), nameof(User))]
public partial class ProjectAddViewModel : ViewModelBase
{
    private readonly IProjectFacade _projectFacade;
    private readonly IActivityFacade _activityFacade;
    private readonly IProjectModelMapper _projectModelMapper;
    private readonly INavigationService _navigationService;
    private readonly IAlertService _alertService;

    public ProjectDetailModel Project { get; init; } = ProjectDetailModel.Empty;
    public UserDetailModel? User { get; set; }
    public ActivityListModel? SelectedActivity { get; set; }

    public ObservableCollection<ActivityListModel> Activities { get; set; } = new();
    public ProjectListModel? SelectedProjectListModel { get; set; }
    public ObservableCollection<ProjectListModel> Projects { get; set; } = new();

    public ProjectAddViewModel(
        IProjectFacade projectFacade,
        IActivityFacade activityFacade,
        IProjectModelMapper projectModelMapper,
        IAlertService alertService,
        INavigationService navigationService,
        IMessengerService messengerService)
        : base(messengerService)
    {
        _projectFacade = projectFacade;
        _activityFacade = activityFacade;
        _projectModelMapper = projectModelMapper;
        _alertService = alertService;
        _navigationService = navigationService;
    }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

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
            Projects.Clear();
            var projects = await _projectFacade.GetAsync();
            foreach (var project in projects)
            {
                Projects.Add(project);
            }
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (Project is not null && User is not null)
        {
            if (Project.Name == "")
            {
                await _alertService.DisplayAsync("Name Error", "Name value not inserted");
            }
            else
            {
                await _projectFacade.SaveAsync(Project, User.Id);
                User.UsrProjects.Add(_projectModelMapper.MapToListModel(Project));
                MessengerService.Send(new ProjectAddMessage());
                _navigationService.SendBackButtonPressed();
            }
        }
    }

    [RelayCommand]
    private async Task JoinProjectAsync()
    {
        if (SelectedProjectListModel is not null && User is not null && SelectedActivity is not null)
        {
            await _activityFacade.AddActivityToProjectAsync(SelectedActivity, User.Id, SelectedProjectListModel.Id);

            
            User.UsrProjects.Add(SelectedProjectListModel);
            MessengerService.Send(new ProjectAddMessage());
            _navigationService.SendBackButtonPressed();

        }
    }

}

