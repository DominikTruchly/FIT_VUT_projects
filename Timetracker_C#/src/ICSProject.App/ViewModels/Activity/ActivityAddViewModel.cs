using CommunityToolkit.Mvvm.Input;
using ICSProject.App.Messages;
using ICSProject.App.Services;
using ICSProject.BL.Facades;
using ICSProject.BL.Mappers;
using ICSProject.BL.Models;

namespace ICSProject.App.ViewModels;

[QueryProperty(nameof(User), nameof(User))]

public partial class ActivityAddViewModel : ViewModelBase
{
    private readonly IActivityFacade _activityFacade;
    private readonly IActivityModelMapper _activityModelMapper;

    private readonly INavigationService _navigationService;
    private readonly IAlertService _alertService;

    public UserDetailModel? User { get; set; }
    public ActivityDetailModel Activity { get; init; } = ActivityDetailModel.Empty;
    public TimeSpan StartTempTime { get; init; } = TimeSpan.Zero;
    public TimeSpan EndTempTime { get; init; } = TimeSpan.Zero;

    public ActivityAddViewModel(
        IActivityFacade activityFacade,
        IActivityModelMapper activityModelMapper,
        INavigationService navigationService,
        IMessengerService messengerService,
        IAlertService alertService)
        : base(messengerService)
    {
        _activityFacade = activityFacade;
        _navigationService = navigationService;
        _activityModelMapper = activityModelMapper;
        _alertService = alertService;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (Activity is not null && User is not null)
        {
            if (Activity.Name == "")
            {
                await _alertService.DisplayAsync("Name Error", "Name value not inserted");
            }
            else
            {
                try
                {
                    Activity.StartDate = new DateTime(
                        Activity.StartDate.Year,
                        Activity.StartDate.Month,
                        Activity.StartDate.Day,
                        StartTempTime.Hours,
                        StartTempTime.Minutes,
                        StartTempTime.Seconds
                    );
                    Activity.EndDate = new DateTime(
                        Activity.StartDate.Year,
                        Activity.StartDate.Month,
                        Activity.StartDate.Day,
                        EndTempTime.Hours,
                        EndTempTime.Minutes,
                        EndTempTime.Seconds
                    );
                    await _activityFacade.SaveAsync(Activity, User.Id);
                    User.UsrActivities.Add(_activityModelMapper.MapToListModel(Activity));
                    MessengerService.Send(new ActivityEditMessage());
                    _navigationService.SendBackButtonPressed();
                }
                catch (InvalidOperationException)
                {
                    await _alertService.DisplayAsync("Datetime Error ",
                        "Colliding times of activities.");
                }
            }
        }
    }
}

