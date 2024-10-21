using Grpc.Core; // Importing the Grpc.Core namespace which provides classes for building gRPC services and clients.
using GrpcAgent; // Importing the GrpcAgent namespace where the gRPC service definitions (generated from .proto files) are located.
using System; // Importing the System namespace which includes fundamental classes and base classes that define commonly used values and reference data types.
using System.Collections.Generic; // Importing the System.Collections.Generic namespace which provides classes for defining generic collections.
using System.Linq; // Importing the System.Linq namespace which provides classes and interfaces that support queries that use Language Integrated Query (LINQ).
using System.Threading.Tasks; // Importing the System.Threading.Tasks namespace which provides types that simplify the work of writing concurrent and asynchronous code.

namespace Receiver.Services // Defining a namespace called Receiver.Services to group related service classes.
{
    // Defining a class NotificationService that inherits from Notifier.NotifierBase.
    public class NotificationService : Notifier.NotifierBase
    {
        // Overriding the Notify method from the base class to implement the notification handling logic.
        public override Task<NotifyReply> Notify(NotifyRequest request, ServerCallContext context)
        {
            // Log the received notification content to the console.
            Console.WriteLine($"Received: {request.Content}");

            // Returning a successful response wrapped in a NotifyReply object.
            return Task.FromResult(new NotifyReply() { IsSuccess = true });
        }
    }
}
