using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ICSProject.App.Messages;
using ICSProject.App.Services;
using ICSProject.BL.Facades;
using ICSProject.BL.Models;


namespace ICSProject.App.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]

public partial class ActivityDetailViewModel : ViewModelBase, IRecipient<ActivityEditMessage>
{
    private readonly IActivityFacade _activityFacade;
    private readonly IProjectFacade _projectFacade;

    private readonly INavigationService _navigationService;
    private readonly IAlertService _alertService;

    public Guid Id { get; set; }
    public ActivityDetailModel? Activity { get; set; }
    public ProjectDetailModel Project { get; set; } = ProjectDetailModel.Empty;

    public TimeSpan StartTempTime { get; set; } = TimeSpan.Zero;
    public TimeSpan EndTempTime { get; set; } = TimeSpan.Zero;
    public TimeSpan ActivityDuration { get; set; } = TimeSpan.Zero;
    public DateTime Date { get; set; } = DateTime.MinValue;


    public ActivityDetailViewModel(
        IActivityFacade activityFacade,
        IProjectFacade projectFacade,
        INavigationService navigationService,
        IMessengerService messengerService,
        IAlertService alertService)
        : base(messengerService)
    {
        _activityFacade = activityFacade;
        _projectFacade = projectFacade;
        _navigationService = navigationService;
        _alertService = alertService;
    }
    
    [RelayCommand]
    private async Task SaveAsync()
    {
        if (Activity is not null)
        {
            if (Activity.Name == "")
            {
                await _alertService.DisplayAsync("Invalid Name", "Missing Name of activity.");
            }
            else
            {
                try
                {
                    Activity.StartDate = new DateTime(Date.Year, Date.Month, Date.Day, StartTempTime.Hours,
                        StartTempTime.Minutes, StartTempTime.Seconds);
                    Activity.EndDate = new DateTime(Date.Year, Date.Month, Date.Day, EndTempTime.Hours,
                        EndTempTime.Minutes, EndTempTime.Seconds);
                    await _activityFacade.UpdateAsync(Activity);
                    MessengerService.Send(new ActivityEditMessage());

                }
                catch (InvalidOperationException)
                {
                    await _alertService.DisplayAsync("Invalid Date and Time",
                        "Time or date is colliding with other activity.");
                }
            }
        }
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (Activity is not null)
        {
            await _activityFacade.DeleteAsync(Activity.Id);
            MessengerService.Send(new ActivityDeleteMessage { UserId = Activity.UserId });
            _navigationService.SendBackButtonPressed();
        }
    }

    [RelayCommand]
    private async Task DeleteFromProjAsync()
    {
        Activity!.ProjectId = null;
        await _activityFacade.UpdateAsync(Activity);
        MessengerService.Send(new ActivityEditMessage());
    }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        Activity = await _activityFacade.GetAsync(Id);

        if (Activity.ProjectId != null)
        {
            Project = await _projectFacade.GetProjByIdAsync((Guid)Activity.ProjectId);
        }
        else
        {
            Project.Name = "There is no project.";
        }

        if (Activity != null)
        {
            StartTempTime = new(Activity.StartDate.Hour, Activity.StartDate.Minute, Activity.StartDate.Second);
            EndTempTime = new(Activity.EndDate.Hour, Activity.EndDate.Minute, Activity.EndDate.Second);
            Date = new DateTime(Activity.StartDate.Year, Activity.StartDate.Month, Activity.StartDate.Day);
        }

        ActivityDuration = EndTempTime - StartTempTime;
    }

    public async void Receive(ActivityEditMessage message)
    {
        await LoadDataAsync();
    }
}

