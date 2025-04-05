using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Grpc.Net.Client;
using Grpc.Net.ClientFactory;
using System;
using System.IO;
using System.Threading.Tasks;
using UserProtobufEFCore.ViewModels;
using UserRpcClient;

namespace UserProtobufEFCore;

public partial class AddUserWindow : Window
{
    public AddUserWindow(IServiceProvider serviceProvider, GrpcClientFactory cf)
    {
        InitializeComponent();
        DataContext = new AddUserVM(this, serviceProvider, StorageProvider, cf);
    }
}