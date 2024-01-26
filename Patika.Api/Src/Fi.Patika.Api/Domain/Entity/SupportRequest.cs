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
    public class SupportRequest : EntityBaseWithBaseFieldsWithIdentity
    {
        public int CustomerId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public bool isAnswered { get; set; }
        public string? Answered { get; set; }
        public DateTime? AnsweredDate { get; set; }

        public virtual Customer Customer { get; set; } 
    }

    public class SupportRequestConfigurator : EntityConfigurator<SupportRequest>
    {
        protected override void OnConfigure(EntityTypeBuilder<SupportRequest> builder)
        {
            builder.Property(m => m.CustomerId).IsRequired(true);
            builder.Property(m => m.Subject).IsRequired(true)
                        .HasMaxLength(100);
            builder.Property(m => m.Description).IsRequired(true)
                        .HasMaxLength(100);
            builder.Property(m => m.isAnswered).IsRequired(true);
            builder.Property(m => m.Answered).IsRequired(false)
                        .HasMaxLength(100);
            builder.Property(m => m.AnsweredDate).IsRequired(false);

            builder.HasIndex(m => m.CustomerId);
            builder.HasOne(m => m.Customer)
                .WithMany(m => m.SupportRequests)
                .HasForeignKey(m => m.CustomerId);
        }
    }
}