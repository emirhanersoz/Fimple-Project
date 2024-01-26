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
    public class Login : EntityBaseWithBaseFieldsWithIdentity
    {
        public int UserId { get; set; }
        public DateTime LoginTime { get; set; }

        public virtual User User { get; set; }
    }

    public class LoginConfigurator : EntityConfigurator<Login>
    {
        protected override void OnConfigure(EntityTypeBuilder<Login> builder)
        {
            builder.Property(m => m.UserId).IsRequired(true);
            builder.Property(m => m.LoginTime).IsRequired(true);

            builder.HasIndex(m => m.UserId);
            builder.HasOne(m => m.User)
                .WithMany(m => m.Logins)
                .HasForeignKey(m => m.UserId);
        }
    }
}