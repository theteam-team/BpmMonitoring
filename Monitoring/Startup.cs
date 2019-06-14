using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Monitoring.Data;
using Monitoring.Hubs;
using Monitoring.Interfaces;
using Monitoring.Repository;

namespace Monitoring
{

    public class Startup
    {
        public IConfiguration _config { get; }
        public Startup(IConfiguration configuration)
        {

            _config = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddTransient<INodeLangRepository, NodeLangRepository>();
            services.AddDbContext<DataDbContext>(options =>
               options.UseSqlServer(_config.GetConnectionString("DefaultConnection")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DataDbContext mcontext)
        {

            app.UseNodeModules(env);
            mcontext.Database.EnsureCreated();
            app.UseStaticFiles();
            //app.UseWebSockets();
            app.UseSignalR(routes =>
            {
                routes.MapHub<DeployWorkflowHub>("/DeployWorkflowHub");              
            });
            app.UseMvc(cfg =>
            {

                cfg.MapRoute("default", "{controller}/{action}/{id?}", new { controller = "App", action = "index" });

            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

           
        }
    }
}
