using System.Runtime.Serialization;

namespace BookLibrary.Business.Contracts.DataContracts
{
    /// <summary>
    /// Return common book library description. 
    /// </summary>
    [DataContract]
    public class LibraryBookData
    {
        [DataMember] 
        public string Isbn { get; set; }

        [DataMember] 
        public string Title { get; set; }

        [DataMember] 
        public string Category { get; set; }

        [DataMember] 
        public string CoverLink { get; set; }

        [DataMember] 
        public bool IsAvailable { get; set; }
    }
}
