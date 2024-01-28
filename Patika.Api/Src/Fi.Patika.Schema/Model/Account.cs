using System;
using System.Collections.Generic;
using Fi.Infra.Schema.Model;
using Fi.Infra.Schema.Attributes;
using Newtonsoft.Json;
using Fi.Infra.Schema.Json;

namespace Fi.Patika.Schema.Model
{
    public record AccountInputModel : InputModelBase, ITranslationInputModelWithNameMLAndDescriptionML
    {
        [JsonIgnore]
        public int Id { get; set; }
        public List<LanguagePair> NameML { get; set; }
        public List<LanguagePair> DescriptionML { get; set; }
        public int CustomerId { get; set; }
        public decimal Balance { get; set; }
        public decimal Salary { get; set; }
        public decimal BankScore { get; set; }
        public decimal TotailDailyTransferAmount { get; set; }
        [JsonConverter(typeof(FiSmartEnumCodeConverter<AccountType, byte>))]
        public AccountType AccountType { get; set; }
    }

    public record AccountOutputModel : OutputModelBase, ITranslationOutputModelWithNameAndDescription
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<LanguagePair> NameML { get; set; }
        public string Description { get; set; }
        public List<LanguagePair> DescriptionML { get; set; }
        public int CustomerId { get; set; }
        public decimal Balance { get; set; }
        public decimal Salary { get; set; }
        public decimal BankScore { get; set; }
        public decimal TotailDailyTransferAmount { get; set; }
        [JsonConverter(typeof(FiSmartEnumCodeConverter<AccountType, byte>))]
        public AccountType AccountType { get; set; }
    }
}