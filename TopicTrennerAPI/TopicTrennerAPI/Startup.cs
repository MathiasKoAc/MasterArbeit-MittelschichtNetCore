using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TopicTrennerAPI.Interfaces;
using TopicTrennerAPI.Service;
using TopicTrennerAPI.Data;
using System;

namespace TopicTrennerAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        readonly string MyAllowAll = "_myAllowAll";

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                //Alle durchlassen
                options.AddPolicy(MyAllowAll, builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });

                //Nur Spezielle durchlassen
                options.AddPolicy(MyAllowSpecificOrigins, builder =>
                {
                    builder.WithOrigins("http://localhost",
                        "https://localhost",
                        "http://localhost:63342",
                        "https://localhost:63342",
                        "http://localhost:8000",
                        "https://localhost:8000")
                        .AllowAnyHeader().
                        AllowAnyMethod();
                });
            });

            this.SetupDbCorrect(Configuration.GetConnectionString("DbDefaultConnection"));
            services.AddDbContext<DbTopicTrennerContext>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddTransient<IServerConfig, ServerConfigConnectionString>();
            services.AddSingleton<IMqttConnector, MqttConnectAll>();
            services.AddSingleton<IServeRuleEvaluation, RuleEvaluationDenyAccessDeny>();
            services.AddTransient<ICreateTopicVertexFromTopics, TopicVertexFactory>();
            services.AddScoped<IManageRuleService, RuleServiceManager>();
            services.AddSingleton<IServeTime, MqttTimeService>();
            services.AddScoped<IManageTimeService, TimeServiceManager>();
            services.AddSingleton<IServeLogging, LogServiceInDbContext>();
            services.AddScoped<IManageLogService, LogServiceManager>();
            services.AddSingleton<IServeEvents, EventService>();
            services.AddScoped<IManageEventService, EventServiceManger>();
            services.AddScoped<IFindSessionRun, ActivSessionRun>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(MyAllowAll);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private void SetupDbCorrect(string config)
        {
            DbTopicTrennerContext.DbConfigString = config;
            DbTopicTrennerContext context = new DbTopicTrennerContext();

            if (context != null)
            {
                context.Database.EnsureCreated();
            }
            else
            {
                throw new Exception("No Connection to SQL-Server");
            }
            
        }
    }
}
