using System;
using Core.Common.Interfaces.Entities;

namespace BookLibrary.Business.Entities
{
    public class Borrower : IIdentifiableEntity
    {
        public int Id { get; set; }
        public int GenderId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PhoneNo { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string AvatarLink { get; set; }
        public DateTime RegistrationDate { get; set; }

        public virtual Gender Gender { get; set; }

        public int EntityId
        {
            get => Id;
            set => Id = value;
        }
    }
}
