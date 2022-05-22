using Core.Common.Interfaces.Entities;

namespace BookLibrary.Business.Entities
{
    public class BookCopy : IEntityBase
    {
        public int BookId { get; set; }
        public int TotalCopy { get; set; }

        public virtual Book Book { get; set; }

        public int EntityId
        {
            get => BookId;
            set => BookId = value;
        }

        public string Version { get; }
        public string ETag { get; }
    }
}
