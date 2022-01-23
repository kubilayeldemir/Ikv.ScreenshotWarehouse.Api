using System;
using Ikv.ScreenshotWarehouse.Api.Persistent.Contexts;
using Ikv.ScreenshotWarehouse.Api.Repositories;
using Ikv.ScreenshotWarehouse.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Ikv.ScreenshotWarehouse.Api
{
    public class Startup
    {
        public static readonly string JwtSecretKey = Environment.GetEnvironmentVariable("ECOMMERCE_JWT_SECRET");
        private static readonly string DbConnString = Environment.GetEnvironmentVariable("IKVDBCONNSTRING");

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Ikv.ScreenshotWarehouse.Api", Version = "v1"});
            });
            
            services.AddDbContext<IkvContext>(options => options.UseNpgsql(DbConnString));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ikv.ScreenshotWarehouse.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}