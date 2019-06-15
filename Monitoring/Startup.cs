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
using Swashbuckle.AspNetCore.Swagger;

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
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "ERP API",
                    Description = "A public API for the team-team ERP system",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "Shayne Boyer",
                        Email = string.Empty,
                        Url = "https://twitter.com/spboyer"
                    },
                    License = new License
                    {
                        Name = "Use under LICX",
                        Url = "https://example.com/license"
                    }
                });
                // Set the comments path for the Swagger JSON and UI.
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DataDbContext mcontext)
        {

            app.UseNodeModules(env);
            //mcontext.Database.EnsureCreated();
            app.UseStaticFiles();
            app.UseWebSockets();
            app.UseSignalR(routes =>
            {
                routes.MapHub<DeployWorkflowHub>("/DeployWorkflowHub");              
            });
            app.UseSwagger(); //enable the use of Swagger To document the api
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
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
