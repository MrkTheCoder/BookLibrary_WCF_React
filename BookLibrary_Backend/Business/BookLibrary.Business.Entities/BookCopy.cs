using Core.Common.Interfaces.Entities;

namespace BookLibrary.Business.Entities
{
    public class BookCopy : IIdentifiableEntity
    {
        public int BookId { get; set; }
        public int TotalCopy { get; set; }

        public virtual Book Book { get; set; }

        public int EntityId
        {
            get => BookId;
            set => BookId = value;
        }
    }
}
