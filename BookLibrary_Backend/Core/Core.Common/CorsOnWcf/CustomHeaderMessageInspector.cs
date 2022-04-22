using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;


namespace Core.Common.CorsOnWcf
{
    /// <summary>
    /// More info: https://enable-cors.org/server_wcf.html
    /// </summary>
    public class CustomHeaderMessageInspector : IDispatchMessageInspector
    {
        readonly Dictionary<string, string> _requiredHeaders;

        public CustomHeaderMessageInspector(Dictionary<string, string> headers)
        {
            _requiredHeaders = headers ?? new Dictionary<string, string>();
        }

        public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel, System.ServiceModel.InstanceContext instanceContext)
        {
            Console.WriteLine();
            Console.WriteLine("<<< Received Request:");
            Console.WriteLine($"URI:\t'{request.Headers.To}'");
            Console.WriteLine($"Query:\t'{request.Headers.To?.Query}'");
            if (request.Properties.Values.Count > 0)
                Console.WriteLine("Properties value:");
            foreach (var value in request.Properties.Values)
            {
                if(value is HttpRequestMessageProperty requestMessage)
                {
                    Console.WriteLine($"\tMethod: '{requestMessage.Method}'");
                    Console.WriteLine("\tHeaders:");
                    foreach (string key in requestMessage.Headers)
                    {
                        Console.WriteLine($"\t\t{key}: {requestMessage.Headers[key]}");
                    }
                }
            }
            return null;
        }

        public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            if (!reply.Properties.ContainsKey("httpResponse") ||
                !(reply.Properties["httpResponse"] is HttpResponseMessageProperty httpHeader))
                return;

            foreach (var item in _requiredHeaders)
            {
                httpHeader.Headers.Add(item.Key, item.Value);
            }

            Console.WriteLine();
            Console.WriteLine(" >>> Sent");
            if (reply.Properties.Values.Count > 0)
                Console.WriteLine("Properties value:");
            foreach (var value in reply.Properties.Values)
            {
                if (value is HttpResponseMessageProperty messageProperty)
                {
                    Console.WriteLine($"\tStatusCode: ({(int)messageProperty.StatusCode}){messageProperty.StatusCode} ");
                    Console.WriteLine("\tHeaders:");
                    foreach (string header in messageProperty.Headers)
                    {
                        Console.WriteLine($"\t\t{header}: {messageProperty.Headers[header]}");
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine(new string('*', 50));
        }
    }
}
