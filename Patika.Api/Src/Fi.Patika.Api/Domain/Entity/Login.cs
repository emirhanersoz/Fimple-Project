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
    public class Login : EntityBaseWithBaseFieldsWithIdentity, ITranslationMasterEntityWithNameAndDescription<LoginTranslation>
    {
        [Multilingual]
        public string Name { get; set; }
        [Multilingual]
        public string Description { get; set; }
        public int UserId { get; set; }
        public DateTime LoginTime { get; set; }

        public virtual User User { get; set; }

        public virtual List<LoginTranslation> Translations { get; set; }
    }

    public class LoginConfigurator : EntityConfigurator<Login>
    {
        protected override void OnConfigure(EntityTypeBuilder<Login> builder)
        {
            builder.Property(m => m.Name).IsRequired(true)
                        .HasMaxLength(100);
            builder.Property(m => m.Description).IsRequired(true)
                        .HasMaxLength(255);
            builder.Property(m => m.UserId).IsRequired(true);
            builder.Property(m => m.LoginTime).IsRequired(true);

            builder.HasIndex(m => m.UserId);
            builder.HasOne(m => m.User)
                .WithMany(m => m.Logins)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Translations).WithOne(y => y.Login)
                    .HasForeignKey(p => p.MasterId).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
        }
    }

    [EntityAttribute(EA.ERT.AsIs, EA.EDT.TransactionalData, EA.ECT.Common, EA.ETT.Common, EA.EMT.Mandatory)]
    public class LoginTranslation : EntityBase, ITranslationDetailEntityWithNameAndDescription
    {
        public int Id { get; set; } // PK

        public int MasterId { get; set; } // FK
        public virtual Login Login { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string LanguageCode { get; set; }

    }

    public class LoginTranslationConfigurator : EntityConfigurator<LoginTranslation>
    {
        protected override void OnConfigure(EntityTypeBuilder<LoginTranslation> builder)
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