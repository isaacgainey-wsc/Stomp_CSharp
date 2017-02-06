using StompProtocol_CSharp.Commands;
using StompProtocol_CSharp.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StompProtocol_CSharp.Parse
{
    public abstract class Parser<T>
    { 
        Dictionary<string, Type> library = new Dictionary<string, Type>();

        public Parser()
        {
            AddDefaults();
        }
        abstract protected internal void AddDefaults();
        abstract protected internal string identifer(T obj);
        public Parser<T> AddClass(T obj)
        {
            if ( ! library.ContainsKey(identifer(obj)))
                library[identifer(obj)] = obj.GetType();
            return this;
        }
        public Parser<T> ReplaceClass(T obj)
        {
            library[identifer(obj)] = obj.GetType();
            return this;
        }
        public Type getType(string Label)
        {
            return library[Label];
        }
        public T getNewObject(string Label)
        {
            if (library.ContainsKey(Label))
            {
                return (T)Activator.CreateInstance(library[Label]);
            }else
            {
                throw new InvalidCastException($"{Label} is not a known type in the Parse library");
            }
        }
    }
    public class ParseHeader : Parser<Header>
    {
        protected internal override void AddDefaults()
        {
            AddClass(new AcceptableVersionsHeader());
            AddClass(new AckHeader());
            AddClass(new ContentLengthHeader());
            AddClass(new ContentTypeHeader());
            AddClass(new DestinationHeader());
            AddClass(new HeartBeatHeader());
            AddClass(new HostHeader());
            AddClass(new IdHeader());
            AddClass(new LoginHeader());
            AddClass(new PassHeader());
            AddClass(new ReceiptHeader());
            AddClass(new ReceiptIdHeader());
            AddClass(new ServerHeader());
            AddClass(new SessionHeader());
            AddClass(new SubscribeIdHeader());
            AddClass(new TransactionHeader());
            AddClass(new MessageHeader());
            AddClass(new MessageIdHeader());
            AddClass(new VersionHeader());
        }
        protected internal override string identifer(Header obj)
        {
            return obj._Property;
        }
    }
    public class ParseCommand : Parser<Command>
    {
        protected internal override void AddDefaults()
        {
            AddClass(new AbortCommand());
            AddClass(new AckCommand());
            AddClass(new BeginCommand());
            AddClass(new CommitCommand());
            AddClass(new ConnectCommand());
            AddClass(new ConnectedCommand());
            AddClass(new DisconnectCommand());
            AddClass(new ErrorCommand());
            AddClass(new MessageCommand());
            AddClass(new NackCommand());
            AddClass(new ReceiptCommand());
            AddClass(new SendCommand());
            AddClass(new SubscribeCommand());
            AddClass(new UnsubscribeCommand());
        }
        protected internal override string identifer(Command obj)
        {
            return obj.CMD;
        }
    }
}
