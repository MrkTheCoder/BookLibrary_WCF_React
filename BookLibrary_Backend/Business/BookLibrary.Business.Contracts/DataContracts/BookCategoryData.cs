using System.Runtime.Serialization;

namespace BookLibrary.Business.Contracts.DataContracts
{
    [DataContract]
    public class BookCategoryData
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember] 
        public int BooksInCategory { get; set; }
    }
}
