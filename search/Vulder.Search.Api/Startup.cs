﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Vulder.Search.Api.Hubs;
using Vulder.Search.Core.Services;
using Vulder.Search.Infrastructure.Data.Config;

namespace Vulder.Search.Api
{
    public class Startup
    {
        public Startup()
        {
            
        }
        
        public IConfiguration Configuration { get; }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ElasticsearchConfiguration>(
                Configuration.GetSection(nameof(ElasticsearchConfiguration)));
            services.AddSingleton<IElasticsearchConfiguration>(sp =>
                sp.GetRequiredService<IOptions<ElasticsearchConfiguration>>().Value);
            services.AddSignalR();
            services.AddSingleton<SchoolsCollectionService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<SearchHub>("searchHub");
            });
        }
    }
}
