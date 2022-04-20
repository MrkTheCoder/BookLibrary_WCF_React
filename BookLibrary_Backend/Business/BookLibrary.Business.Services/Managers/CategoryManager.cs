using BookLibrary.Business.Contracts.DataContracts;
using BookLibrary.Business.Contracts.ServiceContracts;
using BookLibrary.Business.Entities;
using BookLibrary.DataAccess.Interfaces;
using Core.Common.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Threading.Tasks;

namespace BookLibrary.Business.Services.Managers
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
        ConcurrencyMode = ConcurrencyMode.Multiple,
        IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class CategoryManager : ManagerBase, ICategoryService
    {
        public CategoryManager() : base()
        { }
        public CategoryManager(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        { }


        public async Task<BookCategoryData[]> GetCategoriesAsync(int page, int item)
        {
            if (page < 0 || item < 0)
                throw new ArgumentException("Page & Item arguments must be zero or a positive number");

            var bookCategoryRepository = RepositoryFactory.GetEntityRepository<IBookCategoryRepository>();

            var totalItems = await bookCategoryRepository.GetCountAsync();

            InitializePaging(totalItems, page, item);

            var bookCategories = await bookCategoryRepository.GetFilteredCategories(CurrentPage, CurrentItemsPerPage);

            return MapBookCategoriesToBookCategoryData(bookCategories);
        }

        private BookCategoryData[] MapBookCategoriesToBookCategoryData(IEnumerable<BookCategory> bookCategories)
        {
            var bookCategoryDataItems = new List<BookCategoryData>();

            foreach (var bookCategory in bookCategories)
            {
                bookCategoryDataItems.Add(new BookCategoryData
                {
                    Name = bookCategory.Name,
                    BooksInCategory = bookCategory.Books.Count
                });
            }

            return bookCategoryDataItems.ToArray();
        }
    }
}