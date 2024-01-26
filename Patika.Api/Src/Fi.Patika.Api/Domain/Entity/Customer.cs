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
    public class Customer : EntityBaseWithBaseFieldsWithIdentity
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Address { get; set; }


        public virtual User User { get; set; }
        public virtual List<Account> Accounts { get; set; }
        public virtual List<SupportRequest> SupportRequests { get; set; }
    }

    public class CustomerConfigurator : EntityConfigurator<Customer>
    {
        protected override void OnConfigure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(m => m.UserId).IsRequired(true);
            builder.Property(m => m.Name).IsRequired(true)
                        .HasMaxLength(100);
            builder.Property(m => m.PhoneNumber).IsRequired(true)
                        .HasMaxLength(20);
            builder.Property(m => m.Email).IsRequired(true)
                        .HasMaxLength(100);
            builder.Property(m => m.DateOfBirth).IsRequired(true);
            builder.Property(m => m.City).IsRequired(true)
                        .HasMaxLength(50);
            builder.Property(m => m.State).IsRequired(true)
                        .HasMaxLength(50);
            builder.Property(m => m.Address).IsRequired(true)
                        .HasMaxLength(200);

            builder.HasIndex(m => m.UserId);
            builder.HasOne(m => m.User)
                .WithMany(m => m.Customers)
                .HasForeignKey(m => m.UserId);
        }
    }
}