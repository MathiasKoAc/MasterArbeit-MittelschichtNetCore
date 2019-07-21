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

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            this.SetupDbCorrect(Configuration.GetConnectionString("DbDefaultConnection"));
            services.AddDbContext<DbTopicTrennerContext>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddTransient<IServerConfig, ServerConfigConnectionString>();
            services.AddSingleton<IMqttConnector, MqttConnectAll>();
            services.AddSingleton<IServeRuleEvaluation, RuleEvaluationDenyAccessDeny>();
            services.AddTransient<ICreateTopicRulesFromSimpleRules, TopicRuleFactory>();
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
            var context = new DbTopicTrennerContext();
            context.Database.EnsureCreated();
        }
    }
}
