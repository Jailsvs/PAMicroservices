using CompanyMicroservice.DBContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using CompanyMicroservice.Repository;
using CompanyMicroservice.Services;
using AutoMapper;
using CompanyMicroservice.MappingProfiles;
using Microsoft.Extensions.Logging;
using ExtensionLogger;
using SwaggerOptions = SharedMicroservice.Options.SwaggerOptions;
using CompanyMicroservice.Models;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace CompanyMicroservice
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
            {
                services.AddDbContext<CompanyContext>(o => o.UseInMemoryDatabase(Configuration.GetConnectionString("MicroservicesDB")));
                services.AddDbContext<TenantContext>(o => o.UseInMemoryDatabase(Configuration.GetConnectionString("MicroservicesDB")));
            }
            else
            {
                services.AddDbContext<CompanyContext>(o => o.UseSqlServer(Configuration.GetConnectionString("MicroservicesDB")));
                services.AddDbContext<TenantContext>(o => o.UseSqlServer(Configuration.GetConnectionString("MicroservicesDB")));
            }

            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<ICompanyService, CompanyService>();
            services.AddTransient<ITenantRepository, TenantRepository>();
            services.AddTransient<ITenantService, TenantService>();

            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(TenantMappings));
                cfg.AddProfile(typeof(CompanyMappings));
            });
            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Company Microservice",
                    Version = "v1"
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {

            /*if (env.IsDevelopment())
            {
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

            app.UseCors(option => option.AllowAnyOrigin());
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
                    var tenantContext = scope.ServiceProvider.GetService<TenantContext>();
                    var companyTontext = scope.ServiceProvider.GetService<CompanyContext>();
                    AddInMemory(tenantContext, companyTontext);
                }
            }

            if (!Configuration.GetValue<bool>("InMemoryDatabase"))
            {
                loggerFactory.AddContext(LogLevel.Information, Configuration.GetConnectionString("MicroservicesDB"));
            }
        }

        private static void AddInMemory(TenantContext tenantContext, CompanyContext companyContext)
        {
            tenantContext.Add(new Tenant
            {
                Id = 1,
                Name = "Fast and Furious Auto Postos",
                Host = "www.fastandfouriousAP.com.br",
                TenantId = 0
            });

            companyContext.Add(new Company
            {
                Id = 1,
                Name = "Empresa Teste 001",
                CNPJ = "51.965.287/0001-25",
                Email = "empteste001@api.com.br",
                Whats = "47999999999",
                TenantId = 1

            });
            companyContext.Add(new Company
            {
                Id = 2,
                Name = "Empresa Teste 002",
                CNPJ = "40.871.824/0001-51",
                Email = "empteste002@api.com.br",
                Whats = "47999000000",
                TenantId = 1

            });
            companyContext.Add(new Company
                {
                    Id = 3,
                    Name = "Empresa Teste 003",
                    CNPJ = "83.145.029/0001-99",
                    Email = "empteste003@api.com.br",
                    Whats = "47999888888",
                    TenantId = 1

                });
            companyContext.Add(new Company
                {
                    Id = 4,
                    Name = "Empresa Teste 004",
                    CNPJ = "14.145.576/0001-51",
                    Email = "empteste004@api.com.br",
                    Whats = "47999111111",
                    TenantId = 1

                });
            tenantContext.SaveChanges();
            companyContext.SaveChanges();
        }
    }
 }
