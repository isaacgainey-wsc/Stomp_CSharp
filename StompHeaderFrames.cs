using StompProtocol_CSharp.Headers;
using System.Collections.Generic;
using System.Linq;

namespace StompProtocol_CSharp
{
    public class HeaderFrame
    {
        protected internal Dictionary<string, Header> headers = new Dictionary<string, Header>();
        public List<Header> Headers
        {
            get
            {
                return new List<Header>(headers.Values);
            }
        }
        public HeaderFrame Add(Header header)
        {
            ReplaceRequiredHeader(header);
            return this;
        }
        public bool ReplaceRequiredHeader(string property, string setting)
        {
            return ReplaceRequiredHeader(new Header(property, setting));
        }
        public bool ReplaceRequiredHeader(Header header)
        {
            bool contains = headers.ContainsKey(header._Property);
            headers[header._Property] = header;
            return contains;
        }
        public bool AddRequiredHeader(string property, string setting)
        {
            return AddRequiredHeader(new Header(property, setting));
        }
        public bool AddRequiredHeader(Header header)
        {
            bool contains = headers.ContainsKey(header._Property);
            if (!contains)
                headers.Add(header._Property, header);
            return !contains;
        }
        public bool AddOptionalHeader(string property, string setting = null)
        {
            return AddOptionalHeader(new Header(property, setting));
        }
        public bool AddOptionalHeader(Header header)
        {
            if (header._Property == null || header._Setting == null)
                return false;
            return AddRequiredHeader(header);
        }
        public bool ReplaceOptionalHeader(string property, string setting = null)
        {
            return ReplaceOptionalHeader(new Header(property, setting));
        }
        public bool ReplaceOptionalHeader(Header header)
        {
            if (header._Property == null || header._Setting == null)
                return false;
            return ReplaceRequiredHeader(header);
        }
        public Header GetHeaderFromProperty(string Property)
        {
            return (headers.ContainsKey(Property))?headers[Property]:null;
        }
        public Header GetHeaderFromSetting(string Setting)
        {
            return headers.Values.First((item) => (item.Setting) == Setting);
        }
        public new string ToString()
        {
            return ToString(",");
        }
        public string ToString(string buffer)
        {
            return string.Join(buffer, Headers.ConvertAll(header => header.HeaderLine()));
        }
        public static implicit operator HeaderFrame(List<Header> v)
        {
            HeaderFrame shf = new HeaderFrame();
            shf.headers =
                v.Where( (header) => header.Setting != null )
                 .ToDictionary(
                    (header) => header.Property, 
                    (header) => header)
                 ;
            return shf;
        }
    }
}
namespace StompProtocol_CSharp.HeaderFrames
{
    public class AbortHeaderFrame : HeaderFrame
    {
        public AbortHeaderFrame(string transaction)
        {
            AddRequiredHeader(new TransactionHeader(transaction));
        }
    }
    public class AckHeaderFrame : HeaderFrame
    {
        public AckHeaderFrame(string id, string transaction = null)
        {
            AddRequiredHeader(new IdHeader(id));
            AddOptionalHeader(new TransactionHeader(transaction));
        }
    }
    public class BeginHeaderFrame : HeaderFrame
    {
        public BeginHeaderFrame(string transaction)
        {
            AddRequiredHeader(new TransactionHeader(transaction));
        }
    }
    public class CommitHeaderFrame : HeaderFrame
    {
        public CommitHeaderFrame(string transaction)
        {
            AddRequiredHeader(new TransactionHeader(transaction));
        }
    }
    public class ConnectHeaderFrame : HeaderFrame
    {
        public ConnectHeaderFrame(string accepted_versions = null, int ideal_heartbeat = 10000, int hard_heartbeat = 10000, string host = null, string user = null, string pass = null)
        {
            AddRequiredHeader((accepted_versions != null) ?
                    new AcceptableVersionsHeader(accepted_versions) :
                    new AcceptableVersionsHeader());
            AddRequiredHeader(new HeartBeatHeader(ideal_heartbeat, hard_heartbeat));
            AddOptionalHeader(new HostHeader(host));
            AddOptionalHeader(new LoginHeader(user));
            AddOptionalHeader(new PassHeader(pass));
        }
    }
    public class ConnectedHeaderFrame : HeaderFrame
    {
        public ConnectedHeaderFrame(string version, string session_id = null, string server = null, string heartbeat = null)
        {
            AddRequiredHeader(new AcceptableVersionsHeader(version));
            AddOptionalHeader(new SessionHeader(session_id));
            AddOptionalHeader(new ServerHeader(server));
            AddOptionalHeader(new HeartBeatHeader(heartbeat));
        }
    }
    public class DisconnectHeaderFrame : HeaderFrame
    {
        public DisconnectHeaderFrame(string receipt_id)
        {
            AddRequiredHeader(new ReceiptHeader(receipt_id));
        }
    }
    public class ErrorHeaderFrame : HeaderFrame
    {
        public ErrorHeaderFrame(string message = null)
        {
            AddOptionalHeader(new MessageHeader(message));
        }
    }
    public class MessageHeaderFrame : HeaderFrame
    {

