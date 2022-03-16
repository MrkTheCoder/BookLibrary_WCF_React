using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Core.Common.Exceptions;

namespace BookLibrary.Business.Services.Behaviors
{
    [AttributeUsage(AttributeTargets.Class)]
    public class OperationFaultHandlingAttribute : Attribute, IErrorHandler, IServiceBehavior
    {
        // IErrorHandler members:
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            switch (error)
            {
                case NotFoundException _:
                {
                    var faultException = new
                        FaultException<NotFoundException>(new NotFoundException(error.Message));

                    fault = Message.CreateMessage(version,
                        faultException.CreateMessageFault(),
                        faultException.Action);
                    break;
                }
                // General
                default:
                {
                    var faultException = new FaultException(error.Message);

                    fault = Message.CreateMessage(version,
                        faultException.CreateMessageFault(),
                        faultException.Action);
                    break;
                }
            }
        }

        public bool HandleError(Exception error)
        {
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
