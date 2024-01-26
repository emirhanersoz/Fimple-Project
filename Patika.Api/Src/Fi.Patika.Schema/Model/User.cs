using System;
using System.Collections.Generic;
using Fi.Infra.Schema.Model;
using Fi.Infra.Schema.Attributes;
using Newtonsoft.Json;
using Fi.Infra.Schema.Json;

namespace Fi.Patika.Schema.Model
{
    public record UserInputModel : InputModelBase
    {
        [JsonIgnore]
        public int Id { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
        [JsonConverter(typeof(FiSmartEnumCodeConverter<RoleType, byte>))]
        public RoleType RoleType { get; set; }
    }

    public record UserOutputModel : OutputModelBase
    {
        public int Id { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
        [JsonConverter(typeof(FiSmartEnumCodeConverter<RoleType, byte>))]
        public RoleType RoleType { get; set; }
    }
}