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
    public class AccountCredit : EntityBaseWithBaseFieldsWithIdentity
    {
        public int AccountId { get; set; }
        public int CreditId { get; set; }

        public virtual Account Account { get; set; }
        public virtual Credit Credit { get; set; }
    }

    public class AccountCreditConfigurator : EntityConfigurator<AccountCredit>
    {
        protected override void OnConfigure(EntityTypeBuilder<AccountCredit> builder)
        {
            builder.Property(m => m.AccountId).IsRequired(true);
            builder.Property(m => m.CreditId).IsRequired(true);

            builder.HasIndex(m => new {m.AccountId, m.CreditId});

            builder.HasOne(m => m.Account)
                .WithMany(m => m.AccountCredits)
                .HasForeignKey(m => m.AccountId);

            builder.HasIndex(m => m.CreditId);
            builder.HasOne(m => m.Credit)
                .WithMany(m => m.AccountCredits)
                .HasForeignKey(m => m.CreditId);
        }
    }
}