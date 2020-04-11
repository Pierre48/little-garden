using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Reflection;
using LittleGarden.Data;
using Ppl.Core.Docker;
using Pump.Core.Metrics;

namespace LittleGarden.API
{
    public static class StartupExtension
    {
        public static void AddCustomEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            // var subscriptionClientName = configuration["SubscriptionClientName"];
//
            // services.AddSingleton<IEventBus, EventBusKafka>(sp =>
            // {
            //     var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
            //     var logger = sp.GetRequiredService<ILogger<EventBusKafka>>();
//
            //     return new EventBusKafka(logger, sp);
            // });
        }

        public static void AddCustomCors(this IServiceCollection services, IConfiguration conf, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                services.AddCors(options =>
                {
                    options.AddDefaultPolicy(builder =>
                    {
                        builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
                    });
                });
            }
            else if (env.IsProduction())
            {
                // var corsConfiguration = conf.GetCorsConfig();
                // services.AddCors(options =>
                // {
                //     options.AddDefaultPolicy(builder =>
                //     {
                //         builder.WithHeaders(corsConfiguration.Headers)
                //             .WithOrigins(corsConfiguration.Origins)
                //             .WithMethods(corsConfiguration.Methods);
                //     });
                // });
            }
        }

        public static void AddCustomSwagger(this IServiceCollection services)
        {
            services.AddApiVersioning(o =>
                {
                    o.ReportApiVersions = true;
                    o.AssumeDefaultVersionWhenUnspecified = true;
                    o.DefaultApiVersion = new ApiVersion(1, 0);
                })
                .AddVersionedApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });
        }

        public static void AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            /*
            IdentityModelEventSource.ShowPII = true;
            services.AddAuthorization((auth) =>
            {
                auth.AddPolicy("Bearer",
                    new AuthorizationPolicyBuilder()
                        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                        .RequireAuthenticatedUser()
                        .Build());
            });
            services.AddAuthentication(o =>
                {
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    // only for testing
                    cfg.RequireHttpsMetadata = false;
                    cfg.Authority = iamconfig.AuthUrl;
                    cfg.IncludeErrorDetails = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = false,
                        ValidateIssuer = true,
                        ValidIssuer = iamconfig.AuthUrl,
                        ValidateLifetime = true
                    };

                    cfg.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = c =>
                        {
                            c.NoResult();
                            c.Response.StatusCode = 401;
                            c.Response.ContentType = "text/plain";
                            return c.Response.WriteAsync(c.Exception.ToString());
                        }
                    };
                });
        */
        }

        public static void AddCustomServices(this IServiceCollection services)
        {
            services
                .DefineInputParameters("MetricsPort", 9998)
                .DefineInputParameters("MongoDBConnectionString", "mongodb://root:example@localhost/")
                .AddSingleton(typeof(IDataContext<>), typeof(DataContext<>))
                .AddSingleton<IMetricsServer, MetricsServer>();
        }
        
        public static void AddCustomDB(this IServiceCollection services,IConfiguration conf)
        {
        }

        public static void AddCustomSwaggerGen(this IServiceCollection services, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                services.AddSwaggerGen(options =>
                {
                    var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                    foreach (var description in provider.ApiVersionDescriptions)
                        options.SwaggerDoc(description.GroupName,
                            new OpenApiInfo()
                            {
                                Title =
                                    $"{typeof(Startup).Assembly.GetCustomAttribute<AssemblyProductAttribute>().Product} {description.ApiVersion}",
                                Version = description.ApiVersion.ToString(),
                                Description = description.IsDeprecated
                                    ? $"{typeof(Startup).Assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description} - DEPRECATED"
                                    : typeof(Startup).Assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()
                                        .Description
                            });

                    options.IncludeXmlComments(Path.Combine(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                        $"{typeof(Startup).Assembly.GetName().Name}.xml"));
                });
        }
    }
}