using System;
using System.Collections.Generic;
using Fi.Infra.Schema.Model;
using Fi.Infra.Schema.Attributes;
using Newtonsoft.Json;
using Fi.Infra.Schema.Json;

namespace Fi.Patika.Schema.Model
{
    public record AccountCreditInputModel : InputModelBase
    {
        public int AccountId { get; set; }
        public int CreditId { get; set; }
    }

    public record AccountCreditOutputModel : OutputModelBase
    {
        public int AccountId { get; set; }
        public int CreditId { get; set; }
    }
}