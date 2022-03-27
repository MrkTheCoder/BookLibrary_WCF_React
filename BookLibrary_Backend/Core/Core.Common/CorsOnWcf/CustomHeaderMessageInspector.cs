using System.Collections.Generic;
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

        public CustomHeaderMessageInspector (Dictionary<string, string> headers)
        {
            _requiredHeaders = headers ?? new Dictionary<string, string>();
        }

        public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel, System.ServiceModel.InstanceContext instanceContext)
        {
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
        }
    }
}
