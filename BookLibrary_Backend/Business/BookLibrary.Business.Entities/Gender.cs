using System.Collections.Generic;
using Core.Common.Interfaces.Entities;

namespace BookLibrary.Business.Entities
{
    public class Gender : IIdentifiableEntity
    {
        public Gender()
        {
            Borrowers = new HashSet<Borrower>();
        }

        public int Id { get; set; }
        public string Type { get; set; }

        public virtual ICollection<Borrower> Borrowers { get; set; }

        public int EntityId
        {
            get => Id;
            set => Id = value;
        }
    }
}
