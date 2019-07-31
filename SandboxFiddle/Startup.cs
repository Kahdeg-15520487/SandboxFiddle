using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SandboxFiddle.AppService;
using SandboxFiddle.Contract;

namespace SandboxFiddle
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddTransient<CSharpScriptEngine>();
            services.AddTransient<JSScriptEngine>();
            services.AddTransient<Func<SupportedScriptEngine, IScriptEngine>>(serviceProvider => key =>
            {
                switch (key)
                {
                    case SupportedScriptEngine.Csharp:
                        return serviceProvider.GetService<CSharpScriptEngine>();
                    case SupportedScriptEngine.Js:
                        return serviceProvider.GetService<JSScriptEngine>();
                    default:
                        throw new KeyNotFoundException();
                }
            });

            services.AddTransient<IScriptingService, ScriptingService>();
            services.AddNodeServices();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader().WithExposedHeaders("Pagination-Count", "Pagination-Page", "Pagination-Limit", "Content-Disposition");
            });

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();
        }
    }
}
