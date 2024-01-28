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
    public class Account : EntityBaseWithBaseFieldsWithIdentity, ITranslationMasterEntityWithNameAndDescription<AccountTranslation>
    {
        [Multilingual]
        public string Name { get; set; }
        [Multilingual]
        public string Description { get; set; }
        public int CustomerId { get; set; }
        public decimal Balance { get; set; }
        public decimal Salary { get; set; }
        public decimal BankScore { get; set; }
        public decimal TotailDailyTransferAmount { get; set; }

        public AccountType AccountType { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual List<AccountCredit> AccountCredits { get; set; }
        public virtual List<DepositAndWithdraw> DepositAndWithdraws { get; set; }
        public virtual List<MoneyTransfer> MoneyTransfers { get; set; }
        public virtual List<Payee> Payees { get; set; }

        public virtual List<AccountTranslation> Translations { get; set; }
    }

    public class AccountConfigurator : EntityConfigurator<Account>
    {
        protected override void OnConfigure(EntityTypeBuilder<Account> builder)
        {
            builder.Property(m => m.Name).IsRequired(true)
                        .HasMaxLength(100);
            builder.Property(m => m.Description).IsRequired(true)
                        .HasMaxLength(255);
            builder.Property(m => m.Balance).IsRequired(true).HasPrecision(24,6);
            builder.Property(m => m.Salary).IsRequired(true).HasPrecision(24, 6);
            builder.Property(m => m.BankScore).IsRequired(true).HasPrecision(24, 6);
            builder.Property(m => m.TotailDailyTransferAmount).IsRequired(true).HasPrecision(24, 6);

            builder.Property(m => m.AccountType).IsRequired(true)
               .HasConversion(
                   p => p.Value,
                   p => AccountType.FromValue(p)
               );

            builder.HasIndex(m => m.CustomerId);
            builder.HasOne(m => m.Customer)
                .WithMany(m => m.Accounts)
                .HasForeignKey(m => m.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Translations).WithOne(y => y.Account)
        .       HasForeignKey(p => p.MasterId).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
        }
    }

    [EntityAttribute(EA.ERT.AsIs, EA.EDT.TransactionalData, EA.ECT.Common, EA.ETT.Common, EA.EMT.Mandatory)]
    public class AccountTranslation : EntityBase, ITranslationDetailEntityWithNameAndDescription
    {
        public int Id { get; set; } // PK

        public int MasterId { get; set; } // FK
        public virtual Account Account { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string LanguageCode { get; set; }

    }

    public class AccountTranslationConfigurator : EntityConfigurator<AccountTranslation>
    {
        protected override void OnConfigure(EntityTypeBuilder<AccountTranslation> builder)
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