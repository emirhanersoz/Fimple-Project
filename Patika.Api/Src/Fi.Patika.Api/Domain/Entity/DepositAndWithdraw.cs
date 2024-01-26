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
    public class DepositAndWithdraw : EntityBaseWithBaseFieldsWithIdentity
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public bool isSucceded { get; set; }

        public virtual Account Account { get; set; }
    }

    public class DepositAndWithdrawConfigurator : EntityConfigurator<DepositAndWithdraw>
    {
        protected override void OnConfigure(EntityTypeBuilder<DepositAndWithdraw> builder)
        {
            builder.Property(m => m.AccountId).IsRequired(true);
            builder.Property(m => m.Amount).IsRequired(true);
            builder.Property(m => m.isSucceded).IsRequired(true);

            builder.HasIndex(m => m.AccountId);
            builder.HasOne(m => m.Account)
                .WithMany(m => m.DepositAndWithdraws)
                .HasForeignKey(m => m.AccountId);
        }
    }
}