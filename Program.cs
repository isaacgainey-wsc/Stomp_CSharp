using System.Collections.Generic;

namespace StompProtocol_CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Header> headers = new List<Header>();

            int x = 0;
            headers.Add(new Headers.IdHeader($"{x++}"));
            headers.Add(new Headers.DestinationHeader("/app/greeting"));
            headers.Add(new Headers.ContentTypeHeader(Headers.ContentTypeHeader.Inputs.TEXT));

            StompFrame msg_frame = new StompFrame(new Commands.SendCommand(), headers, "Bob");
            string frame_str = msg_frame.ToString();
        }
    }
}
