using System;
using Fi.Infra.Const;
using Fi.Infra.Context;
using Fi.Infra.Exceptions;
using Fi.Infra.Impl;
using Fi.Infra.Options;
using Fi.Infra.Schema.Attributes;
using Fi.Persistence.Relational.Context;
using Fi.Persistence.Relational.Interfaces;
using Fi.Persistence.Relational.Context.Factory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fi.Patika.Api.Persistence
{
    public class FiPatikaDbContext : FiDbContext, IFiModuleDbContext
    {
        public FiPatikaDbContext(IFiDbContextFactory fiDbContextFactory) : base(fiDbContextFactory)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder, string dbName, string applicationName, int sqlCommandTimeout)
        {
            optionsBuilder.EnableSensitiveDataLogging();

            string connectionString = string.Format(databaseSettings.DatabaseCluster.ConnectionString,
                    databaseSettings.DatabaseCluster.HostName,
                    dbName,
                    databaseSettings.DatabaseCluster.UserName,
                    databaseSettings.DatabaseCluster.Password,
                    applicationName);

            optionsBuilder.UseSqlServer(connectionString,
                                        sqlServerOptions => sqlServerOptions.CommandTimeout(sqlCommandTimeout));
        }
    }
}
