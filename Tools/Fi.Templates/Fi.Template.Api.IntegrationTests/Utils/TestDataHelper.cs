using Fi.Infra.Schema.Const;
using Fi.TemplateUniqueName.Api.Domain.Entity;
using Fi.Persistence.Relational.Context;
using FizzWare.NBuilder;
using Fi.Test.Utility;
using Microsoft.EntityFrameworkCore;

namespace Fi.Test.Extensions;

public static class TestDataHelper
{
    /*
    internal static async Task<SampleEntity> DataPreparationForSample(FiDbContext testDbContext)
    {
        await testDbContext.EnsureEntityIsEmpty<SampleEntity>();

        var sampleEntity = Builder<SampleEntity>.CreateNew()
                                    .With(p => p.Prop1 = "Prop1")
                                    .With(p => p.Prop2 = 1)
                                    .Build();

        await testDbContext.AddAsync(sampleEntity);
        await testDbContext.SaveChangesAsync();

        return sampleEntity;
    }
    */
}