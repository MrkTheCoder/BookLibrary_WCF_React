using BookLibrary.Business.Entities.Extensions;
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
        public byte[] RowVersion { get; set; }

        public virtual BookCategory BookCategory { get; set; }
        public virtual BookCopy BookCopy { get; set; }

        public int EntityId
        {
            get => Id;
            set => Id = value;
        }

        public string Version => RowVersion.ToHexString();
    }
}
