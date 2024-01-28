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
    public class MoneyTransfer : EntityBaseWithBaseFieldsWithIdentity
    {
        public int AccountId { get; set; }
        public int DestAccountId { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }

        public virtual Account Account { get; set; }
    }

    public class MoneyTransferConfigurator : EntityConfigurator<MoneyTransfer>
    {
        protected override void OnConfigure(EntityTypeBuilder<MoneyTransfer> builder)
        {
            builder.Property(m => m.DestAccountId).IsRequired(true);
            builder.Property(m => m.Amount).IsRequired(true).HasPrecision(24, 6);
            builder.Property(m => m.Comment).IsRequired(true)
                        .HasMaxLength(200);

            builder.HasIndex(m => m.AccountId);
            builder.HasOne(m => m.Account)
                .WithMany(m => m.MoneyTransfers)
                .HasForeignKey(m => m.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}