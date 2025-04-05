using Avalonia.Logging;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.ClientFactory;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive;
using System.Threading.Tasks;
using UserProtobufEFCore.Models;
using UserRpcClient;


namespace UserProtobufEFCore.ViewModels
{
    public class UsersVM : ReactiveObject
    {
        private readonly ObservableCollection<UserModel> _users = new();
        private readonly IServiceProvider _serviceProvider;
        private readonly GrpcClientFactory _clientFactory;
        private bool _isLoading;

        public ReactiveCommand<Unit, Unit> LoadUsersCommand { get; }
        public ObservableCollection<UserModel> Users => _users;
        public bool IsLoading
        {
            get => _isLoading;
            set => this.RaiseAndSetIfChanged(ref _isLoading, value);
        }

        public UsersVM(IServiceProvider serviceProvider, GrpcClientFactory clientFactory)
        {
            LoadUsersCommand = ReactiveCommand.CreateFromTask(LoadUsersAsync);
            _serviceProvider = serviceProvider;
            _clientFactory = clientFactory;
        }

        private async Task LoadUsersAsync()
        {
            try
            {
                IsLoading = true;

                var client = _clientFactory.CreateClient<UserService.UserServiceClient>("httpClient");
                var response = await client.GetUsersAsync(new());

                _users.Clear();
                foreach (var user in response.Users)
                {
                    _users.Add(new UserModel
                    {
                        Id = new Guid(user.Id),
                        Name = user.Name,
                        Surname = user.Surname,
                        Email = user.Email,
                        Phone = user.Phone,
                        Photo = UserModel.ConvertBytesToImage(user.Photo.ToByteArray())
                    });
                }
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.InvalidArgument)
            {
                var exceptionType = ex.Trailers.GetValue("exception-type");
                var stackTrace = ex.Trailers.GetValue("stack-trace");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}