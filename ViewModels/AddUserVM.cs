using ReactiveValidation;
using ReactiveValidation.Extensions;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using Avalonia.Media.Imaging;
using System.IO;
using ReactiveUI;
using System.Linq;
using System.Reactive;
using Grpc.Net.Client;
using UserRpcClient;
using System;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using Grpc.Net.ClientFactory;

namespace UserProtobufEFCore.ViewModels
{
    public class AddUserVM : ValidatableObject
    {
        private string _name;
        private string _surname;
        private string _phone;
        private string _email;
        private byte[] _photo;
        private string _imagePath;
        private readonly Window _window;
        private readonly IServiceProvider _serviceProvider;
        private readonly IStorageProvider _storageProvider;
        private readonly GrpcClientFactory _grpcClientFactory;
        private readonly UserService.UserServiceClient _client;

        public ReactiveCommand<Unit, Unit> AddPhotoCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveToDbCommand { get; }

        public AddUserVM(Window window,IServiceProvider serviceProvider, IStorageProvider storageProvider, GrpcClientFactory grpcClientFactory)
        {
            Validator = GetValidator();
            _serviceProvider = serviceProvider;
            _storageProvider = storageProvider;
            _grpcClientFactory = grpcClientFactory;
            _window = window;
            AddPhotoCommand = ReactiveCommand.CreateFromTask(SelectPhoto);
            SaveToDbCommand = ReactiveCommand.CreateFromTask(SaveToDb);
        }

        private IObjectValidator GetValidator()
        {
            var builder = new ValidationBuilder<AddUserVM>();

            builder.RuleFor(u => u.Name)
                .NotEmpty()
                .MaxLength(100)
                .Matches(@"\p{IsCyrillic}")
                    .WithMessage("Поле не должно содержать более 100 символов и состоять из символов кириллицы");

            builder.RuleFor(u => u.Surname)
                .NotEmpty()
                .MaxLength(100)
                .Matches(@"\p{IsCyrillic}")
                    .WithMessage("Поле не должно содержать более 100 символов и состоять из символов кириллицы");

            builder.RuleFor(u => u.Phone)
                .NotEmpty()
                    .When(u => u.Email, email => string.IsNullOrEmpty(email))
                    .WithMessage("Заполните Email и телефон")
                .Matches(@"^\d{11}$")
                    .WithMessage("Номер должен содержать 11 цифр");


            builder.RuleFor(u => u.Email)
                .NotEmpty()
                    .When(u => u.Phone, phone => string.IsNullOrEmpty(phone))
                    .WithMessage("Заполните Email и телефон")
                .Matches("@")
                    .WithMessage("Несуществующий Email");

            return builder.Build(this);
        }
        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged(nameof(ImagePath));
            }
        }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }
        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        public byte[] Photo
        {
            get => _photo;
            set
            {
                _photo = value;
                OnPropertyChanged(nameof(Photo));
            }
        }

        private async Task SelectPhoto()
        {
            var files = await _storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Выберите фотографию",
                FileTypeFilter =
                [
                    new FilePickerFileType("Изображения")
                    {
                        Patterns = ["*.jpg", "*.jpeg", "*.png", "*.bmp", "*.gif"]
                    }
                ],
                AllowMultiple = false
            });

            if (files.Count > 0)
            {
                var file = files[0];
                ImagePath = file.Path.AbsolutePath;
                await ConvertImageToByte(file);
            }
        }

        private async Task ConvertImageToByte(IStorageFile file)
        {
            await using var stream = await file.OpenReadAsync();
            using MemoryStream memoryStream = new();
            await stream.CopyToAsync(memoryStream);
            Photo = memoryStream.ToArray();
        }

        private async Task SaveToDb()
        {
            
            var client = _grpcClientFactory.CreateClient<UserService.UserServiceClient>("httpClient");

            AddUserRequest request = new()
            {
                User = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = Name,
                    Surname = Surname,
                    Email = Email,
                    Phone = Phone,
                    Photo = Google.Protobuf.ByteString.CopyFrom(Photo)
                }
            };

            var response = await client.AddUserAsync(request);
            _window.Close();
        }
    }
}
