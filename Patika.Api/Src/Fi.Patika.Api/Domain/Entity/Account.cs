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
    public class Account : EntityBaseWithBaseFieldsWithIdentity
    {
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
    }

    public class AccountConfigurator : EntityConfigurator<Account>
    {
        protected override void OnConfigure(EntityTypeBuilder<Account> builder)
        {
            builder.Property(m => m.Balance).IsRequired(true);
            builder.Property(m => m.Salary).IsRequired(true);
            builder.Property(m => m.BankScore).IsRequired(true);
            builder.Property(m => m.TotailDailyTransferAmount).IsRequired(true);

            builder.Property(m => m.AccountType).IsRequired(true)
               .HasConversion(
                   p => p.Value,
                   p => AccountType.FromValue(p)
               );

            builder.HasIndex(m => m.CustomerId);
            builder.HasOne(m => m.Customer)
                .WithMany(m => m.Accounts)
                .HasForeignKey(m => m.CustomerId);
        }
    }
}