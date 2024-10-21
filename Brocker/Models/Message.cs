namespace Broker.Models
{
    // This class represents a message that will be published to subscribers
    public class Message
    {
        // Constructor to initialize a message with a topic and content
        public Message(string topic, string content)
        {
            Topic = topic; // Topic of the message
            Content = content; // Content of the message
        }

        // Read-only properties for the topic and content of the message
        public string Topic { get; }
        public string Content { get; }
    }
}
