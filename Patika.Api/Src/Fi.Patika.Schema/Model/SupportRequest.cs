using System;
using System.Collections.Generic;
using Fi.Infra.Schema.Model;
using Fi.Infra.Schema.Attributes;
using Newtonsoft.Json;
using Fi.Infra.Schema.Json;

namespace Fi.Patika.Schema.Model
{
    public record SupportRequestInputModel : InputModelBase
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public bool isAnswered { get; set; }
        public string? Answered { get; set; }
        public DateTime? AnsweredDate { get; set; }
    }

    public record SupportRequestOutputModel : OutputModelBase
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public bool isAnswered { get; set; }
        public string? Answered { get; set; }
        public DateTime? AnsweredDate { get; set; }
    }
}