using System;
using Avalonia;
using Avalonia.ReactiveUI;
using AvaloniaInside.Shell;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Meta;
using UserProtobufEFCore;
using UserProtobufEFCore.ViewModels;
using UserRpcClient;

namespace UserProtobufEFCore
{
    internal sealed class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
        
            var serviceProvider = serviceCollection.BuildServiceProvider();
        
            BuildAvaloniaApp(serviceProvider)
                .StartWithClassicDesktopLifetime(args);
        }

        public static AppBuilder BuildAvaloniaApp(IServiceProvider serviceProvider)
        {
            return AppBuilder.Configure<App>(() => new(serviceProvider))
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI()
                .UseShell();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpcClient<UserService.UserServiceClient>("httpClient", options =>
            options.Address = new("http://localhost:5000"));

            services.AddGrpcClient<UserService.UserServiceClient>("httpsClient", options =>
            options.Address = new("https://localhost:5001"));

            services.AddTransient<AddUserVM>();
            services.AddTransient<UsersVM>();
            services.AddTransient<MainWindowVM>();

            services.AddTransient<MainWindow>();
            services.AddTransient<AddUserWindow>();
            services.AddTransient<UserList>();
        }
    }
}
