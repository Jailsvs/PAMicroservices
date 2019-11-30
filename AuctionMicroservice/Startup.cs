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
using ExemploLogCore.ExtensionLogger;
using SwaggerOptions = SharedMicroservice.Options.SwaggerOptions;


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

            /*services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));*/

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddControllers();

            services.AddCors();

            services.AddDbContext<AuctionContext>(o => o.UseSqlServer(Configuration.GetConnectionString("MicroservicesDB")));
            
            services.AddTransient<IAuctionRepository, AuctionRepository>();
            services.AddTransient<IAuctionService, AuctionService>();
            //services.AddTransient<HttpClient>();


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
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(option => option.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            //app.UseCors(option => option.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod().AllowCredentials());

            // app.UseCors("MyPolicy");

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

            loggerFactory.AddContext(LogLevel.Information, Configuration.GetConnectionString("MicroservicesDB"));

           
        }
    }
}
