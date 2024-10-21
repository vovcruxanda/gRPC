using System; // Importing the System namespace, which contains fundamental classes and base classes that define commonly used values and reference data types.
using System.Collections.Generic; // Importing the Collections.Generic namespace, which provides classes for defining generic collections.
using System.Text; // Importing the System.Text namespace, which contains classes representing ASCII, Unicode, UTF-7, UTF-8, and UTF-32 character encoding.

namespace Common // Defining a namespace named 'Common' to group related classes and avoid naming conflicts.
{
    public class EndpointsConstants // Defining a public class named 'EndpointsConstants'.
    {
        // Declaring a constant string that holds the broker's address for communication.
        public const string BrokerAddress = "http://127.0.0.1:5001"; // The address where the broker service is expected to be hosted, using the localhost IP on port 5001.

        // Declaring a constant string that holds the subscriber's address for communication.
        public const string SubscriberAddres = "http://127.0.0.1:0"; // The address where subscribers can connect, using localhost with port 0 indicating any available port.
    }
}
