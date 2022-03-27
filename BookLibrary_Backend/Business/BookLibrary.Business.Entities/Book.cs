using Core.Common.Interfaces.Entities;

namespace BookLibrary.Business.Entities
{
    public class Book : IIdentifiableEntity
    {
        public int Id { get; set; }
        public int BookCategoryId { get; set; }
        public string Isbn { get; set; }
        public string Title { get; set; }
        public string CoverLink { get; set; }
        
        public virtual BookCategory BookCategory { get; set; }
        public virtual BookCopy BookCopy { get; set; }

        public int EntityId
        {
            get => Id;
            set => Id = value;
        }
    }
}
