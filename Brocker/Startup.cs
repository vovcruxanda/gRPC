using Broker.Services; // Imports services related to message and connection storage.
using Broker.Services.Interfaces; // Imports interfaces for the services.
using Microsoft.AspNetCore.Builder; // Contains types for configuring the application request pipeline.
using Microsoft.AspNetCore.Hosting; // Provides functionalities for hosting ASP.NET Core applications.
using Microsoft.AspNetCore.Http; // Provides types for HTTP request and response.
using Microsoft.Extensions.DependencyInjection; // Provides mechanisms for dependency injection.
using Microsoft.Extensions.Hosting; // Provides functionalities for application hosting.

namespace Brocker // Namespace for the broker application. Note: It seems like there might be a typo here; it should probably be `Broker`.
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Adds gRPC services to the service collection.
            services.AddGrpc();

            // Registers the MessageStorageService as a singleton, meaning the same instance will be used throughout the application.
            services.AddSingleton<IMessageStorageService, MessageStorageService>();

            // Registers the ConnectionStorageService as a singleton.
            services.AddSingleton<IConnectionStorageService, ConnectionStorageService>();

            // Registers the SenderWorker as a hosted service which runs in the background.
            services.AddHostedService<SenderWorker>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Check if the environment is in development mode
            if (env.IsDevelopment())
            {
                // Enables the developer exception page for easier debugging
                app.UseDeveloperExceptionPage();
            }

            // Enables routing capabilities in the application
            app.UseRouting();

            // Configures the endpoints for the application
            app.UseEndpoints(endpoints =>
            {
                // Maps the PublisherService for gRPC calls
                endpoints.MapGrpcService<PublisherService>();

                // Maps the SubscriberService for gRPC calls
                endpoints.MapGrpcService<SubscriberService>();

                // Provides a basic HTTP GET endpoint for the root URL
                endpoints.MapGet("/", async context =>
                {
                    // Responds with a message explaining how to communicate with gRPC endpoints
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
