using System;
using System.Collections.Generic;
using Fi.Infra.Schema.Model;
using Fi.Infra.Schema.Attributes;
using Newtonsoft.Json;
using Fi.Infra.Schema.Json;

namespace Fi.Patika.Schema.Model
{
    public record DepositAndWithdrawInputModel : InputModelBase
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public bool isSucceded { get; set; }
        [JsonConverter(typeof(FiSmartEnumCodeConverter<TransactionType, byte>))]
        public TransactionType TransactionType { get; set; }
    }

    public record DepositAndWithdrawOutputModel : OutputModelBase
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public bool isSucceded { get; set; }
        [JsonConverter(typeof(FiSmartEnumCodeConverter<TransactionType, byte>))]
        public TransactionType TransactionType { get; set; }
    }
}