using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Fi.Infra.Context;
using Fi.Infra.Schema.Attributes;
using Fi.Infra.Schema.Model;
using Fi.TemplateUniqueName.Schema.Model;
using Fi.Persistence.Relational.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fi.Template.Api.Domain.Entity
{
    //[EntityAttribute(EA.ERT.AsIs, EA.EDT.DefinitionData, EA.ECT.Common, EA.ETT.Common, EA.EMT.Mandatory)]
    public class Sample : EntityBaseWithBaseFieldsWithIdentity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public SampleType SampleType { get; set; }
    }

    public class SampleConfiguration : IEntityTypeConfiguration<Sample>
    {
        public void Configure(EntityTypeBuilder<Sample> builder)
        {
            builder.Property(m => m.Code).IsRequired(true).HasMaxLength(10);
            builder.Property(m => m.Name).IsRequired(true).HasMaxLength(100);
            builder.Property(m => m.Description).IsRequired(true).HasMaxLength(500);

            builder.Property(m => m.SampleType).IsRequired(true)
                   .HasConversion(
                       p => p.Value,
                       p => SampleType.FromValue(p)
                   );

            builder.HasIndex(p => new { p.Code }).IsUnique();
        }
    }
}