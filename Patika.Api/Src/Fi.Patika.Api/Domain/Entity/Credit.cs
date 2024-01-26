using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Fi.Infra.Schema.Json;
using Fi.Patika.Schema.Model;
using Fi.Infra.Persistence;
using Fi.Infra.Schema.Attributes;
using Fi.Persistence.Relational.Domain;

namespace Fi.Patika.Api.Domain.Entity
{
    // Decide the need of the Audit Log Enable attribute
    //[AuditLogEnabled]
    // Decide each parameter in EntityAttribute
    [EntityAttribute(EA.ERT.AsIs, EA.EDT.TransactionalData, EA.ECT.Common, EA.ETT.Common, EA.EMT.Mandatory)]
    public class Credit : EntityBaseWithBaseFieldsWithIdentity
    {
        public decimal TotalAmount { get; set; }
        public decimal MontlyPayment { get; set; }
        public int RepaymentPeriodMonths { get; set; }
        public DateTime LoanDate { get; set; }

        public virtual List<AccountCredit> AccountCredits { get; set; }
    }

    public class CreditConfigurator : EntityConfigurator<Credit>
    {
        protected override void OnConfigure(EntityTypeBuilder<Credit> builder)
        {
            builder.Property(m => m.TotalAmount).IsRequired(true);
            builder.Property(m => m.MontlyPayment).IsRequired(true);
            builder.Property(m => m.RepaymentPeriodMonths).IsRequired(true);
            builder.Property(m => m.LoanDate).IsRequired(true);
        }
    }
}