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
    public class SupportRequest : EntityBaseWithBaseFieldsWithIdentity, ITranslationMasterEntityWithNameAndDescription<SupportRequestTranslation>
    {
        [Multilingual]
        public string Name { get; set; }
        [Multilingual]
        public string Description { get; set; }
        public int CustomerId { get; set; }
        public string Subject { get; set; }
        public string Comment { get; set; }
        public bool isAnswered { get; set; }
        public string? Answered { get; set; }
        public DateTime? AnsweredDate { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual List<SupportRequestTranslation> Translations { get; set; }
    }

    public class SupportRequestConfigurator : EntityConfigurator<SupportRequest>
    {
        protected override void OnConfigure(EntityTypeBuilder<SupportRequest> builder)
        {
            builder.Property(m => m.Name).IsRequired(true)
                        .HasMaxLength(100);
            builder.Property(m => m.Description).IsRequired(true)
                        .HasMaxLength(255);

            builder.Property(m => m.CustomerId).IsRequired(true);
            builder.Property(m => m.Subject).IsRequired(true)
                        .HasMaxLength(100);
            builder.Property(m => m.Comment).IsRequired(true)
                        .HasMaxLength(100);
            builder.Property(m => m.isAnswered).IsRequired(true);
            builder.Property(m => m.Answered).IsRequired(false)
                        .HasMaxLength(100);
            builder.Property(m => m.AnsweredDate).IsRequired(false);

            builder.HasIndex(m => m.CustomerId);
            builder.HasOne(m => m.Customer)
                .WithMany(m => m.SupportRequests)
                .HasForeignKey(m => m.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Translations).WithOne(y => y.SupportRequest)
        .HasForeignKey(p => p.MasterId).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
        }
    }

    [EntityAttribute(EA.ERT.AsIs, EA.EDT.TransactionalData, EA.ECT.Common, EA.ETT.Common, EA.EMT.Mandatory)]
    public class SupportRequestTranslation : EntityBase, ITranslationDetailEntityWithNameAndDescription
    {
        public int Id { get; set; } // PK

        public int MasterId { get; set; } // FK
        public virtual SupportRequest SupportRequest { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string LanguageCode { get; set; }

    }

    public class SupportRequestTranslationConfigurator : EntityConfigurator<SupportRequestTranslation>
    {
        protected override void OnConfigure(EntityTypeBuilder<SupportRequestTranslation> builder)
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