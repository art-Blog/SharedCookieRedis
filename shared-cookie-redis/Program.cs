using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SharedCookieRedis;

namespace shared_cookie_redis
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SetEnvironmentVariable();
            CreateWebHostBuilder(args).Build().Run();
        }

        private static void SetEnvironmentVariable()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (env == "Lab")
            {
                Environment.SetEnvironmentVariable("redis", "myredis,abortConnect=false,syncTimeout=10000");
            }
        }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
