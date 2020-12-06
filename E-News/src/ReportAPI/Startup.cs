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
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage(); 

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
