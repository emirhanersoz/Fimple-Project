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
    public class User : EntityBaseWithBaseFieldsWithIdentity, ITranslationMasterEntityWithNameAndDescription<UserTranslation>
    {
        [Multilingual]
        public string Name { get; set; }
        [Multilingual]
        public string Description { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }

        public RoleType RoleType { get; set; }

        public virtual List<Customer> Customers { get; set; }
        public virtual List<Login> Logins { get; set; }

        public virtual List<UserTranslation> Translations { get; set; }
}

    public class UserConfigurator : EntityConfigurator<User>
    {
        protected override void OnConfigure(EntityTypeBuilder<User> builder)
        {
            builder.Property(m => m.Name).IsRequired(true)
                        .HasMaxLength(100);
            builder.Property(m => m.Description).IsRequired(true)
                        .HasMaxLength(255);

            builder.Property(m => m.PasswordHash).IsRequired(true);
            builder.Property(m => m.PasswordSalt).IsRequired(true);
            builder.Property(m => m.RefreshToken).IsRequired(true)
                        .HasMaxLength(100);
            builder.Property(m => m.TokenCreated).IsRequired(true);
            builder.Property(m => m.TokenExpires).IsRequired(true);

            builder.Property(m => m.RoleType).IsRequired(true)
           .HasConversion(
               p => p.Value,
               p => RoleType.FromValue(p)
           );

            builder.HasMany(p => p.Translations).WithOne(y => y.User)
                   .HasForeignKey(p => p.MasterId).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
        }
    }
    
    [EntityAttribute(EA.ERT.AsIs, EA.EDT.TransactionalData, EA.ECT.Common, EA.ETT.Common, EA.EMT.Mandatory)]
    public class UserTranslation : EntityBase, ITranslationDetailEntityWithNameAndDescription
    {
        public int Id { get; set; } // PK

        public int MasterId { get; set; } // FK
        public virtual User User { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string LanguageCode { get; set; }

    }

    public class UserTranslationConfigurator : EntityConfigurator<UserTranslation>
    {
        protected override void OnConfigure(EntityTypeBuilder<UserTranslation> builder)
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