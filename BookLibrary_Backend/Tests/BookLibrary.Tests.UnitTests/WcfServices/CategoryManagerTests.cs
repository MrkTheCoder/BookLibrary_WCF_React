using BookLibrary.Business.Entities;
using BookLibrary.Business.Services.Managers;
using BookLibrary.DataAccess.Dto;
using BookLibrary.DataAccess.Interfaces;
using Core.Common.Interfaces.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace BookLibrary.Tests.UnitTests.WcfServices
{
    public class CategoryManagerTests
    {
        private readonly Mock<IRepositoryFactory> _moqRepositoryFactory;
        private readonly Mock<IBookCategoryRepository> _moqCategoryRepository;
        private IEnumerable<BookCategory> _fakeDbCategories;

        public CategoryManagerTests()
        {
            _fakeDbCategories = FeedCategories();

            _moqCategoryRepository = new Mock<IBookCategoryRepository>();
            _moqCategoryRepository.Setup(s => s.GetFilteredCategories(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((int page, int item) => 
                    _fakeDbCategories
                        .Skip(item * (page - 1))
                        .Take(item)
                        .ToList());

            _moqCategoryRepository.Setup(s => s.GetCountAsync()).ReturnsAsync(_fakeDbCategories.Count);
            _moqCategoryRepository.Setup(s => s.GetCountAsync(It.IsAny<Expression<Func<BookCategory,bool>>>()))
                .Returns<Expression<Func<BookCategory, bool>>>(f =>
                    Task.FromResult(_fakeDbCategories.AsQueryable().Count(f)));

            _moqRepositoryFactory = new Mock<IRepositoryFactory>();
            _moqRepositoryFactory.Setup(s => s.GetEntityRepository<IBookCategoryRepository>())
                .Returns(_moqCategoryRepository.Object);
        }


        [Fact]
        [Trait(nameof(CategoryManagerTests), "Initialize")]
        public void Initialize_ShouldReturnInstance()
        {
            var categoryManager = new CategoryManager(_moqRepositoryFactory.Object);

            Assert.IsType<CategoryManager>(categoryManager);
        }

        [Fact]
        [Trait(nameof(CategoryManagerTests), nameof(CategoryManager.GetCategoriesAsync))]
        public async Task GetCategories_WithoutParameters_ShouldReturnAllCategories()
        {
            var categoryManager = new CategoryManager(_moqRepositoryFactory.Object);

            var categoryDataItems = await categoryManager.GetCategoriesAsync(0, 0);

            Assert.NotEmpty(categoryDataItems);
            Assert.Equal(_fakeDbCategories.Count(), categoryDataItems.Length);
        }

        [Fact]
        [Trait(nameof(CategoryManagerTests), nameof(CategoryManager.GetCategoriesAsync))]
        public async Task GetCategories_WithoutParameters_ShouldFirstCategoryPlaceAtStart()
        {
            var categoryManager = new CategoryManager(_moqRepositoryFactory.Object);

            var categoryDataItems = await categoryManager.GetCategoriesAsync(0, 0);

            var firstCat = categoryDataItems.First();

            Assert.Equal(1, firstCat.BooksInCategory);
            Assert.Equal(_fakeDbCategories.First().EntityId, firstCat.BooksInCategory);
            Assert.Equal(_fakeDbCategories.First().Books.Count, firstCat.BooksInCategory);
            Assert.Equal("A", firstCat.Name);
            Assert.Equal(_fakeDbCategories.First().Name, firstCat.Name);
        }

        [Fact]
        [Trait(nameof(CategoryManagerTests), nameof(CategoryManager.GetCategoriesAsync))]
        public async Task GetCategories_WithoutParameters_ShouldLastCategoryPlaceAtLast()
        {
            var categoryManager = new CategoryManager(_moqRepositoryFactory.Object);

            var categoryDataItems = await categoryManager.GetCategoriesAsync(0, 0);

            var lastCat = categoryDataItems.Last();

            Assert.Equal(_fakeDbCategories.Count(), lastCat.BooksInCategory);
            Assert.Equal(_fakeDbCategories.Last().EntityId, lastCat.BooksInCategory);
            Assert.Equal(_fakeDbCategories.Last().Books.Count, lastCat.BooksInCategory);
            Assert.Equal(((char)('A' + _fakeDbCategories.Count() - 1)).ToString(), lastCat.Name);
            Assert.Equal(_fakeDbCategories.Last().Name, lastCat.Name);
        }

        [Fact]
        [Trait(nameof(CategoryManagerTests), nameof(CategoryManager.GetCategoriesAsync))]
        public async Task GetCategories_WithWrongValueInArgument_ThrowArgumentException()
        {
            var categoryManager = new CategoryManager(_moqRepositoryFactory.Object);

            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await categoryManager.GetCategoriesAsync(-1, 0));
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await categoryManager.GetCategoriesAsync(0, -1));
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
                await categoryManager.GetCategoriesAsync(-1, -1));

            Assert.Equal("Page & Item arguments must be zero or a positive number", exception.Message);
        }

        [Fact]
        [Trait(nameof(CategoryManagerTests), nameof(CategoryManager.GetCategoriesAsync))]
        public async Task GetCategories_WithoutParametersAndEmptyDb_ShouldReturnEmptyList()
        {
            _fakeDbCategories = new List<BookCategory>();

            var categoryManager = new CategoryManager(_moqRepositoryFactory.Object);

            var categoryDataItems = await categoryManager.GetCategoriesAsync(0, 0);

            Assert.Empty(categoryDataItems);
        }

        [Fact]
        [Trait(nameof(CategoryManagerTests), nameof(CategoryManager.GetCategoriesAsync))]
        public async Task GetCategories_PageOneAndEmptyDb_ShouldReturnEmptyList()
        {
            _fakeDbCategories = new List<BookCategory>();

            var categoryManager = new CategoryManager(_moqRepositoryFactory.Object);

            var categoryDataItems = await categoryManager.GetCategoriesAsync(1, 0);

            Assert.Empty(categoryDataItems);
        }

        [Fact]
        [Trait(nameof(CategoryManagerTests), nameof(CategoryManager.GetCategoriesAsync))]
        public async Task GetCategories_PageOne_ShouldReturnTenCategoriesAndLastItemBe10th()
        {
            var categoryManager = new CategoryManager(_moqRepositoryFactory.Object);

            var categoryDataItems = await categoryManager.GetCategoriesAsync(1, 0);

            Assert.NotEmpty(categoryDataItems);
            Assert.Equal(10, categoryDataItems.Length);

            var lastBookInList = categoryDataItems.Last();
            var bookCategory = _fakeDbCategories.ToArray()[10 - 1];

            Assert.Equal(bookCategory.Name, lastBookInList.Name);
        }

        [Fact]
        [Trait(nameof(CategoryManagerTests), nameof(CategoryManager.GetCategoriesAsync))]
        public async Task GetCategories_ItemTwenty_ShouldReturnTwentyCategoriesAndLastItemBe20th()
        {
            var categoryManager = new CategoryManager(_moqRepositoryFactory.Object);

            var categoryDataItems = await categoryManager.GetCategoriesAsync(0, 20);

            Assert.NotEmpty(categoryDataItems);
            Assert.Equal(20, categoryDataItems.Length);

            var lastBookInList = categoryDataItems.Last();
            var bookCategory = _fakeDbCategories.ToArray()[20 - 1];

            Assert.Equal(bookCategory.Name, lastBookInList.Name);
        }

        [Theory]
        [MemberData(nameof(DifferentArguments))]
        [Trait(nameof(CategoryManagerTests), nameof(CategoryManager.GetCategoriesAsync))]
        public async Task GetCategories_DifferentObjects_ShouldReturnExpectedItems(int page, int item, 
            int expectedReturnedItems, int expectedPages, int expectedItemsPP)
        {
            var categoryManager = new CategoryManager(_moqRepositoryFactory.Object);

            var categoryDataItems = await categoryManager.GetCategoriesAsync(page, item);

            Assert.Equal(expectedReturnedItems, categoryDataItems.Length);
            Assert.Equal(expectedPages, categoryManager.CurrentPage);
            Assert.Equal(expectedItemsPP, categoryManager.CurrentItemsPerPage);
        }

        public static IEnumerable<object[]> DifferentArguments()
        {
            yield return new object[] { 0, 0, 21, 1, 21 };
            yield return new object[] { 0, 1, 10, 1, 10 };
            yield return new object[] { 0, 10, 10, 1, 10 };
            yield return new object[] { 0, 15, 10, 1, 10 };
            yield return new object[] { 0, 20, 20, 1 ,20 };
            yield return new object[] { 0, 25, 10, 1, 10 };
            yield return new object[] { 0, 30, 21, 1, 30 };
            yield return new object[] { 1, 0, 10, 1, 10};
            yield return new object[] { 1, 20, 20, 1, 20 };
            yield return new object[] { 1, 30, 21, 1, 30 };
            yield return new object[] { 2, 0, 10, 2, 10 };
            yield return new object[] { 2, 1, 10, 2, 10 };
            yield return new object[] { 2, 20, 1, 2, 20 };
            yield return new object[] { 2, 30, 21 , 1, 30};
            yield return new object[] { 3, 0, 1, 3, 10 };
            yield return new object[] { 3, 1, 1, 3, 10 };
            yield return new object[] { 3, 20, 1, 2, 20 };
        }

        private IEnumerable<BookCategory> FeedCategories()
        {
            var cats = new List<BookCategory>();
            for (int i = 1; i <= 21; i++)
            {
                cats.Add(new BookCategory
                {
                    Id = i,
                    Name = ((char)('A' + i - 1)).ToString(),
                    Books = new List<Book>(new Book[i])
                });
            }

            return cats;
        }
    }
}
