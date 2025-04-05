using Grpc.Core;
using Grpc.Net.ClientFactory;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using UserRpcClient;
using static UserRpcClient.UserService;

namespace UserProtobufEFCore.ViewModels
{
    public class MainWindowVM : ReactiveObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly GrpcClientFactory _clientFactory;
        private Stack<string> _notifications = [];
        private ObservableAsPropertyHelper<string> _lastNotification;

        public ReactiveCommand<Unit, Unit> AddUserCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> ShowUsersCommand { get; private set; }
        public string LastNotification => _lastNotification.Value;

        public MainWindowVM(IServiceProvider serviceProvider, GrpcClientFactory clientFactory)
        {
            AddUserCommand = ReactiveCommand.CreateFromTask(OpenAddUserWindow);
            ShowUsersCommand = ReactiveCommand.CreateFromTask(OpenUserListWindow);
            _serviceProvider = serviceProvider;
            _clientFactory = clientFactory;
            SubscribeToUpdates();
        }

        private async Task OpenAddUserWindow()
        {
            await WindowOpener.OpenWindow(_serviceProvider.GetRequiredService<AddUserWindow>());
        }

        private async Task OpenUserListWindow()
        {
            await WindowOpener.OpenWindow(_serviceProvider.GetRequiredService<UserList>());
        }

        private async Task SubscribeToUpdates()
        {
            var client = _clientFactory.CreateClient<UserServiceClient>("httpClient");

            var subscription = client.SubscribeToUserUpdates(new() { UserId = Guid.NewGuid().ToString() })
                .ResponseStream
                .ReadAllAsync()
                .ToObservable()
                .Subscribe(update =>
                {
                    _notifications.Push($"User {update.User.Name} {update.User.Surname} updated: {update.Action}");
                    this.RaisePropertyChanged(nameof(LastNotification));
                });

            _lastNotification = this
                .WhenAnyValue(x => x._notifications.Count)
                .Select(_ => _notifications.TryPeek(out var msg) ? msg : "")
                .ToProperty(this, x => x.LastNotification);
        }
    }
}
