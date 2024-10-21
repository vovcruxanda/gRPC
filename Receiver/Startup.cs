using System; // Importing the System namespace which includes fundamental classes and base classes that define commonly used values and reference data types.
using System.Collections.Generic; // Importing the System.Collections.Generic namespace which provides classes for defining generic collections.
using System.Linq; // Importing the System.Linq namespace which provides classes and interfaces that support queries that use Language Integrated Query (LINQ).
using System.Threading.Tasks; // Importing the System.Threading.Tasks namespace which provides types that simplify the work of writing concurrent and asynchronous code.
using Microsoft.AspNetCore.Builder; // Importing the Microsoft.AspNetCore.Builder namespace which provides types for configuring the application's request pipeline.
using Microsoft.AspNetCore.Hosting; // Importing the Microsoft.AspNetCore.Hosting namespace which contains types for hosting web applications.
using Microsoft.AspNetCore.Http; // Importing the Microsoft.AspNetCore.Http namespace which provides classes for handling HTTP requests and responses.
using Microsoft.Extensions.DependencyInjection; // Importing the Microsoft.Extensions.DependencyInjection namespace which provides support for dependency injection.
using Microsoft.Extensions.Hosting; // Importing the Microsoft.Extensions.Hosting namespace which provides hosting capabilities for applications.
using Receiver.Services; // Importing the Receiver.Services namespace which contains the service implementations, like NotificationService.

namespace Receiver // Defining a namespace called Receiver to group related classes for the receiver service.
{
    public class Startup // Defining the Startup class that configures services and the request pipeline for the application.
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) // Method to add services to the container.
        {
            services.AddGrpc(); // Adds gRPC services to the service collection, allowing the application to handle gRPC requests.
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) // Method to configure the HTTP request pipeline.
        {
            // Check if the environment is in development mode.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // Enables a developer exception page for detailed error information.
            }

            app.UseRouting(); // Enables routing capabilities to the application.

            app.UseEndpoints(endpoints => // Configures the application's endpoints.
            {
                endpoints.MapGrpcService<NotificationService>(); // Maps the NotificationService gRPC service to the endpoint.

                // Maps a GET request to the root URL ("/") to return a message.
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
