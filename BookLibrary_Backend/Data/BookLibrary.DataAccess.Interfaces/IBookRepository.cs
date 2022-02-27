﻿using BookLibrary.Business.Entities;
using Core.Common.Interfaces.Data;

namespace BookLibrary.DataAccess.Interfaces
{
    public interface IBookRepository : IRepositoryBase<Book>
    {
    }
}
