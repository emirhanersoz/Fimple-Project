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
    public class Payee : EntityBaseWithBaseFieldsWithIdentity, ITranslationMasterEntityWithNameAndDescription<PayeeTranslation>
    {
        [Multilingual]
        public string Name { get; set; }
        [Multilingual]
        public string Description { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public int PaymentDay { get; set; }
        public bool isPayment { get; set; }

        public PayeeType PayeeType { get; set; }

        public virtual Account Account { get; set; }

        public virtual List<PayeeTranslation> Translations { get; set; }
    }

    public class PayeeConfigurator : EntityConfigurator<Payee>
    {
        protected override void OnConfigure(EntityTypeBuilder<Payee> builder)
        {
            builder.Property(m => m.Name).IsRequired(true)
                        .HasMaxLength(100);
            builder.Property(m => m.Description).IsRequired(true)
                        .HasMaxLength(255);

            builder.Property(m => m.AccountId).IsRequired(true);
            builder.Property(m => m.Amount).IsRequired(true).HasPrecision(24, 6);
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
                .HasForeignKey(m => m.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Translations).WithOne(y => y.Payee)
                .HasForeignKey(p => p.MasterId).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
        }
    }

    [EntityAttribute(EA.ERT.AsIs, EA.EDT.TransactionalData, EA.ECT.Common, EA.ETT.Common, EA.EMT.Mandatory)]
    public class PayeeTranslation : EntityBase, ITranslationDetailEntityWithNameAndDescription
    {
        public int Id { get; set; } // PK

        public int MasterId { get; set; } // FK
        public virtual Payee Payee { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string LanguageCode { get; set; }

    }

    public class PayeeTranslationConfigurator : EntityConfigurator<PayeeTranslation>
    {
        protected override void OnConfigure(EntityTypeBuilder<PayeeTranslation> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Id).ValueGeneratedOnAdd();

            builder.Property(m => m.MasterId).IsRequired(true);

            builder.Property(m => m.LanguageCode).IsRequired(true);
            builder.Property(m => m.LanguageCode).HasMaxLength(3);

            builder.Property(m => m.Name).IsRequired(true);
            builder.Property(m => m.Name).HasMaxLength(100);
            builder.Property(m => m.Description).IsRequired(true);
            builder.Property(m => m.Description).HasMaxLength(255);

            builder.HasIndex(x => new { x.MasterId, x.LanguageCode }).IsUnique();
        }
    }
}