namespace StompProtocol_CSharp
{
    public abstract class Command
    {
        protected internal string _cmd { get; set; }
        public string CMD {
            get { return _cmd; }
        }
        public Command(string Command_Text)
        {
            _cmd = Command_Text;
        }
    }
}
namespace StompProtocol_CSharp.Commands
{
    public class AbortCommand : Command
    {
        public AbortCommand()
            : base("ABORT")
        {
        }
    }
    public class AckCommand : Command
    {
        public AckCommand()
            : base("ACK")
        {
        }
    }
    public class BeginCommand : Command
    {
        public BeginCommand()
            : base("BEGIN")
        {
        }
    }
    public class CommitCommand : Command
    {
        public CommitCommand()
            : base("COMMIT")
        {
        }
    }
    public class ConnectCommand : Command
    {
        public ConnectCommand()
            : base("CONNECT")
        {
        }
    }
    public class ConnectedCommand : Command
    {
        public ConnectedCommand()
            : base("CONNECTED")
        {
        }
    }
    public class DisconnectCommand : Command
    {
        public DisconnectCommand()
            : base("DISCONNECT")
        {
        }
    }
    public class ErrorCommand : Command
    {
        public ErrorCommand()
            : base("ERROR")
        {
        }
    }
    public class MessageCommand : Command
    {
        public MessageCommand()
            : base("MESSAGE")
        {
        }
    }
    public class NackCommand : Command
    {
        public NackCommand()
            : base("NACK")
        {
        }
    }
    public class ReceiptCommand : Command
    {
        public ReceiptCommand()
            : base("RECEIPT")
        {
        }
    }
    public class SendCommand : Command
    {
        public SendCommand()
            : base("SEND")
        {
        }
    }
    public class SubscribeCommand : Command
    {
        public SubscribeCommand()
            : base("SUBSCRIBE")
        {
        }
    }
    public class UnsubscribeCommand : Command
    {
        public UnsubscribeCommand()
            : base("UNSUBSCRIBE")
        {
        }
    }
}
