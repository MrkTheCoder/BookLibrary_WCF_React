using System.Runtime.Serialization;

namespace BookLibrary.Business.Contracts.DataContracts
{
    [DataContract]
    public class LibraryBookData
    {
        [DataMember] 
        public int Id { get; set; }

        [DataMember] 
        public string Isbn { get; set; }

        [DataMember] 
        public string Title { get; set; }

        [DataMember] 
        public bool IsAvailable { get; set; }
    }
}
