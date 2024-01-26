using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fi.Patika.Api
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

            new AutofacModule().LoadDbContext(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            base.Configure(app, env);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            base.ConfigureContainer(builder);

            builder.RegisterFiPersistenceEfCoreModule();


            new AutofacModule().RegisterFiModule(builder);
        }
    }
}
