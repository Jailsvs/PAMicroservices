using AuctionMicroservice.DBContexts;
using AuctionMicroservice.Repository;
using AuctionMicroservice.Services;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UserMicroservice.MappingProfiles;
using ExtensionLogger;
using SwaggerOptions = SharedMicroservice.Options.SwaggerOptions;
using System;
using AuctionMicroservice.Models;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace AuctionMicroservice
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
            if (Configuration.GetValue<bool>("InMemoryDatabase"))
            { services.AddDbContext<AuctionContext>(o => o.UseInMemoryDatabase(Configuration.GetConnectionString("MicroservicesDB"))); }
            else
            { services.AddDbContext<AuctionContext>(o => o.UseSqlServer(Configuration.GetConnectionString("MicroservicesDB"))); }


            services.AddTransient<IAuctionRepository, AuctionRepository>();
            services.AddTransient<IAuctionService, AuctionService>();
            
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(AuctionProductMappings));
            });
            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSwaggerGen(x => {
                x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Auction Microservice",
                    Version = "v1"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            /*if (env.IsDevelopment()){
                app.UseDeveloperExceptionPage();
            }*/
            app.UseExceptionHandler(
               options =>
               {
                   options.Run(
                       async context =>
                       {
                           context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                           context.Response.ContentType = "application/json";
                           var exceptionObject = context.Features.Get<IExceptionHandlerFeature>();
                           if (null != exceptionObject)
                           {
                               var result = JsonConvert.SerializeObject(new { error = exceptionObject.Error.Message });
                               await context.Response.WriteAsync(result).ConfigureAwait(false);
                           }
                       });
               }
           );

            app.UseCors(option => option.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
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

            if (Configuration.GetValue<bool>("InMemoryDatabase"))
            {
                using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<AuctionContext>();
                    AddInMemory(context);
                }
            }
            if (!Configuration.GetValue<bool>("InMemoryDatabase"))
            {
                loggerFactory.AddContext(LogLevel.Information, Configuration.GetConnectionString("MicroservicesDB"));
            }
        }

        private static void AddInMemory(AuctionContext context)
        {
            context.Add(new AuctionProduct
            {
                Id = 1,
                MinValue = 100,
                OpeningDate = DateTime.Today.AddMinutes(-5),
                Description = "Bola Futsal Topper",
                StopwatchTime = 100,
                BidValue = 1,
                URLImg = "https://static.netshoes.com.br/produtos/bola-futsal-topper-slick-ii-exclusiva/20/D30-1269-120/D30-1269-120_zoom1.jpg",
                CompanyId = 1,
                TenantId = 1
            });

            context.Add(new AuctionProduct
            {
                Id = 2,
                MinValue = 200,
                OpeningDate = DateTime.Today.AddMinutes(-10),
                Description = "Furadeira Bosh 550W",
                StopwatchTime = 100,
                BidValue = 1,
                URLImg = "https://www.taqi.com.br/ccstore/v1/images/?source=/file/v8706143511674300263/products/178325.00-furadeira-hobby-impacto-black-decker-tm500kbr-220-volts.jpg&height=1000&width=1000&quality=0.9",
                CompanyId = 2,
                TenantId = 1
            });
            context.SaveChanges();
        }


    }
}
