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
    public class DepositAndWithdraw : EntityBaseWithBaseFieldsWithIdentity, ITranslationMasterEntityWithNameAndDescription<DepositAndWithdrawTranslation>
    {
        [Multilingual]
        public string Name { get; set; }
        [Multilingual]
        public string Description { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public bool isSucceded { get; set; }

        public virtual Account Account { get; set; }

        public virtual List<DepositAndWithdrawTranslation> Translations { get; set; }
    }

    public class DepositAndWithdrawConfigurator : EntityConfigurator<DepositAndWithdraw>
    {
        protected override void OnConfigure(EntityTypeBuilder<DepositAndWithdraw> builder)
        {
            builder.Property(m => m.Name).IsRequired(true)
                        .HasMaxLength(100);
            builder.Property(m => m.Description).IsRequired(true)
                        .HasMaxLength(255);

            builder.Property(m => m.AccountId).IsRequired(true);
            builder.Property(m => m.Amount).IsRequired(true).HasPrecision(24, 6);
            builder.Property(m => m.isSucceded).IsRequired(true);

            builder.HasIndex(m => m.AccountId);
            builder.HasOne(m => m.Account)
                .WithMany(m => m.DepositAndWithdraws)
                .HasForeignKey(m => m.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Translations).WithOne(y => y.DepositAndWithdraw)
                    .HasForeignKey(p => p.MasterId).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
        }
    }

    [EntityAttribute(EA.ERT.AsIs, EA.EDT.TransactionalData, EA.ECT.Common, EA.ETT.Common, EA.EMT.Mandatory)]
    public class DepositAndWithdrawTranslation : EntityBase, ITranslationDetailEntityWithNameAndDescription
    {
        public int Id { get; set; } // PK

        public int MasterId { get; set; } // FK
        public virtual DepositAndWithdraw DepositAndWithdraw { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string LanguageCode { get; set; }

    }

    public class DepositAndWithdrawTranslationConfigurator : EntityConfigurator<DepositAndWithdrawTranslation>
    {
        protected override void OnConfigure(EntityTypeBuilder<DepositAndWithdrawTranslation> builder)
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