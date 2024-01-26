using System;
using System.Collections.Generic;
using Fi.Infra.Schema.Model;
using Fi.Infra.Schema.Attributes;
using Newtonsoft.Json;
using Fi.Infra.Schema.Json;

namespace Fi.Patika.Schema.Model
{
    public record CreditInputModel : InputModelBase
    {
        [JsonIgnore]
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal MontlyPayment { get; set; }
        public int RepaymentPeriodMonths { get; set; }
        public DateTime LoanDate { get; set; }
    }

    public record CreditOutputModel : OutputModelBase
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal MontlyPayment { get; set; }
        public int RepaymentPeriodMonths { get; set; }
        public DateTime LoanDate { get; set; }
    }
}