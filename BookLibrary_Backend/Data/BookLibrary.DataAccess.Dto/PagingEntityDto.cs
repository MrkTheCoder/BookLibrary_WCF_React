using System.Collections.Generic;
using BookLibrary.Business.Entities;
using Core.Common.Interfaces.Entities;

namespace BookLibrary.DataAccess.Dto
{
    public class PagingEntityDto<TEntity> where TEntity : class, IIdentifiableEntity
    {
        public int TotalItems { get; set; }
        public List<TEntity> Entities { get; set; }
    }
}
