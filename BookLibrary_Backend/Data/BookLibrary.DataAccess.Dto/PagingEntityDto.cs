using Core.Common.Interfaces.Entities;
using System.Collections.Generic;

namespace BookLibrary.DataAccess.Dto
{
    public class PagingEntityDto<TEntity> where TEntity : class, IIdentifiableEntity
    {
        public int TotalItems { get; set; }
        public List<TEntity> Entities { get; set; }
    }
}
