using System; // Basic system utilities (like Console, Exception, etc.)
using System.Collections.Generic; // (Unused) For working with collections like lists and dictionaries.
using System.IO; // (Unused) For handling input/output file operations.
using System.Linq; // (Unused) Provides LINQ querying functionality.
using System.Threading.Tasks; // (Unused here) For working with asynchronous programming.
using Brocker; // (Unused) A custom namespace, might contain shared components for the broker application.
using Common; // This imports common utility classes, in this case, likely containing endpoint constants.
using Microsoft.AspNetCore; // Provides the basic structure for hosting ASP.NET Core applications.
using Microsoft.AspNetCore.Hosting; // Used to build and configure the web host.
using Microsoft.Extensions.Hosting; // Provides hosting infrastructure for the application.

namespace Broker
{
    // This is the main entry point of the Broker application.
    public class Program
    {
        public static void Main(string[] args)
        {
            // Enables HTTP/2 protocol support without encryption (HTTP2 unencrypted connections support).
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            // Creates a default web host builder with the specified settings.
            WebHost.CreateDefaultBuilder(args)

                // Specifies the Startup class where services and middleware pipeline are configured.
                .UseStartup<Startup>()

                // Specifies the URL on which the broker service will be hosted.
                // `EndpointsConstants.BrokerAddress` contains the address for the broker, defined elsewhere.
                .UseUrls(EndpointsConstants.BrokerAddress)

                // Builds the web host with the configurations provided.
                .Build()

                // Runs the application by starting the web server and keeping it alive.
                .Run();
        }
    }
}
