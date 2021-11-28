using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SharedCodeLibrary.Interfaces;
using SharedCodeLibrary.Providers;
using TT.Deliveries.Core.Interfaces;
using TT.Deliveries.Core.Providers;
using TT.Deliveries.Core.Services;
using TT.Deliveries.Core.Validators;
using TT.Deliveries.DummyDB.DataAccesses;
using TT.Deliveries.Web.Api.Builders;
using TT.Deliveries.Web.Api.Enums;
using TT.Deliveries.Web.Api.Extensions;
using TT.Deliveries.Web.Api.Interfaces;

namespace TT.Deliveries.Web.Api
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            this.Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TT.Deliveries.Web.Api", Version = "v1" });
            });

            ConfigureMappers(services);
            ConfigureMvc(services);
            ConfigureDependencies(services, Configuration);
            ConfigureAuthentication(services, Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TT.Deliveries.Web.Api v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(e => e.MapControllers());
        }

        private static void ConfigureDependencies(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IDbConnectionProvider, DummyDbConnectionProvider>();

            services.AddSingleton<IDeliveryDataAccess, DeliveryDataAccess>();
            services.AddSingleton<IOrderDataAccess, OrderDataAccess>();

            services.AddSingleton<IDeliveryValidator, DeliveryValidator>();

            services.AddTransient<IDeliveryService, DeliveryService>();

            services.AddTransient<IUrlsProvider, UrlsProvider>();
            services.AddTransient<IActionsBuilder, ActionsBuilder>();

            var urlProviderTemplates = configuration.Get<UrlProviderTemplates>("UrlProviderTemplates");
            services.AddSingleton<IUrlProviderTemplates>(urlProviderTemplates);

            var dateTimeFormatter = (string)configuration.GetValue(typeof(string), "DateTimeFormatter");
            services.AddSingleton<IDateTimeProvider>(new DateTimeProvider(dateTimeFormatter));
        }

        private static void ConfigureMappers(IServiceCollection services)
        {
            services.AddSingleton(config => new MapperConfiguration(cfg =>
            {
                var urlsProvider = config.GetService<IUrlsProvider>();
                var dateTimeProvider = config.GetService<IDateTimeProvider>();
                var actionsBuilder = config.GetService<IActionsBuilder>();

                cfg.AddProfile(new AutoMapper.EntityProfile(urlsProvider, dateTimeProvider, actionsBuilder));
                cfg.AddProfile(new AutoMapper.ModelProfile(dateTimeProvider));
                cfg.AddProfile(new DummyDB.AutoMapper.EntityProfile());
                cfg.AddProfile(new DummyDB.AutoMapper.ModelProfile());
            }).CreateMapper());
        }

        private static void ConfigureMvc(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true;
                options.ReturnHttpNotAcceptable = true;
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });
        }

        private static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            var actualEnvironment = configuration.GetWorkEnvironmentType();

            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            });

            //This is the place to configure AzureAdBearer and Azure Graphs authorization.

            if (actualEnvironment == EnvironmentType.Local ||
                actualEnvironment == EnvironmentType.Development)
            {
                //Disable authentication for developers to make it easier to test features.
            }
        }
    }
}
