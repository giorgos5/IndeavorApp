using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Indeavor.API.Entity;
using Indeavor.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Indeavor.API
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            //BuildAppSettingsProvider();
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSingleton<ISearchService, SearchService>();

            services.AddDbContext<DatabaseSet>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowAnyOrigin(); // For anyone access.
            //corsBuilder.WithOrigins("http://localhost:56573"); // for a specific url. Don't add a forward slash on the end!
            corsBuilder.AllowCredentials();

            services.AddCors(options =>
            {
                options.AddPolicy("SiteCorsPolicy", corsBuilder.Build());
            });

            //var appSettingsSection = Configuration.GetSection("AppSettings");
            //services.Configure<AppSettings>(appSettingsSection);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("SiteCorsPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseMvc();
        }

        //private void BuildAppSettingsProvider()
        //{
        //    AppSettings.DefaultConnection = Configuration["ConnectionStrings:DefaultConnection"];
        //    //AppSettings.OutgoingAttachmentPath = Configuration["AppSettings:OutgoingAttachmentPath"];
        //    //AppSettings.IncomingAttachmentPath = Configuration["AppSettings:IncomingAttachmentPath"];
        //    //AppSettings.OutgoingRepositoryPath = Configuration["AppSettings:OutgoingRepositoryPath"];
        //    //AppSettings.IncomingRepositoryPath = Configuration["AppSettings:IncomingRepositoryPath"];
        //    //AppSettings.connection_string = Configuration["AppSettings:connection_string"];
        //    //AppSettings.SendErrorsByEmail = Configuration["AppSettings:SendErrorsByEmail"];
        //    //AppSettings.log_sender = Configuration["AppSettings:log_sender"];
        //    //AppSettings.log_receiver = Configuration["AppSettings:log_receiver"];
        //    //AppSettings.mail_server = Configuration["AppSettings:mail_server"];
        //    //AppSettings.CommandTimeOut = Configuration["AppSettings:CommandTimeOut"];
        //    //AppSettings.DefaultCurrentPage = Configuration["AppSettings:DefaultCurrentPage"];
        //    //AppSettings.DefaultPageSize = Configuration["AppSettings:DefaultPageSize"];
        //    //AppSettings.DefaultUserID = Configuration["AppSettings:DefaultUserID"];
        //}
    }
}
