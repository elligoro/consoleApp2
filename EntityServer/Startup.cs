using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using EntityServer.Business;
using EntityServer.Contracts;
using EntityServer.Infra.Extensions;
using EntityServer.Persist;
using EntityServer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EntityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureContainer(Autofac.ContainerBuilder builder)
        {
            // logic
            builder.RegisterType<AuthLogic>();

            // base token service
            builder.RegisterType<TokenService>();
            builder.RegisterType<JsonTokenService>();

            builder.RegisterType<HmacTokenService>().As<ITokenService>().InstancePerLifetimeScope();

            // persist
            builder.RegisterType<AuthPersist>().As<IAuthPersist>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<UserAuthDbContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("DbConnection"),
                    builder =>
                {
                    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseStatusCodePagesWithReExecute("/api/error/{0}");
            }

            //app.ConfigureExceptionsMiddleware();

            //app.ConfigureCustomExceptionsMiddleware();

            app.ConfigureTestExceptionMiddleware();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
