using Grpc.Net.Client;
using System;

namespace Broker.Models
{
    // This class represents a connection between the broker and a subscriber
    public class Connection
    {
        // Constructor to initialize a connection with address and topic
        public Connection(string address, string topic)
        {
            // Set the address and topic, and create a new gRPC channel for communication
            Address = address; // Address of the client/subscriber
            Topic = topic; // The topic of messages the client is interested in
            Channel = GrpcChannel.ForAddress(address); // gRPC channel to communicate with the client
        }

        // Read-only properties for the address, topic, and channel
        public string Address { get; }
        public string Topic { get; }
        public GrpcChannel Channel { get; } // gRPC channel used for notifications
    }
}
