using Autofac;
#if (NoSqlDatabase)
using Fi.Persistence.NoSql;
#endif
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fi.Template.Api
{
    public class Startup : ApiBase.StartupBase
    {
        protected override string ModuleVersion => "v1";

        public Startup(IConfiguration configuration, IWebHostEnvironment env) : base(configuration, env) { }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddAutoMapper(options => options.AddProfile<AutoMapperProfile>());

#if (RelationalDatabase)
            new AutofacModule().LoadDbContext(services);
#endif
        }

#if (RelationalDatabase)
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            base.Configure(app, env);
        }

#elif (NoSqlDatabase)
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IFiCollectionInfoBuilder inf)
        {
            base.Configure(app, env);

            new AutofacModule().LoadCollections(inf);
        }
#endif
        public void ConfigureContainer(ContainerBuilder builder)
        {
            base.ConfigureContainer(builder);

#if (RelationalDatabase)
            builder.RegisterFiPersistenceEfCoreModule();

#elif (NoSqlDatabase)
            builder.RegisterFiPersistenceCouchbaseModule();
#endif

            new AutofacModule().RegisterFiModule(builder);
        }
    }
}
