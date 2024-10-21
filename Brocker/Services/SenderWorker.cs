using Broker.Services.Interfaces;
using Grpc.Core;
using GrpcAgent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Broker.Services
{
    // This class is a background worker that sends messages to subscribers
    public class SenderWorker : IHostedService
    {
        private Timer _timer; // Timer to trigger message sending at intervals
        private const int TimeToWait = 2000; // Interval between checks (2 seconds)
        private readonly IMessageStorageService _messageStorage;
        private readonly IConnectionStorageService _connectionStorage;

        // Constructor that uses the service scope factory to resolve dependencies
        public SenderWorker(IServiceScopeFactory serviceScopeFactory)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                _messageStorage = scope.ServiceProvider.GetRequiredService<IMessageStorageService>();
                _connectionStorage = scope.ServiceProvider.GetRequiredService<IConnectionStorageService>();
            }
        }

        // Start the worker, setting up the timer to trigger work
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoSendWork, null, 0, TimeToWait); // Start the timer
            return Task.CompletedTask;
        }

        // Stop the worker and dispose of the timer
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0); // Stop the timer
            return Task.CompletedTask;
        }

        // This method is called periodically to send messages to connected clients
        private void DoSendWork(object state)
        {
            while (!_messageStorage.IsEmpty()) // While there are messages to process
            {
                var message = _messageStorage.GetNext(); // Get the next message

                if (message != null)
                {
                    var connections = _connectionStorage.GetConnectionsByTopic(message.Topic); // Get subscribers for this topic

                    foreach (var connection in connections) // Notify each subscriber
                    {
                        var client = new Notifier.NotifierClient(connection.Channel); // Create gRPC client
                        var request = new NotifyRequest() { Content = message.Content };

                        try
                        {
                            var reply = client.Notify(request); // Send the notification
                            Console.WriteLine($"Notified subscriber {connection.Address} with {message.Content}. Response: {reply.IsSuccess}");
                        }
                        catch (RpcException rpcException)
                        {
                            // Handle internal gRPC errors and remove failing connections
                            if (rpcException.StatusCode == StatusCode.Internal)
                            {
                                _connectionStorage.Remove(connection.Address);
                            }

                            Console.WriteLine($"RPC Error notifying subscriber {connection.Address}. {rpcException.Message}");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Error notifying subscriber {connection.Address}. {e.Message}");
                        }
                    }
                }
            }
        }
    }
}
