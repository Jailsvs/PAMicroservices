using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SharedMicroservice.Constants;
using SharedMicroservice.DTO;
using StopwatchMicroservice.Services;
using StopwatchMicroservice.Tasks;
using ExtensionLogger;
using SwaggerOptions = SharedMicroservice.Options.SwaggerOptions;
using System.Diagnostics;

namespace StopwatchMicroservice
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
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddControllers();
            services.AddCors();
            services.AddTransient<IStopwatchService, StopwatchService>();
            services.AddTransient<StopwatchHub>();

            ConcurrentDictionary<int, StopwatchAuction> _stopwatchs = new ConcurrentDictionary<int, StopwatchAuction>();
            services.AddSingleton(_stopwatchs);
            services.AddSwaggerGen(x => {
                x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Stopwatch Microservice",
                    Version = "v1"
                });
            });

            services.AddSignalR();
            _ = AllAuctions(_stopwatchs);

        }

        private async Task AllAuctions(ConcurrentDictionary<int, StopwatchAuction> stopwatchs)
        {
            HttpClient _httpClientAuctionSrv = new HttpClient();
            _httpClientAuctionSrv.BaseAddress = new Uri(ServiceConstants.AUCTIONSERVICEAPI_URL);
            _httpClientAuctionSrv.DefaultRequestHeaders.Accept.Clear();
            _httpClientAuctionSrv.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                HttpResponseMessage response = await _httpClientAuctionSrv.GetAsync("api/Auction?tenantId=1");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    IEnumerable<AuctionProductIndexDTO> auctions = JsonConvert.DeserializeObject<IEnumerable<AuctionProductIndexDTO>>(data);
                    foreach (AuctionProductIndexDTO a in auctions)
                    {
                        StopwatchAuction s = new StopwatchAuction(a.Id, a.StopwatchTime, a.OpeningDate, stopwatchs);
                        stopwatchs.TryAdd(a.Id, s);
                    }
                }
            }
            catch (Exception e)
            {
                //StopwatchAuction s = new StopwatchAuction(6, 1000, DateTime.Parse("2019-11-20T20:00:00"), stopwatchs);
                //bool add = stopwatchs.TryAdd(6, s);
                Debug.WriteLine("stopwatch add error: " + e.Message);
            }
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(option => option.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod().AllowCredentials());
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<StopwatchHub>("/signalr");
            });

            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

            app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });
            app.UseSwaggerUI(option => { option.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description); });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "swagger/ui";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI(v1)");
            });

            //loggerFactory.AddContext(LogLevel.Information, Configuration.GetConnectionString("MicroservicesDB"));
        }

    }
}
