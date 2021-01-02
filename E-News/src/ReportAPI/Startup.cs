using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting; 
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting; 
using MediatR;
using Microsoft.OpenApi.Models;
using Serilog; 
using MassTransit;
using System;
using System.Linq;
using System.Text;
using Elastic.Apm.NetCoreAll;
using FluentValidation;
using Infrastructure.Contracts;
using Infrastructure.Concretes;
using Infrastructure.PipelineBehaviours;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;
using ReportAPI.Services;

namespace ReportAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; } 

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(); 
            services.AddMediatR(typeof(Startup));  
            services.AddCommandValidators(new[] {typeof(Startup).Assembly});
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "",
                }); 
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            }); 

            services.AddHealthChecks();   

            services.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                    {
                        config.UseHealthCheck(provider);
                        config.Host(new Uri(Configuration.GetSection("RabbitMqSettings:host").Value),"/",
                        h =>
                        { 
                            h.Username(Configuration.GetSection("RabbitMqSettings:username").Value);
                            h.Password(Configuration.GetSection("RabbitMqSettings:password").Value);  
                        });  
                        
                    }));        
            }); 

            services.AddMassTransitHostedService();

            services.AddSingleton<IMongoDbContext, MongoDbContext>();
            services.AddSingleton<INewsRepository, NewsRepository>();
            services.AddSingleton<INewsService, NewsService>();
            services.Configure<DatabaseSettings>(Configuration.GetSection(nameof(DatabaseSettings)));

            var provider = services.BuildServiceProvider();
            var context = provider.GetService<IMongoDbContext>();
            context.Init();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage(); 

            app.UseAllElasticApm(Configuration); 
            app.Use(async (ctx, next) =>
            {
                try
                {
                    await next();
                }
                catch (ValidationException e)
                {
                    var response = ctx.Response;
                    if (response.HasStarted)
                        throw;

                    ctx.RequestServices
                        .GetRequiredService<ILogger<Startup>>()
                        .LogWarning(e, "Command could not be validated!");

                    response.Clear();
                    response.StatusCode = 400;
                    response.ContentType = "application/json";
                    await response.WriteAsync(JsonConvert.SerializeObject(new
                    {
                        Message = "Opps.. Something went terribly wrong!",
                        ModelState = e.Errors.ToDictionary(error => error.ErrorCode, error => error.ErrorMessage)
                    }), Encoding.UTF8, ctx.RequestAborted);
                }
            });
            app.UseHttpsRedirection(); 
            app.UseRouting(); 
            app.UseAuthorization(); 

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/api/v1/status");
                endpoints.MapControllers();
            }); 

            app.UseSwagger(); 

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json","Report API");
            }); 

            app.UseSerilogRequestLogging();
            
        } 
         
    }
}
