using AutoMapper;
using ExtensionLogger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharedMicroservice.Services;
using UserMicroservice.DBContexts;
using UserMicroservice.MappingProfiles;
using UserMicroservice.Models;
using UserMicroservice.Repository;
using UserMicroservice.Services;
using SwaggerOptions = SharedMicroservice.Options.SwaggerOptions;

namespace UserMicroservice
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

            //services.AddDbContext<UserContext>(o => o.UseSqlServer(Configuration.GetConnectionString("MicroservicesDB")));
            services.AddDbContext<UserContext>(o => o.UseInMemoryDatabase(Configuration.GetConnectionString("MicroservicesDB")));
            
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IEncrypter , Encrypter>();

            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(UserMappings));
            });
            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSwaggerGen(x => {
                x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "User Microservice",
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
            app.UseCors(option => option.AllowAnyOrigin());
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions); //bind with model by configuration appsettings.json

            app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });
            app.UseSwaggerUI(option => { option.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description); });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "swagger/ui";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI(v1)");
            });

            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<UserContext>();
                AddInMemory(context);              
            }
            //var context = app.ApplicationServices.GetService<UserContext>();
            //AddInMemory(context);
            //loggerFactory.AddContext(LogLevel.Information, Configuration.GetConnectionString("MicroservicesDB"));

            }

        private static void AddInMemory(UserContext context)
        {
            context.Add(new User
            {
                Id = 1,
                Name = "Jailson VS",
                AvailableBids = 10,
                Email = "user001@gmail.com",
                Password = "R$%TGss5",
                TenantId = 1,
                UserType = "C",
                Whats = "47999999999"

            });
            context.Add(new User
                {
                    Id = 2,
                    Name = "Jhon WF",
                    AvailableBids = 100,
                    Email = "user002@gmail.com",
                    Password = "#$#HJJTT@88",
                    TenantId = 1,
                    UserType = "C",
                    Whats = "47999000000"
                });
            context.Add(
                new User
                {
                    Id = 3,
                    Name = "Kay QG",
                    AvailableBids = 100,
                    Email = "user002@gmail.com",
                    Password = "UY6%$$885",
                    TenantId = 1,
                    UserType = "C",
                    Whats = "47999888888"
                });
            context.SaveChanges();
        }
    }
}
