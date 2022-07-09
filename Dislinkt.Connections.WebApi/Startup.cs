using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Reflection;
using System.IO;
using Dislinkt.Connections.Application.RegisterUser.Commands;
using Dislinkt.Connections.Persistence.MongoDB.Factory;
using Dislinkt.Connections.Persistence.MongoDB.Common;
using Dislinkt.Connections.Core.Repositories;
using Dislinkt.Connections.Persistence.MongoDB.Repositories;
using Dislinkt.Connections.Persistence.Neo4j.Common;
using Dislinkt.Connections.Persistence.Neo4j.Repositories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MediatR;
using Newtonsoft.Json.Serialization;
using IQueryExecutor = Dislinkt.Connections.Persistence.MongoDB.Common.IQueryExecutor;
using QueryExecutor = Dislinkt.Connections.Persistence.MongoDB.Common.QueryExecutor;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger.Senders.Thrift;
using OpenTracing;
using OpenTracing.Contrib.NetCore.Configuration;
using Prometheus;

namespace Dislinkt.Connections.WebApi
{
    /// <summary>
    /// Program and service configurations
    /// </summary>
    public class Startup
    {

        /// <summary>
        /// Configuration DI
        /// </summary>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration field
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            var audienceConfig = Configuration.GetSection("Audience");
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(audienceConfig["Secret"]));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = audienceConfig["Iss"],
                ValidateAudience = true,
                ValidAudience = audienceConfig["Aud"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };
            services.AddMvc();
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:44351", "http://localhost:4200")
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                    });
            });
            services.AddMvcCore();
            services.AddControllers().AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }).AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())); ;

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Profile API", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                c.IncludeXmlComments(xmlPath);

            });
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = "Auth_key";
            })
       .AddJwtBearer("Auth_key", x =>
       {
           x.RequireHttpsMetadata = false;
           x.TokenValidationParameters = tokenValidationParameters;
       });
            services.Configure<MongoSettings>(options =>
            {
                options.Connection = Configuration.GetSection("MongoSettings:ConnectionString").Value;
                options.DatabaseName = Configuration.GetSection("MongoSettings:DatabaseName").Value;
            });
            services.AddOpenTracing();
            // Adds the Jaeger Tracer.
            services.AddSingleton<ITracer>(sp =>
            {
                var serviceName = sp.GetRequiredService<IWebHostEnvironment>().ApplicationName;
                var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                var reporter = new RemoteReporter.Builder().WithLoggerFactory(loggerFactory).WithSender(new UdpSender())
                    .Build();
                var tracer = new Tracer.Builder(serviceName)
                    // The constant sampler reports every span.
                    .WithSampler(new ConstSampler(true))
                    // LoggingReporter prints every reported span to the logging framework.
                    .WithReporter(reporter)
                    .Build();
                return tracer;
            });

            services.Configure<HttpHandlerDiagnosticOptions>(options =>
                options.OperationNameResolver =
                    request => $"{request.Method.Method}: {request?.RequestUri?.AbsoluteUri}");
            services.AddMediatR(typeof(RegisterUserCommand).GetTypeInfo().Assembly);
            services.AddScoped<IDatabaseFactory, DatabaseFactory>();
            services.AddScoped<Persistence.Neo4j.Factory.IDatabaseFactory, Persistence.Neo4j.Factory.DatabaseFactory> ();
            services.AddScoped<IQueryExecutor, QueryExecutor>();
            services.AddScoped<Persistence.Neo4j.Common.IQueryExecutor, Persistence.Neo4j.Common.QueryExecutor>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IConnectionsRepository, ConnectionsRepository>();
            services.AddScoped<MongoDbContext>();
            services.AddScoped<Neo4jDbContext>();

            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMetricServer();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Profile API V1");
                });
            }

            app.UseHttpsRedirection();
            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });


            app.UseAuthentication();
            app.UseRouting();
            app.UseHttpMetrics();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapMetrics();
            });
        }
    }
}
