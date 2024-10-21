using Broker.Models;
using Broker.Services.Interfaces;
using System.Collections.Concurrent;

namespace Broker.Services
{
    // This service stores and manages the messages in a concurrent queue
    class MessageStorageService : IMessageStorageService
    {
        private readonly ConcurrentQueue<Message> _messages; // Thread-safe queue for messages

        // Constructor initializes the message queue
        public MessageStorageService()
        {
            _messages = new ConcurrentQueue<Message>();
        }

        // Adds a new message to the queue
        public void Add(Message message)
        {
            _messages.Enqueue(message); // Add message to the queue
        }

        // Retrieves and removes the next message from the queue
        public Message GetNext()
        {
            Message message;
            _messages.TryDequeue(out message); // Remove and return the next message
            return message; // Return the dequeued message or null if the queue is empty
        }

        // Check if the queue is empty
        public bool IsEmpty()
        {
            return _messages.IsEmpty; // Returns true if the queue is empty
        }
    }
}
