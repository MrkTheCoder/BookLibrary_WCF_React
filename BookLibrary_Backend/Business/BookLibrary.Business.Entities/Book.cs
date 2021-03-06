using Core.Common.Interfaces.Entities;

namespace BookLibrary.Business.Entities
{
    public class Book : IEntityBase
    {
        public int Id { get; set; }
        public int BookCategoryId { get; set; }
        public string Isbn { get; set; }
        public string Title { get; set; }
        public string CoverLinkOriginal { get; set; }
        public string CoverLinkThumbnail { get; set; }
        public long RowVersion { get; set; }

        public virtual BookCategory BookCategory { get; set; }
        public virtual BookCopy BookCopy { get; set; }

        public int EntityId
        {
            get => Id;
            set => Id = value;
        }

        public string Version => RowVersion.ToString();
        public string ETag => Isbn + Version;
    }
}
