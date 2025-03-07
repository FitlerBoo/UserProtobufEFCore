using Avalonia.Controls;
using Microsoft.AspNetCore.Components.Routing;
using Tmds.DBus.Protocol;
using UserProtobufEFCore;

namespace UsedProtobufEFCore.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {

        }

        private void AddUser_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var addUser = new AddUserView();
            var dialog = new Window
            {
                Title = "¬вод данных",
                Content = addUser,
                Width = 500,
                Height = 720,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            dialog.ShowDialog(this);
        }
    }
}