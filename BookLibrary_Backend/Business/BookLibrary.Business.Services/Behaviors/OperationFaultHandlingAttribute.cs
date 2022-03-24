using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using BookLibrary.Business.Contracts.DataContracts;
using Core.Common.Exceptions;

namespace BookLibrary.Business.Services.Behaviors
{
    /// <summary>
    /// A costume attribute for Services to handel:
    /// * Exceptions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class OperationFaultHandlingAttribute : Attribute, IErrorHandler, IServiceBehavior
    {
        // IErrorHandler members:
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            var errorCode = HttpStatusCode.BadRequest;
            var errorResponseMessage = "Unknown Exception";

            switch (error)
            {
                case NotFoundException _:
                    {
                        errorCode = HttpStatusCode.NotFound;
                        errorResponseMessage = "Not Found!";
                        break;
                    }
                case FormatException _:
                    {
                        errorCode = HttpStatusCode.BadRequest;
                        errorResponseMessage = "Check Arguments Format!";
                        break;
                    }
                // General
                default:
                    {
                        break;
                    }

            }

            fault = Message
                .CreateMessage(version, "",
                    new HttpErrorMessage(error, errorCode),
                    new DataContractJsonSerializer(typeof(HttpErrorMessage)));
            var wbf = new WebBodyFormatMessageProperty(WebContentFormat.Json);
            fault.Properties.Add(WebBodyFormatMessageProperty.Name, wbf);

            if (WebOperationContext.Current != null)
            {
                var response = WebOperationContext.Current.OutgoingResponse;
                response.ContentType = "application/json";
                response.StatusCode = errorCode;
                response.StatusDescription = errorResponseMessage;
            }

        }

        public bool HandleError(Exception error)
        {
            //TODO: log stacktrace
            return true; // Place for auditing
        }


        // IServiceBehavior members:
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            // throw new NotImplementedException();
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints,
            BindingParameterCollection bindingParameters)
        {
            // throw new NotImplementedException();
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (var channelDispatcherBase in serviceHostBase.ChannelDispatchers)
            {
                var channelDispatcher = (ChannelDispatcher)channelDispatcherBase;
                channelDispatcher?.ErrorHandlers.Add(this);
            }
        }
    }
}
