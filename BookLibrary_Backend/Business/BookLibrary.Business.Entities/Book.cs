using Core.Common.Interfaces.Entities;

namespace BookLibrary.Business.Entities
{
    public class Book : IIdentifiableEntity
    {
        public int Id { get; set; }
        public string Isbn { get; set; }
        public string Title { get; set; }

        public int EntityId
        {
            get => Id;
            set => Id = value;
        }
    }
}
