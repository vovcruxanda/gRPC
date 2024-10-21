using System; // Importing the System namespace which includes fundamental classes and base classes that define commonly used values and reference data types.
using System.Collections.Generic; // Importing the System.Collections.Generic namespace which provides classes for defining generic collections.
using System.IO; // Importing the System.IO namespace which provides functionality for input and output operations on data streams and files.
using System.Linq; // Importing the System.Linq namespace which provides classes and interfaces that support queries that use Language Integrated Query (LINQ).
using System.Threading.Tasks; // Importing the System.Threading.Tasks namespace which provides types that simplify the work of writing concurrent and asynchronous code.
using Common; // Importing the Common namespace which likely contains shared constants and definitions.
using Grpc.Net.Client; // Importing the Grpc.Net.Client namespace to use gRPC client functionalities for making RPC calls.
using GrpcAgent; // Importing the GrpcAgent namespace where the gRPC service definitions (generated from .proto files) are located.
using Microsoft.AspNetCore; // Importing the Microsoft.AspNetCore namespace which provides classes and interfaces for building web applications.
using Microsoft.AspNetCore.Hosting; // Importing the Microsoft.AspNetCore.Hosting namespace which contains types for hosting web applications.
using Microsoft.AspNetCore.Hosting.Server.Features; // Importing the Microsoft.AspNetCore.Hosting.Server.Features namespace which provides interfaces for server features.
using Microsoft.Extensions.Hosting; // Importing the Microsoft.Extensions.Hosting namespace which provides hosting capabilities for applications.

namespace Receiver // Defining a namespace called Receiver to group related classes for the receiver service.
{
    public class Program // Defining the Program class which contains the entry point for the application.
    {
        public static void Main(string[] args) // Main method where the application starts executing.
        {
            // Enable unencrypted HTTP/2 support in the HTTP client handler.
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            // Create and configure a web host builder with default settings and the Startup class.
            var host = WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>() // Specify the Startup class to configure services and the app's request pipeline.
                .UseUrls(EndpointsConstants.SubscriberAddres) // Set the URL for the subscriber service using a constant address.
                .Build(); // Build the web host.

            host.Start(); // Start the web host to listen for incoming requests.

            Subscribe(host); // Call the Subscribe method to subscribe to the broker for receiving messages.

            Console.WriteLine("Press enter to exit"); // Prompt the user to press Enter to terminate the application.
            Console.ReadLine(); // Wait for the user to press Enter.
        }

        private static void Subscribe(IWebHost host) // Private method to handle subscribing to the broker service.
        {
            // Create a gRPC channel for the broker address defined in the constants.
            var channel = GrpcChannel.ForAddress(EndpointsConstants.BrokerAddress);
            var client = new Subscriber.SubscriberClient(channel); // Instantiate the gRPC client for the Subscriber service.

            // Prompt the user to enter a topic to subscribe to.
            Console.Write("Enter the topic: ");
            var topic = Console.ReadLine().ToLower(); // Read and convert the input topic to lowercase.

            // Retrieve the address where the subscriber is listening from the host features.
            var address = host.ServerFeatures.Get<IServerAddressesFeature>().Addresses.First();
            Console.WriteLine($"Subscriber listening at: {address}"); // Log the address where the subscriber is listening.

            // Create a request object with the topic and address to subscribe to the broker.
            var request = new SubscriberRequest() { Topic = topic, Address = address };

            try
            {
                // Call the Subscribe method on the gRPC client and receive a reply.
                var reply = client.Subscribe(request);
                Console.WriteLine($"Subscriber reply: {reply.IsSuccess}"); // Log the success status of the subscription.
            }
            catch (Exception e) // Catch any exceptions that occur during the subscription process.
            {
                Console.WriteLine($"Error subscribing: {e.Message}"); // Log the error message if subscription fails.
            }
        }
    }
}
