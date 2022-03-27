using BookLibrary.DataAccess.Interfaces;
using BookLibrary.DataAccess.SQLite;
using BookLibrary.DataAccess.SQLite.Repositories;
using Core.Common.Interfaces.Data;
using DryIoc;

namespace BookLibrary.Business.Bootstrapper
{
    /// <summary>
    /// This is loader for IoC Container and registering all defined DI's.
    /// </summary>
    public static class Bootstrapper
    {
        public static Container Bootstrap()
        {
            var builder = new Container();

            builder.Register<IRepositoryFactory, RepositoryFactory>();

            builder.Register<IBookRepository, BookRepository>();
            builder.Register<IBookCopyRepository, BookCopyRepository>();
            builder.Register<IBookCategoryRepository, BookCategoryRepository>();
            

            return builder;
        }
    }
}
