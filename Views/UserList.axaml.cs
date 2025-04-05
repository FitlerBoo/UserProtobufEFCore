using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using UserProtobufEFCore.ViewModels;

namespace UserProtobufEFCore;

public partial class UserList : Window
{
    public UserList(UsersVM usersVM)
    {
        InitializeComponent();
        DataContext = usersVM;
    }
}