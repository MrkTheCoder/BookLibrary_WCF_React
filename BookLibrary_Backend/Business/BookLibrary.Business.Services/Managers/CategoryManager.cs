using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Threading.Tasks;
using BookLibrary.Business.Contracts.DataContracts;
using BookLibrary.Business.Contracts.ServiceContracts;
using BookLibrary.Business.Entities;
using BookLibrary.DataAccess.Interfaces;
using Core.Common.Interfaces.Data;

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
        public CategoryManager(IRepositoryFactory repositoryFactory) : base (repositoryFactory)
        { }
        
        
        public async Task<BookCategoryData[]> GetCategoriesAsync(int page, int item)
        {
            if (page < 0 || item < 0)
                throw new ArgumentException("Page & Item arguments must be zero or a positive number");

            var bookCategoryDataItems = new List<BookCategoryData>();

            var bookCategoryRepository = RepositoryFactory.GetEntityRepository<IBookCategoryRepository>();
            var bookCategories =  await bookCategoryRepository.GetAllAsync();

            var bookCategoryCount = bookCategories.Count();
            var currentPages = ValidatePage(page);
            var currentItems = ValidateItemPerPage(page, item, bookCategoryCount);
            
            SetHeaders(bookCategoryCount, currentPages, currentItems);

            bookCategories = bookCategories
                .Skip(currentItems * (currentPages - 1))
                .Take(currentItems);

            MapBookCategoriesToBookCategoryData(bookCategories, bookCategoryDataItems);

            return bookCategoryDataItems.ToArray();
        }

        private void MapBookCategoriesToBookCategoryData(IEnumerable<BookCategory> bookCategories, List<BookCategoryData> bookCategoryDataItems)
        {
            bookCategoryDataItems.Clear();

            foreach (var bookCategory in bookCategories)
            {
                bookCategoryDataItems.Add(new BookCategoryData
                {
                     Name = bookCategory.Name,
                     BooksInCategory = bookCategory.Books.Count
                });
            }
        }


        private int ValidateItemPerPage(int page, int item, int itemCounts)
        {
            var currentItems = AcceptedItemsPerPage.Contains(item)
                ? item
                : page == 0 && item == 0
                    ? itemCounts
                    : DefaultItemsPerPage;
            return currentItems;
        }
        private int ValidatePage(int page)
        {
            var currentPages = page > 0
                ? page
                : 1;
            return currentPages;
        }
    }
}