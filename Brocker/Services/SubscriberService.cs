using Broker.Models; // Importing the models related to Broker, like Connection.
using Broker.Services.Interfaces; // Importing the service interfaces, specifically for connection storage.
using Grpc.Core; // Importing the core gRPC classes and functionality.
using GrpcAgent; // Importing the gRPC-generated classes from proto files.
using System; // Importing basic system utilities like Exception.
using System.Collections.Generic; // (Unused here) Could be used for lists or collections.
using System.Linq; // (Unused here) Provides LINQ query support.
using System.Threading.Tasks; // Importing the support for asynchronous operations.

namespace Broker.Services
{
    // This class implements the gRPC service for managing subscribers.
    public class SubscriberService : Subscriber.SubscriberBase
    {
        private readonly IConnectionStorageService _connectionStorageService; // Service to manage connections.

        // Constructor that injects the IConnectionStorageService dependency.
        public SubscriberService(IConnectionStorageService connectionStorageService)
        {
            _connectionStorageService = connectionStorageService;
        }

        // gRPC method to handle subscription requests from clients.
        public override Task<SubscriberReply> Subscribe(SubscriberRequest request, ServerCallContext context)
        {
            // Logging the subscription attempt.
            Console.WriteLine($"New client trying to subscribe: {request.Address} {request.Topic}");

            try
            {
                // Create a new connection object with the client's address and topic.
                var connect = new Connection(request.Address, request.Topic);

                // Try to add the new connection to the storage service.
                _connectionStorageService.Add(connect);
            }
            catch (Exception e)
            {
                // If an exception occurs, log the error with details.
                Console.WriteLine($"Could not add the new connection {request.Address} {request}. {e.Message}");
            }

            // (This code seems redundant since the connection was added above)
            // Adding the connection again outside the try-catch block.
            var connection = new Connection(request.Address, request.Topic);
            _connectionStorageService.Add(connection);

            // Logging the successful subscription.
            Console.WriteLine($"New client subscribed: {request.Address} {request.Topic}");

            // Return a success message to the client as a gRPC reply.
            return Task.FromResult(new SubscriberReply()
            {
                IsSuccess = true // Indicating that the subscription was successful.
            });
        }
    }
}
