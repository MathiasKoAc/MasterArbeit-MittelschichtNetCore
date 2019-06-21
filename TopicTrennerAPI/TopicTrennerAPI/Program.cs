using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using TopicTrennerAPI.Data;
using TopicTrennerAPI.Models;

namespace TopicTrennerAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // var context = new DbTopicTrennerContext();
            // context.Database.EnsureCreated();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
