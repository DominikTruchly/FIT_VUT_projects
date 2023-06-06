using ICSProject.App.ViewModels;

namespace ICSProject.App.Views.User;

public partial class UserListView
{
	public UserListView(UserListViewModel viewModel) : base(viewModel)
	{
		InitializeComponent();
	}
}
