using System.Collections.Generic;
using Core.Common.Interfaces.Entities;

namespace BookLibrary.Business.Entities
{
    public class BookCategory : IEntityBase
    {
        public BookCategory()
        {
            Books = new HashSet<Book>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public long RowVersion { get; set; }

        public virtual ICollection<Book> Books { get; set; }
        
        public int EntityId
        {
            get => Id;
            set => Id = value;
        }

        public string Version => RowVersion.ToString();
        public string ETag => Name + Version;
    }
}
