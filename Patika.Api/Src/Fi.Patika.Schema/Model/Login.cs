using System;
using System.Collections.Generic;
using Fi.Infra.Schema.Model;
using Fi.Infra.Schema.Attributes;
using Newtonsoft.Json;
using Fi.Infra.Schema.Json;

namespace Fi.Patika.Schema.Model
{
    public record LoginInputModel : InputModelBase, ITranslationInputModelWithNameMLAndDescriptionML
    {
        [JsonIgnore]
        public int Id { get; set; }
        public List<LanguagePair> NameML { get; set; }
        public List<LanguagePair> DescriptionML { get; set; }
        public int UserId { get; set; }
        public DateTime LoginTime { get; set; }
    }

    public record LoginOutputModel : OutputModelBase, ITranslationOutputModelWithNameAndDescription
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<LanguagePair> NameML { get; set; }
        public string Description { get; set; }
        public List<LanguagePair> DescriptionML { get; set; }
        public int UserId { get; set; }
        public DateTime LoginTime { get; set; }
    }
}