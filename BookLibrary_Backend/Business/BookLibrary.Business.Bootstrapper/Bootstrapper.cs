using BookLibrary.DataAccess.Interfaces;
using BookLibrary.DataAccess.SQLite.Repositories;
using DryIoc;

namespace BookLibrary.Business.Bootstrapper
{
    public static class Bootstrapper
    {
        public static Container Bootstrap()
        {
            var builder = new Container();

            builder.Register<IBookRepository, BookRepository>();

            return builder;
        }
    }
}
