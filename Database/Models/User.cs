using ProtoBuf;
using System;

namespace UserProtobufEFCore.Database.Models
{
    [ProtoContract]
    internal class User
    {
        [ProtoMember(1)]
        public Guid Id { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; } = string.Empty;

        [ProtoMember(3)]
        public string Surname { get; set; } = string.Empty;

        [ProtoMember(4)]
        public string Email { get; set; } = string.Empty;

        [ProtoMember(5)]
        public string Phone { get; set; } = string.Empty;

        [ProtoMember(6)]
        public required byte[] Photo { get; set; }

    }
}
