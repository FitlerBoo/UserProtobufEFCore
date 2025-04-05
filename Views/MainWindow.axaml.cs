using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Windows.Input;
using UserProtobufEFCore.ViewModels;

namespace UserProtobufEFCore;

public partial class MainWindow : Window
{

    public MainWindow(MainWindowVM mainWindowVM)
    {
        InitializeComponent();
        DataContext = mainWindowVM;
    }
}