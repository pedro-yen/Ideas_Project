using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Raven.Client.Documents;
using Raven.Client.ServerWide.Operations;
using Raven.Client.ServerWide;
using System.Linq;
using Backend.Challenge.BusinessManager;
using Backend.Challenge._2._Data.Repositories;
using FluentValidation.AspNetCore;
using FluentValidation;
using System.Text.Json.Serialization;

namespace Backend.Challenge
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddControllers().AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            #region IOC
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<Startup>();

            // Register RavenDB DocumentStore
            services.AddSingleton<IDocumentStore>(provider =>
            {
                var databaseName = "Ideas_CRUD";
                var store = new DocumentStore
                {
                    Urls = new[] { "http://localhost:8080" },
                    Database = databaseName
                };

                var initialized = false;
                int retries = 5;
                while (!initialized && retries-- > 0)
                {
                    try
                    {
                        store.Initialize();

                        // Check if the database exists
                        var dbNames = store.Maintenance.Server.Send(new GetDatabaseNamesOperation(0, 25));
                        if (!dbNames.Contains(databaseName))
                        {
                            var dbRecord = new DatabaseRecord(databaseName);
                            store.Maintenance.Server.Send(new CreateDatabaseOperation(dbRecord));
                        }

                        initialized = true;
                    }
                    catch
                    {
                        if (retries == 0) throw;
                        System.Threading.Thread.Sleep(2000); // Wait for 2 seconds
                    }
                }

                // Only set this AFTER the DB is created
                return store;
            });
            services.AddSingleton<IUsersBusinessManager, UsersBusinessManager>();
            services.AddSingleton<IIdeasBusinessManager, IdeasBusinessManager>();
            services.AddSingleton<IRepository, MainRepository>();
            #endregion

            // Add Swagger services
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Enable Swagger and Swagger UI in development
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.RoutePrefix = string.Empty; // This makes Swagger UI available at the root (http://localhost:000/ or http://localhost:5001/)
                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
