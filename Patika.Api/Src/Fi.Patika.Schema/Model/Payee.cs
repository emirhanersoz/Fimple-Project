using System;
using System.Collections.Generic;
using Fi.Infra.Schema.Model;
using Fi.Infra.Schema.Attributes;
using Newtonsoft.Json;
using Fi.Infra.Schema.Json;

namespace Fi.Patika.Schema.Model
{
    public record PayeeInputModel : InputModelBase, ITranslationInputModelWithNameMLAndDescriptionML
    {
        [JsonIgnore]
        public int Id { get; set; }
        public List<LanguagePair> NameML { get; set; }
        public List<LanguagePair> DescriptionML { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public int PaymentDay { get; set; }
        public bool isPayment { get; set; }
        [JsonConverter(typeof(FiSmartEnumCodeConverter<PayeeType, byte>))]
        public PayeeType PayeeType { get; set; }
    }

    public record PayeeOutputModel : OutputModelBase, ITranslationOutputModelWithNameAndDescription
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<LanguagePair> NameML { get; set; }
        public string Description { get; set; }
        public List<LanguagePair> DescriptionML { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public int PaymentDay { get; set; }
        public bool isPayment { get; set; }
        [JsonConverter(typeof(FiSmartEnumCodeConverter<PayeeType, byte>))]
        public PayeeType PayeeType { get; set; }
    }
}