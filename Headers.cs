using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace StompProtocol_CSharp
{
    public class Header : ICloneable
    {
        protected internal string _Property { get; set; }
        protected internal string _Setting { get; set; }
        public string Property { get { return _Property; } }
        public string Setting { get { return _Setting; } }
        public Header(string Property = null, string Setting = null)
        {
            this._Property = Property;
            this._Setting = Setting;
        }

        public string HeaderLine()
        {
            return $"{Property}:{Setting}";
        }
        public object Clone()
        {
            return new Header(_Property, _Setting);
        }
    }
}

namespace StompProtocol_CSharp.Headers{
    
    public class AcceptableVersionsHeader : Header
    {
        public AcceptableVersionsHeader() : this(null) { }
        public AcceptableVersionsHeader(string versions)
            : base("accept-version", versions)
        {
        }
        public AcceptableVersionsHeader(VersionHeader.Versions v = VersionHeader.Versions.OneDotTwo)
            : this(VersionHeader.versions[(int)v])
        {
        }
    }
    public class AckHeader : Header
    {
        protected internal static readonly string[] _values
            = new string[] { "auto", "client", "client-individual" };
        public enum Values
        {
            Auto,
            Client,
            ClientIndividual
        }
        public AckHeader() : this(null) { }
        public AckHeader(string v)
            : base("ack", v)
        {
        }
        public AckHeader(Values v = Values.Auto)
            : this(_values[(int)v])
        {
        }
    }
    public class ContentLengthHeader : Header
    {
        public ContentLengthHeader() : this(null)
        {
        }
        public ContentLengthHeader(int content_length)
            : base("content-length", $"{content_length}")
        {
        }
        public ContentLengthHeader(object content)
            : this(((content != null) ? content.ToString().Length : 0))
        {

        }
        public ContentLengthHeader(string content)
            : this((content != null)?content.Length:0)
        {
        }
    }
    public class ContentTypeHeader : Header
    {
        public const string
            _default_input = "utf-8";

        protected internal static readonly string[]
            inputs = { "application/json", "application/xml", "text/plain", "text/html" },
            charsets = { "utf-8", "utf-16" };
        public enum Inputs
        {
            JSON,
            XML,
            TEXT,
            HMTL
        }
        public enum CharSets
        {
            UFT_8,
            UFT_16
        }
        public ContentTypeHeader() : this(null) { }
        public ContentTypeHeader(string type, string charset = _default_input)
            : base("content-type", $"{type};charset={charset}")
        {
        }
        public ContentTypeHeader(Inputs input = Inputs.JSON, CharSets charset = CharSets.UFT_8)
            : this(inputs[(int)input], charsets[(int)charset])
        {
        }
    }
    public class DestinationHeader : Header
    {
        public DestinationHeader() : this(null) { }
        public DestinationHeader(string address)
            : base("destination", (address == null || address.ToCharArray()[0] == '/') ?
                                    address :
                                    $"/{address}")
        {
        }
    }
    public class HeartBeatHeader : Header
    {
        public HeartBeatHeader() : this(null) { }
        public HeartBeatHeader(int ideal_heartbeat, int hard_heartbeat)
            : this($"{ideal_heartbeat},{hard_heartbeat}")
        {
        }
        public HeartBeatHeader(string server_heartbeat)
            : base("heart-beat", server_heartbeat)
        {
        }
    }
    public class HostHeader : Header
    {
        public HostHeader() : this(null) { }
        public HostHeader(string host) 
            : base("host", (host != null)? host.Clone() as string: null)
        {
        }
    }
    public class IdHeader : Header
    {
        public IdHeader() : this(null) { }
        public IdHeader(string id_given)
            : base("id", id_given)
        {
        }
    }
    public class LoginHeader : Header
    {
        public LoginHeader() : this(null) { }
        public LoginHeader(string username)
            : base("login", username)
        {
        }
    }
    public class MessageHeader : Header
    {
        public MessageHeader() : this(null) { }
        public MessageHeader(string message) : 
            base("message", message)
        {
        }
    }
    public class MessageIdHeader : Header
    {
        public MessageIdHeader() : this(null) { }
        public MessageIdHeader(string id) 
            : base("message-id", id)
        {
        }
    }
    public class PassHeader : Header
    {
        public PassHeader() : this(null) { }
        public PassHeader(string password = null)
            : base("passcode", password)
        {
        }
    }
    public class PreJsonContentLengthHeader : ContentLengthHeader
    {
        public PreJsonContentLengthHeader(object content = null)
            : base((content != null) ?
                        JsonConvert.SerializeObject(content).Length :
                        0)
        {
        }
        public PreJsonContentLengthHeader(int content_length)
            : base(content_length)
        {
        }
    }
    public class ReceiptHeader : Header
    {
        public ReceiptHeader() : this(null) { }
        public ReceiptHeader(string id) :
            base("receipt", id)
        {
        }
    }
    public class ReceiptIdHeader : Header
    {
        public ReceiptIdHeader() : this(null) { }
        public ReceiptIdHeader(string id) :
            base("receipt-id", id)
        {
        }
    }
    public class SessionHeader : Header
    {
        public SessionHeader() : this(null){ }
        public SessionHeader(string session_id)
            : base("session", session_id)
        {

        }
    }
    public class ServerHeader : Header
    {
        public ServerHeader() : this(null) { }
        public ServerHeader(string name = "", string versions = "")
            : base("server", (versions.Length > 0 && versions.ToCharArray()[0] != '/') ?
                  $"{name}/{versions}" :
                  $"{name}{versions}")
        {
        }
    }
    public class SubscribeIdHeader : Header
    {
        public SubscribeIdHeader() : this(null) { }
        public SubscribeIdHeader(string id)
            : base("subscription", id)
        {
        }
    }
    public class TransactionHeader : Header
    {
        public TransactionHeader() : this(null) { }
        public TransactionHeader(string label)
            : base("transaction", label)
        {
        }
    }
    public class VersionHeader : Header
    {
        protected internal static readonly string[]
            versions = { "1.0", "1.1", "1.2" };
        public enum Versions
        {
            [DescriptionAttribute("1.0")]
            OneDotZero,
            [DescriptionAttribute("1.1")]
            OneDotOne,
            [DescriptionAttribute("1.2")]
            OneDotTwo
        }
        public VersionHeader() : this(null) { }
        public VersionHeader(string version)
            : base("version", version)
        {
        }
    }
}
