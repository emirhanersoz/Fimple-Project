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
    public class User : EntityBaseWithBaseFieldsWithIdentity
    {
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }

        public RoleType RoleType { get; set; }

        public virtual List<Customer> Customers { get; set; }
        public virtual List<Login> Logins { get; set; }
    }

    public class UserConfigurator : EntityConfigurator<User>
    {
        protected override void OnConfigure(EntityTypeBuilder<User> builder)
        {
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
        }
    }
}