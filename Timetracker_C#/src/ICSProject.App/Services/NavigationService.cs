using ICSProject.App.Models;
using ICSProject.App.ViewModels;
using ICSProject.App.Views.Activity;
using ICSProject.App.Views.User;
using ICSProject.App.Views.Project;

namespace ICSProject.App.Services;

public class NavigationService : INavigationService
{
    public IEnumerable<RouteModel> Routes { get; } = new List<RouteModel>
    {
        new("//users", typeof(UserListView), typeof(UserListViewModel)),

        new("//users/userAdd", typeof(UserAddView), typeof(UserAddViewModel)),
        new("//users/detail", typeof(UserDetailView), typeof(UserDetailViewModel)),

        new("//users/detail/profile", typeof(UserProfileView), typeof(UserProfileViewModel)),
        new("//users/detail/activities", typeof(ActivityDetailView), typeof(ActivityDetailViewModel)),
        new("//users/detail/activityAdd", typeof(ActivityAddView), typeof(ActivityAddViewModel)),
        new("//users/detail/projectAdd", typeof(ProjectAddView), typeof(ProjectAddViewModel)),
        new("//users/detail/projectDetail", typeof(ProjectDetailView), typeof(ProjectDetailViewModel)),
        new("//users/detail/activityDetail", typeof(ActivityDetailView), typeof(ActivityDetailViewModel)),
    };

    public async Task GoToAsync<TViewModel>()
        where TViewModel : IViewModel
    {
        var route = GetRouteByViewModel<TViewModel>();
        await Shell.Current.GoToAsync(route);
    }
    public async Task GoToAsync<TViewModel>(IDictionary<string, object?> parameters)
        where TViewModel : IViewModel
    {
        var route = GetRouteByViewModel<TViewModel>();
        await Shell.Current.GoToAsync(route, parameters);
    }

    public async Task GoToAsync(string route)
        => await Shell.Current.GoToAsync(route);

    public async Task GoToAsync(string route, IDictionary<string, object?> parameters)
        => await Shell.Current.GoToAsync(route, parameters);

    public bool SendBackButtonPressed()
        => Shell.Current.SendBackButtonPressed();

    private string GetRouteByViewModel<TViewModel>()
        where TViewModel : IViewModel
        => Routes.First(route => route.ViewModelType == typeof(TViewModel)).Route;
}
