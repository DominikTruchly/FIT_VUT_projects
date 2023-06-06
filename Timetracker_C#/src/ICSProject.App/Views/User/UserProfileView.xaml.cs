using ICSProject.App.ViewModels;

namespace ICSProject.App.Views.User;

public partial class UserProfileView
{
    public UserProfileView(UserProfileViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}
