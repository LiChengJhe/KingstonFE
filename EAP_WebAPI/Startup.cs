using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EAP_Library.Configs;
using EAP_WebAPI.Configs;
using EAP_WebAPI.Contexts;
using EAP_WebAPI.HostedServices;
using EAP_WebAPI.Hubs;
using EAP_WebAPI.Mapper;
using EAP_WebAPI.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;

namespace EAP_WebAPI
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
            SetupWebConfigs(services);
            SetupServiceConfigs(services);
            SetupServices(services);
            SetupHostedServices(services);
        }

        public void SetupHostedServices(IServiceCollection services)
        {

            services.AddHostedService<DashboardHostedService>();
            services.AddHostedService<RepositoryHostedService>();
            
        }
        public void SetupServices(IServiceCollection services)
        {
            services.AddTransient<KingstonContext>();
            services.AddTransient<EqpAlarmRepository>();
            services.AddTransient<EqpInfoRepository>();
            services.AddTransient<EqpStatusRepository>();
            services.AddTransient<EqpWipRepository>();
            services.AddTransient<UnitOfWork>();
            services.AddTransient<EqpCachedRepository>();
            
        }
        public void SetupServiceConfigs(IServiceCollection services)
        {

            services.Configure<MQConfig>(Configuration.GetSection("MQConfig"));
        }
        public void SetupWebConfigs(IServiceCollection services)
        {
            services.AddControllers();
            services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(Configuration.GetSection("Redis").Get<RedisConfiguration>());
            services.AddAutoMapper(typeof(AutoMapping));
    

            //    .AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            //    options.JsonSerializerOptions.PropertyNamingPolicy = null;
            //});


            services.AddSignalR(hubOptions =>
            {
                hubOptions.EnableDetailedErrors = true;
            });

            //.AddJsonProtocol(options =>
            //{
            //    options.PayloadSerializerOptions.PropertyNamingPolicy = null;
            //    options.PayloadSerializerOptions.WriteIndented = false;
            //});


            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(builder =>
            {
                builder
                .SetIsOriginAllowed(_ => true)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            });
            app.UseAuthorization();
            app.UseWebSockets();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<DashboardHub>("/dashboard-hub", options =>
                {
                    //options.Transports =
                    //    HttpTransportType.WebSockets |
                    //    HttpTransportType.LongPolling;

                });
                endpoints.MapControllers();
            });
        }
    }
}
