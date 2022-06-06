using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dislinkt.Connections.Application.RegisterUser.Commands;
using Dislinkt.Connections.Persistence.MongoDB.Factory;
using Dislinkt.Connections.Persistence.MongoDB.Common;
using Dislinkt.Connections.Core.Repositories;
using Dislinkt.Connections.Persistence.MongoDB.Repositories;
using Dislinkt.Connections.Persistence.Neo4j.Common;
using Dislinkt.Connections.Persistence.Neo4j.Repositories;
using MediatR;
using IQueryExecutor = Dislinkt.Connections.Persistence.MongoDB.Common.IQueryExecutor;
using QueryExecutor = Dislinkt.Connections.Persistence.MongoDB.Common.QueryExecutor;

namespace GrpcService
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            services.AddMediatR(typeof(RegisterUserCommand).GetTypeInfo().Assembly);
            // Dislinkt.Connections.Persistence.MongoDB
            services.AddScoped<IDatabaseFactory, DatabaseFactory>();
            services.AddScoped<Dislinkt.Connections.Persistence.Neo4j.Factory.IDatabaseFactory, Dislinkt.Connections.Persistence.Neo4j.Factory.DatabaseFactory>();
            services.AddScoped<IQueryExecutor, QueryExecutor>();
            services.AddScoped<Dislinkt.Connections.Persistence.Neo4j.Common.IQueryExecutor, Dislinkt.Connections.Persistence.Neo4j.Common.QueryExecutor>();
            services.AddScoped<IConnectionsRepository, ConnectionsRepository>();
            services.AddScoped<MongoDbContext>();
            services.AddScoped<Neo4jDbContext>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<GreeterService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
