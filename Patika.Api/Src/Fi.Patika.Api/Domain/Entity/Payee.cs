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
    public class Payee : EntityBaseWithBaseFieldsWithIdentity
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public int PaymentDay { get; set; }
        public bool isPayment { get; set; }

        public PayeeType PayeeType { get; set; }

        public virtual Account Account { get; set; }
    }

    public class PayeeConfigurator : EntityConfigurator<Payee>
    {
        protected override void OnConfigure(EntityTypeBuilder<Payee> builder)
        {
            builder.Property(m => m.AccountId).IsRequired(true);
            builder.Property(m => m.Amount).IsRequired(true);
            builder.Property(m => m.PaymentDay).IsRequired(true);
            builder.Property(m => m.isPayment).IsRequired(true);

            builder.Property(m => m.PayeeType).IsRequired(true)
           .HasConversion(
               p => p.Value,
               p => PayeeType.FromValue(p)
           );

            builder.HasIndex(m => m.AccountId);
            builder.HasOne(m => m.Account)
                .WithMany(m => m.Payees)
                .HasForeignKey(m => m.AccountId);
        }
    }
}