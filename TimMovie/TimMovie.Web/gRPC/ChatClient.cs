namespace TimMovie.Web.gRPC;

public class ChatClient
{
    public ChatClient(string name)
    {
        Name = name;
        Messages = new Queue<ChatMessage>();
        Events = new Queue<ChatEvent>();
    }

    public string Name { get; }

    public Queue<ChatMessage> Messages { get; }
    
    public Queue<ChatEvent> Events { get; }

    public bool IsAdmin => Name.StartsWith("admin");

    public override bool Equals(object? obj)
    {
        if (obj is not ChatClient other)
            return false;
        return Name == other.Name;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}