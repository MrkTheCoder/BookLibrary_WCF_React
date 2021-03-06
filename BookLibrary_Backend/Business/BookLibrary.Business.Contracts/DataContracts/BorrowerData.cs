using System;
using System.Data;
using System.Runtime.Serialization;

namespace BookLibrary.Business.Contracts.DataContracts
{
    /// <summary>
    /// Borrower data for List and Detail page.
    /// </summary>
    [DataContract]
    public class BorrowerData
    {
        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string MiddleName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string Gender { get; set; }

        [DataMember]
        public string ImageLink { get; set; }

        [DataMember]
        public int TotalBorrows { get; set; }

        [DataMember] 
        public string Email { get; set; }

        [DataMember] 
        public string PhoneNo { get; set; }
        
        [DataMember]
        public string Username { get; set; }

        [DataMember] 
        public DateTime RegistrationDate { get; set; }

    }
}
