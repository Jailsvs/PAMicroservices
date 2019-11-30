using CompanyMicroservice.DBContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
//using Microsoft.EntityFrameworkCore.SqlServer; //dotnet add package Microsoft.EntityFrameworkCore.SqlServer
using Microsoft.EntityFrameworkCore;
using CompanyMicroservice.Repository;
using CompanyMicroservice.Services;
using AutoMapper;
using CompanyMicroservice.MappingProfiles;
using Microsoft.Extensions.Logging;
using ExemploLogCore.ExtensionLogger;
using Swashbuckle.AspNetCore.Swagger;
using SharedMicroservice.Options;
using SwaggerOptions = SharedMicroservice.Options.SwaggerOptions;

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

            services.AddDbContext<CompanyContext>(o => o.UseSqlServer(Configuration.GetConnectionString("MicroservicesDB")));
            services.AddDbContext<TenantContext>(o => o.UseSqlServer(Configuration.GetConnectionString("MicroservicesDB")));

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

            services.AddSwaggerGen(x => {
                x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Company Microservice",
                Version = "v1"});
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
        
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
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

            app.UseSwagger(option => {option.RouteTemplate = swaggerOptions.JsonRoute;  });
            app.UseSwaggerUI(option => { option.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description); });


            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "swagger/ui";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI(v1)");
            });

            loggerFactory.AddContext(LogLevel.Information, Configuration.GetConnectionString("MicroservicesDB"));

           
        }
    }
}
