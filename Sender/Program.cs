using Common; // Importing the Common namespace which contains shared constants and definitions.
using Grpc.Net.Client; // Importing the Grpc.Net.Client namespace which provides a gRPC client for connecting to gRPC services.
using GrpcAgent; // Importing the GrpcAgent namespace which contains generated gRPC service definitions and messages.
using System; // Importing the System namespace which includes fundamental classes and base classes that define commonly used values and reference data types.
using System.Threading.Tasks; // Importing the System.Threading.Tasks namespace which provides types that simplify the work of writing concurrent and asynchronous code.

namespace Sender // Defining a namespace called Sender to group related classes for the publisher service.
{
    class Program // Defining the Program class which contains the entry point of the application.
    {
        static async Task Main(string[] args) // The main method of the application, marked as async to allow asynchronous operations.
        {
            Console.WriteLine("Publisher"); // Outputs a message indicating that the publisher is starting.

            // Enable unencrypted HTTP/2 support. This is needed when connecting to a gRPC server using HTTP/2 without TLS.
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            // Create a gRPC channel to the broker's address specified in the EndpointsConstants.
            var channel = GrpcChannel.ForAddress(EndpointsConstants.BrokerAddress);

            // Create a gRPC client for the Publisher service using the channel created above.
            var client = new Publisher.PublisherClient(channel);

            // Infinite loop to repeatedly prompt the user for input.
            while (true)
            {
                // Prompt the user to enter a topic.
                Console.Write("Enter the topic: ");
                var topic = Console.ReadLine().ToLower(); // Read user input and convert it to lowercase.

                // Prompt the user to enter the content of the message.
                Console.Write("Enter content: ");
                var content = Console.ReadLine(); // Read user input for the message content.

                // Create a new PublisherRequest with the topic and content provided by the user.
                var request = new PublisherRequest() { Topic = topic, Content = content };

                try
                {
                    // Asynchronously call the PublishMessage method on the client with the request created.
                    var reply = await client.PublishMessageAsync(request);
                    // Output the success status of the publish operation.
                    Console.WriteLine($"Publish Reply: {reply.IsSuccess}");
                }
                catch (Exception e) // Catch any exceptions that occur during the publish operation.
                {
                    // Output the error message to the console if publishing fails.
                    Console.WriteLine($"Error publishing the message: {e.Message}");
                }
            }
        }
    }
}
