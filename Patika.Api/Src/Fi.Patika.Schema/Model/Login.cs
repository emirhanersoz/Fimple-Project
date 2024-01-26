using System;
using System.Collections.Generic;
using Fi.Infra.Schema.Model;
using Fi.Infra.Schema.Attributes;
using Newtonsoft.Json;
using Fi.Infra.Schema.Json;

namespace Fi.Patika.Schema.Model
{
    public record LoginInputModel : InputModelBase
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime LoginTime { get; set; }
    }

    public record LoginOutputModel : OutputModelBase
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime LoginTime { get; set; }
    }
}