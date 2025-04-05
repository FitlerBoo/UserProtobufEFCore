using Avalonia.Media.Imaging;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UserProtobufEFCore.Models
{
    public class UserModel : ReactiveObject
    {
        private Guid _id;
        private string? _name;
        private string? _surname;
        private string? _email;
        private string? _phone;
        public Bitmap? _photo;

        public Guid Id
        {
            get => _id;
            set => this.RaiseAndSetIfChanged(ref _id, value);
        }

        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        public string Surname
        {
            get => _surname;
            set => this.RaiseAndSetIfChanged(ref _surname, value);
        }

        public string Email
        {
            get => _email;
            set => this.RaiseAndSetIfChanged(ref _email, value);
        }

        public string Phone
        {
            get => _phone;
            set => this.RaiseAndSetIfChanged(ref _phone, value);
        }

        public Bitmap? Photo
        {
            get => _photo;
            set => this.RaiseAndSetIfChanged(ref _photo, value);
        }

        public static Bitmap? ConvertBytesToImage(byte[] photoInBytes)
        {
            if (photoInBytes == null || photoInBytes.Length == 0)
                return null;
            using var memoryStream = new MemoryStream(photoInBytes);
            return new Bitmap(memoryStream);
        }
    }
}