        public MessageHeaderFrame(string destination, string sub_id, string message_id, ContentTypeHeader.Inputs content_type, string content_length = null, string transaction = null)
        {
            AddRequiredHeader(new DestinationHeader(destination));
            AddRequiredHeader(new SubscribeIdHeader(sub_id));
            AddRequiredHeader(new MessageIdHeader(message_id));
            AddOptionalHeader(new ContentTypeHeader(content_type));
            AddOptionalHeader(new ContentLengthHeader(content_length));
            AddOptionalHeader(new TransactionHeader(transaction));
        }

        public MessageHeaderFrame(string destination, string sub_id, string message_id, string content_type = null, string content_length = null, string transaction = null)
        {
            AddRequiredHeader(new DestinationHeader(destination));
            AddRequiredHeader(new SubscribeIdHeader(sub_id));
            AddRequiredHeader(new MessageIdHeader(message_id));
            AddOptionalHeader(new ContentTypeHeader(content_type));
            AddOptionalHeader(new ContentLengthHeader(content_length));
            AddOptionalHeader(new TransactionHeader(transaction));
        }
    }
    public class NackHeaderFrame : HeaderFrame
    {
        public NackHeaderFrame(string id, string transaction = null)
        {
            AddRequiredHeader(new IdHeader(id));
            AddOptionalHeader(new TransactionHeader(transaction));
        }
    }
    public class ReceiptHeaderFrame : HeaderFrame
    {
        public ReceiptHeaderFrame(string receipt_id)
        {
            AddRequiredHeader(new ReceiptIdHeader(receipt_id));
        }
    }
    public class SendHeaderFrame : HeaderFrame
    {
        public SendHeaderFrame(string mapping,
                            object preserialize_content = null,
                            ContentTypeHeader.Inputs content_type = ContentTypeHeader.Inputs.JSON,
                            string transaction_id = null)
        {
            AddRequiredHeader(new DestinationHeader(mapping));
            AddRequiredHeader(new ContentTypeHeader(content_type));
            AddRequiredHeader(
                (content_type == ContentTypeHeader.Inputs.JSON
                && preserialize_content.GetType() != typeof(string))? 
                            new PreJsonContentLengthHeader(preserialize_content): 
                            new ContentLengthHeader(preserialize_content as string));
            AddOptionalHeader(new TransactionHeader(transaction_id));
        }
    }
    public class SubscribeHeaderFrame : HeaderFrame
    {
        public SubscribeHeaderFrame(string id, string mapping, AckHeader.Values value = AckHeader.Values.Auto)
        {
            AddRequiredHeader(new IdHeader(id));
            AddRequiredHeader(new DestinationHeader(mapping));
            if (value != AckHeader.Values.Auto)
                AddRequiredHeader(new AckHeader(value));
        }
    }
    public class UnsubscribeHeaderFrame : HeaderFrame
    {
        public UnsubscribeHeaderFrame(string id)
        {
            AddRequiredHeader(new IdHeader(id));
        }
    }
}
