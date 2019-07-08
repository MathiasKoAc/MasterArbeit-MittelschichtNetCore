using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TopicTrennerAPI.Interfaces;
using TopicTrennerAPI.Service;
using TopicTrennerAPI.Data;

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
            services.AddSingleton<IRuleEvaluation, RuleEvaluationDenyAccessDeny>();
            services.AddTransient<ICreateTopicRulesFromSimpleRules, TopicRuleFactory>();
            services.AddScoped<IControlRuleSessions, RuleSessionManager>();
            services.AddSingleton<IServeTime, MqttTimeService>();
            services.AddScoped<IControlTimeSession, TimeSessionManager>();

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
