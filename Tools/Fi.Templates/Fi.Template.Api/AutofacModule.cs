using Autofac;
using Fi.Template.Api.Persistence;
#if (RelationalDatabase)
using Fi.Persistence.Relational.Interfaces;
using Microsoft.Extensions.DependencyInjection;
#elif (NoSqlDatabase)
using Fi.Persistence.NoSql;
#endif

namespace Fi.Template.Api
{
    public class AutofacModule : Autofac.Module,
#if (RelationalDatabase)
                                 IFiEfCoreModuleLoader
#elif (NoSqlDatabase)
                                 IFiCouchbaseModuleLoader
#endif
    {
#if (RelationalDatabase)
        public void LoadDbContext(IServiceCollection serviceCollection)
        {
        }

        public void RegisterFiModule(ContainerBuilder containerBuilder)
        {
            Load(containerBuilder);
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterMediatorTypeImplementations(System.Reflection.Assembly.GetExecutingAssembly());

            builder.RegisterType<FiTemplateUniqueNameDbContext>().As<IFiModuleDbContext>().InstancePerLifetimeScope();
        }
#elif (NoSqlDatabase)
        public void LoadCollections(IFiCollectionInfoBuilder builder)
        {
            builder.Add(new CollectionInfo(CollectionInfo.DefaultScope, CollectionInfo.DefaultScope, NoSqlCollectionInfo.BucketTemplateUniqueName));
        }

        public void RegisterFiModule(ContainerBuilder containerBuilder)
        {
            Load(containerBuilder);
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterMediatorTypeImplementations(ThisAssembly);
        }
#endif
    }
}