using System;
using System.Collections.Generic;
using Fi.Infra.Schema.Model;
using Fi.Infra.Schema.Attributes;
using Newtonsoft.Json;
using Fi.Infra.Schema.Json;

namespace Fi.Patika.Schema.Model
{
    public record MoneyTransferInputModel : InputModelBase
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int DestAccountId { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
    }

    public record MoneyTransferOutputModel : OutputModelBase
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int DestAccountId { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
    }
}