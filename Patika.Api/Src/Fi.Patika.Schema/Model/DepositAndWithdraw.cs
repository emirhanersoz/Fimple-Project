using System;
using System.Collections.Generic;
using Fi.Infra.Schema.Model;
using Fi.Infra.Schema.Attributes;
using Newtonsoft.Json;
using Fi.Infra.Schema.Json;

namespace Fi.Patika.Schema.Model
{
    public record DepositAndWithdrawInputModel : InputModelBase, ITranslationInputModelWithNameMLAndDescriptionML
    {
        [JsonIgnore]
        public int Id { get; set; }
        public List<LanguagePair> NameML { get; set; }
        public List<LanguagePair> DescriptionML { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public bool isSucceded { get; set; }
        [JsonConverter(typeof(FiSmartEnumCodeConverter<TransactionType, byte>))]
        public TransactionType TransactionType { get; set; }
    }

    public record DepositAndWithdrawOutputModel : OutputModelBase, ITranslationOutputModelWithNameAndDescription
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<LanguagePair> NameML { get; set; }
        public string Description { get; set; }
        public List<LanguagePair> DescriptionML { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public bool isSucceded { get; set; }
        [JsonConverter(typeof(FiSmartEnumCodeConverter<TransactionType, byte>))]
        public TransactionType TransactionType { get; set; }
    }
}