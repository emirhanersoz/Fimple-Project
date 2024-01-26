using System;
using Fi.Infra.Schema.Json;
using Fi.Infra.Schema.Model;
using Fi.Infra.Schema.Const;
using Newtonsoft.Json;

namespace Fi.Template.Schema.Model
{
    public record SampleInputModel : InputModelBase
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonConverter(typeof(FiSmartEnumCodeConverter<SampleType, byte>))]
        public SampleType SampleType { get; set; }
    }

    public record SampleOutputModel : OutputModelBase
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonConverter(typeof(FiSmartEnumCodeConverter<SampleType, byte>))]
        public SampleType SampleType { get; set; }
    }
}