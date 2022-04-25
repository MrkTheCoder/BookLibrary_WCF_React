using System;
using System.Net;
using System.Runtime.Serialization;

namespace BookLibrary.Business.Contracts.DataContracts
{
    /// <summary>
    /// return Exception data to client.
    /// </summary>
    [DataContract]
    public class HttpErrorMessage
    {
        public HttpErrorMessage(Exception ex, HttpStatusCode httpStatusCode)
        {
            Message = ex.Message;
            Exception = ex.GetType().Name;
            HttpStatusCode = httpStatusCode;
        }

        [DataMember]
        public string Message { get; set; }
        
        [DataMember]
        public string Exception { get; set; }
        
        [DataMember]
        public HttpStatusCode HttpStatusCode { get; set; }
    }
}
