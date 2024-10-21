using Broker.Models;
using Broker.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Broker.Services
{
    // This service stores and manages active connections from clients/subscribers
    public class ConnectionStorageService : IConnectionStorageService
    {
        private readonly List<Connection> _connections; // A list of active connections
        private readonly object _locker; // Object for thread-safety (locking mechanism)

        // Constructor initializes the list and the locking object
        public ConnectionStorageService()
        {
            _connections = new List<Connection>();
            _locker = new object();
        }

        // Adds a new connection to the storage in a thread-safe manner
        public void Add(Connection connection)
        {
            lock (_locker)
            {
                _connections.Add(connection); // Add new connection to the list
            }
        }

        // Retrieves all connections that match a given topic
        public IList<Connection> GetConnectionsByTopic(string topic)
        {
            lock (_locker)
            {
                // Return only connections that match the topic
                return _connections.Where(x => x.Topic == topic).ToList();
            }
        }

        // Removes a connection by address (e.g., when a client disconnects or fails)
        public void Remove(string address)
        {
            lock (_locker)
            {
                // Remove all connections with the specified address
                _connections.RemoveAll(x => x.Address == address);
            }
        }
    }
}
