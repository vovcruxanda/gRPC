using Broker.Models;
using Broker.Services.Interfaces;
using Grpc.Core;
using GrpcAgent;
using System;
using System.Threading.Tasks;

namespace Broker.Services
{
    // This class implements the Publisher gRPC service, which allows clients to publish messages
    public class PublisherService : Publisher.PublisherBase
    {
        private readonly IMessageStorageService _messageStorage; // Reference to message storage service

        // Constructor that injects the message storage service
        public PublisherService(IMessageStorageService messageStorageService)
        {
            _messageStorage = messageStorageService;
        }

        // Method that handles incoming PublishMessage requests via gRPC
        public override Task<PublisherReply> PublishMessage(PublisherRequest request, ServerCallContext context)
        {
            // Log the received message
            Console.WriteLine($"Received: {request.Topic} {request.Content}");

            // Create a new message and add it to the message storage
            var message = new Message(request.Topic, request.Content);
            _messageStorage.Add(message);

            // Return a success response to the client
            return Task.FromResult(new PublisherReply()
            {
                IsSuccess = true // Indicate success
            });
        }
    }
}
