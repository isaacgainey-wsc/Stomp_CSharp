using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using StompProtocol_CSharp.Parse;
using StompProtocol_CSharp.Commands;
using StompProtocol_CSharp.HeaderFrames;
using StompProtocol_CSharp.Headers;

namespace StompProtocol_CSharp
{
    public class StompFrame
    {
        const char _nl = '\n';
        const char end_of_string = '\x00';

        public readonly Command Command;
        public readonly HeaderFrame Headers;
        public readonly string Content;
        private readonly static ParseHeader ph = new ParseHeader();
        public StompFrame(Command Command, HeaderFrame Headers, object Content = null)
        {
            this.Command = Command;
            this.Headers = Headers;
            this.Content = convertContent(Content);
        }
        public StompFrame(Command Command, List<Header> Headers, object Content = null)
        {
            this.Command = Command;
            this.Headers = Headers;
            this.Content = convertContent(Content);
        }
        protected internal string convertContent(object Content)
        {
            return (Content != null) ?
                        (Content.GetType() == typeof(string) || Content.GetType() == typeof(String)) ?
                            Content as string :
                            JsonConvert.SerializeObject(Content) :
                        "";
        }
        public StompFrame(string stream_stomp_input)
        {
            string[] arr = null;
            try
            {
                if (stream_stomp_input == null || stream_stomp_input.Trim().Equals(""))
                    throw new ArgumentNullException("Input is null or empty");
                arr = JsonConvert.DeserializeObject<string[]>(stream_stomp_input)[0].Split(_nl);
            }catch
            {
                throw new ArgumentException("Input was not serialized");
            }

            Command = new ParseCommand().getNewObject(arr[0]);
            Headers = new List<Header>();
            Content = "";

            int x = 0;
            while (++x < arr.Length)
            {
                if (!arr[x].Contains(':'))
                    break;

                string[] head_dp = arr[x].Split(':');
                Header head = null;
                try
                {
                    head = ph.getNewObject(head_dp[0]);
                    head._Setting = head_dp[1];
                }
                catch (InvalidCastException)
                {
                    head = new Header(head_dp[0], head_dp[1]);
                }
                Headers.Add(head);
            }

            while (x < arr.Length)
                Content += arr[x++];
        }
        public new string ToString()
        {
            string frame = $"{Command.CMD}{_nl}";
            bool content_length_header = false;

            foreach (Header head in Headers.Headers)
            {
                frame += $"{head._Property}:{head._Setting}{_nl}";
                if (head.GetType() == typeof(ContentLengthHeader) || head.GetType() == typeof(PreJsonContentLengthHeader))
                    content_length_header = true;
            }

            if (!content_length_header && Content != "")
            {
                ContentLengthHeader clHeader = new ContentLengthHeader(Content);
                frame += $"{clHeader._Property}:{clHeader._Setting}{_nl}";
            }
            frame += $"{_nl}{Content}";

            return JsonConvert.SerializeObject(new string[] { $"{frame}{end_of_string}" });
        }
        private static List<T> cloneCollection<T>(List<T> coll) where T : ICloneable
        {
            return coll.Select(item => (T)item.Clone()).ToList();
        }
    }
}
namespace StompProtocol_CSharp.StompFrames {
    public class ConnectFrame : StompFrame
    {
        public ConnectFrame(int ideal_heartbeat = 10000, int hard_heartbeat = 10000, string host = null)
            : base(new ConnectCommand(), new ConnectHeaderFrame("1.2,1.1,1.0", ideal_heartbeat, hard_heartbeat), null)
        {
        }
    }
    public class SendFrame : StompFrame
    {
        public SendFrame(string mapping, object content = null)
            : base (new SendCommand(), new SendHeaderFrame(mapping, content), content)
        {
        }
    }
    public class SubscribeFrame : StompFrame
    {
        public SubscribeFrame(string sub_number = "sub-0", string mapping = null)
            : base (new SubscribeCommand(), new SubscribeHeaderFrame($"{sub_number}", mapping))
        {
        }
    }
    public class UnsubscribeFrame : StompFrame
    {
        public UnsubscribeFrame(string sub_id)
            : base (new UnsubscribeCommand(), new UnsubscribeHeaderFrame(sub_id)) 
        {
        }
    }
    public class AckFrame : StompFrame
    {
        public AckFrame(string id, string transaction_id = null)
            : base(new AckCommand(), new AckHeaderFrame(id, transaction_id) )
        {
        }
    }
    public class NackFrame : StompFrame
    {
        public NackFrame(string id, string transaction_id = null)
            : base(new NackCommand(), new NackHeaderFrame(id, transaction_id))
        {
        }
    }
    public class BeginFrame : StompFrame
    {
        public BeginFrame(string transaction_id)
            : base(new BeginCommand(), new BeginHeaderFrame(transaction_id)) 
        {
        }
    }
    public class CommitFrame : StompFrame
    {
        public CommitFrame(string transaction_id)
            : base(new CommitCommand(), new CommitHeaderFrame(transaction_id))
        {
        }
    }
    public class AbortFrame : StompFrame
    {
        public AbortFrame(string transaction_id)
            : base(new AbortCommand(), new AbortHeaderFrame(transaction_id))
        {
        }
    }
    public class DisconnectFrame : StompFrame
    {
        public DisconnectFrame(string receipt_id = "0000")
            : base (new DisconnectCommand(), new DisconnectHeaderFrame(receipt_id)) 
        {
        }
    }
    public class MessageFrame : StompFrame
    {
        public  MessageFrame(string dest, string sub_id, string msg_id, ContentTypeHeader.Inputs content_type = ContentTypeHeader.Inputs.TEXT, string content = null)
            : base(new MessageCommand(), ((content != null) ?
                                new MessageHeaderFrame(dest, sub_id, msg_id, content_type, $"{content.Length}") :
                                new MessageHeaderFrame(dest, sub_id, msg_id, content_type)
                             ))
        {
        }
    }
}