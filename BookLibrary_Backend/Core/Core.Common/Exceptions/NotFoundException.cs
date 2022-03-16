using System;
using System.Runtime.Serialization;

namespace Core.Common.Exceptions
{
    [Serializable]
    public class NotFoundException : ApplicationException
    {
        public NotFoundException()
        {}

        public NotFoundException(string message)
            : base(message)
        {}

        public NotFoundException(string message, System.Exception exception)
            : base(message, exception)
        {}

        protected NotFoundException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {}
    }
}
