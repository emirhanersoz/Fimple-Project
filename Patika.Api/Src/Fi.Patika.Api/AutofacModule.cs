using Autofac;
using Fi.Patika.Api.Persistence;
using Fi.Persistence.Relational.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Fi.Patika.Api
{
    public class AutofacModule : Autofac.Module,
                                 IFiEfCoreModuleLoader
    {
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

            builder.RegisterType<FiPatikaDbContext>().As<IFiModuleDbContext>().InstancePerLifetimeScope();
        }
    }
}