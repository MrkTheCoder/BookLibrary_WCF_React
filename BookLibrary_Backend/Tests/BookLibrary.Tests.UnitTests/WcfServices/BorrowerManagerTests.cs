using BookLibrary.Business.Entities;
using BookLibrary.Business.Services.Managers;
using BookLibrary.DataAccess.Dto;
using BookLibrary.DataAccess.Interfaces;
using Core.Common.Exceptions;
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
    public class BorrowerManagerTests
    {
        private readonly Mock<IRepositoryFactory> _moqRepositoryFactory;
        private readonly Mock<IBorrowerRepository> _moqBorrowerRepository;
        private IEnumerable<Borrower> _fakeDbBorrowers;
        private IEnumerable<Gender> _fakeDatabaseGenders;

        public BorrowerManagerTests()
        {
            _fakeDbBorrowers = FeedCategories();

            _moqBorrowerRepository = new Mock<IBorrowerRepository>();
            _moqBorrowerRepository.Setup(s => s.GetFilteredBorrowersAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((int page, int item) => 
                    _fakeDbBorrowers
                    .Skip(item * (page - 1))
                    .Take(item)
                    .ToList());
            _moqBorrowerRepository.Setup(s =>
                    s.GetByExpressionAsync(It.IsAny<Expression<Func<Borrower, bool>>>()))
                .Returns<Expression<Func<Borrower, bool>>>(p =>
                    Task.FromResult(_fakeDbBorrowers.AsQueryable().FirstOrDefault(p)));

            _moqBorrowerRepository.Setup(s => s.GetCountAsync()).ReturnsAsync(_fakeDbBorrowers.Count);
            _moqBorrowerRepository.Setup(s => s.GetCountAsync(It.IsAny<Expression<Func<Borrower,bool>>>()))
                .Returns<Expression<Func<Borrower, bool>>>(f =>
                    Task.FromResult(_fakeDbBorrowers.AsQueryable().Count(f)));

            _moqRepositoryFactory = new Mock<IRepositoryFactory>();
            _moqRepositoryFactory.Setup(s => s.GetEntityRepository<IBorrowerRepository>())
                .Returns(_moqBorrowerRepository.Object);
        }




        [Fact]
        [Trait(nameof(BorrowerManagerTests), "Initialize")]
        public void Initialize_ShouldReturnInstance()
        {
            var borrowerManager = new BorrowerManager(_moqRepositoryFactory.Object);

            Assert.IsType<BorrowerManager>(borrowerManager);
        }

        [Fact]
        [Trait(nameof(BorrowerManagerTests), nameof(BorrowerManager.GetBorrowersAsync))]
        public async Task GetBorrowers_WithoutParameters_ShouldReturnAllBorrowers()
        {
            var borrowerManager = new BorrowerManager(_moqRepositoryFactory.Object);

            var borrowerDataItems = await borrowerManager.GetBorrowersAsync(0, 0);

            Assert.NotEmpty(borrowerDataItems);
            Assert.Equal(_fakeDbBorrowers.Count(), borrowerDataItems.Length);
        }

        [Fact]
        [Trait(nameof(BorrowerManagerTests), nameof(BorrowerManager.GetBorrowersAsync))]
        public async Task GetBorrowers_WithoutParameters_ShouldFirstBorrowerPlaceAtStart()
        {
            var borrowerManager = new BorrowerManager(_moqRepositoryFactory.Object);

            var borrowerDataItems = await borrowerManager.GetBorrowersAsync(0, 0);

            var firstBorrowerData = borrowerDataItems.First();

            Assert.Equal(_fakeDbBorrowers.First().PhoneNo, firstBorrowerData.PhoneNo);
        }

        [Fact]
        [Trait(nameof(BorrowerManagerTests), nameof(BorrowerManager.GetBorrowersAsync))]
        public async Task GetBorrowers_WithoutParameters_ShouldFirstBorrowerHasCorrectData()
        {
            var borrowerManager = new BorrowerManager(_moqRepositoryFactory.Object);

            var borrowerDataItems = await borrowerManager.GetBorrowersAsync(0, 0);
            var firstBorrowerData = borrowerDataItems.First();
            var borrowerFromDb = _fakeDbBorrowers.First();

            Assert.Equal(borrowerFromDb.PhoneNo, firstBorrowerData.PhoneNo);
            Assert.Equal(borrowerFromDb.AvatarLink, firstBorrowerData.ImageLink);
            Assert.Equal(borrowerFromDb.Email, firstBorrowerData.Email);
            Assert.Equal(borrowerFromDb.FirstName, firstBorrowerData.FirstName);
            Assert.Equal(borrowerFromDb.MiddleName, firstBorrowerData.MiddleName);
            Assert.Equal(borrowerFromDb.LastName, firstBorrowerData.LastName);
            Assert.Equal(borrowerFromDb.Gender.Type, firstBorrowerData.Gender);
            Assert.Equal(borrowerFromDb.GenderId,
                _fakeDatabaseGenders.Single(s => s.Type == firstBorrowerData.Gender).Id);
            Assert.Equal(borrowerFromDb.RegistrationDate, firstBorrowerData.RegistrationDate);
            Assert.Equal(borrowerFromDb.Username, firstBorrowerData.Username);
        }

        [Fact]
        [Trait(nameof(BorrowerManagerTests), nameof(BorrowerManager.GetBorrowersAsync))]
        public async Task GetBorrowers_WithoutParameters_ShouldLastBorrowerPlaceAtLast()
        {
            var borrowerManager = new BorrowerManager(_moqRepositoryFactory.Object);

            var borrowerDataItems = await borrowerManager.GetBorrowersAsync(0, 0);

            var lastUser = borrowerDataItems.Last();

            Assert.Equal(_fakeDbBorrowers.Last().PhoneNo, lastUser.PhoneNo);
        }

        [Fact]
        [Trait(nameof(BorrowerManagerTests), nameof(BorrowerManager.GetBorrowersAsync))]
        public async Task GetBorrowers_WithWrongValueInArgument_ThrowArgumentException()
        {
            var borrowerManager = new BorrowerManager(_moqRepositoryFactory.Object);

            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await borrowerManager.GetBorrowersAsync(-1, 0));
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await borrowerManager.GetBorrowersAsync(0, -1));
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
                await borrowerManager.GetBorrowersAsync(-1, -1));

            Assert.Equal("Page & Item arguments must be zero or a positive number", exception.Message);
        }

        [Fact]
        [Trait(nameof(BorrowerManagerTests), nameof(BorrowerManager.GetBorrowersAsync))]
        public async Task GetBorrowers_WithoutParametersAndEmptyDb_ShouldReturnEmptyList()
        {
            _fakeDbBorrowers = new List<Borrower>();

            var borrowerManager = new BorrowerManager(_moqRepositoryFactory.Object);

            var borrowerDataItems = await borrowerManager.GetBorrowersAsync(0, 0);

            Assert.Empty(borrowerDataItems);
        }

        [Fact]
        [Trait(nameof(BorrowerManagerTests), nameof(BorrowerManager.GetBorrowersAsync))]
        public async Task GetBorrowers_PageOneAndEmptyDb_ShouldReturnEmptyList()
        {
            _fakeDbBorrowers = new List<Borrower>();

            var borrowerManager = new BorrowerManager(_moqRepositoryFactory.Object);

            var borrowerDataItems = await borrowerManager.GetBorrowersAsync(1, 0);

            Assert.Empty(borrowerDataItems);
        }

        [Fact]
        [Trait(nameof(BorrowerManagerTests), nameof(BorrowerManager.GetBorrowersAsync))]
        public async Task GetBorrowers_PageOne_ShouldReturnTenCategoriesAndLastItemBe10th()
        {
            var borrowerManager = new BorrowerManager(_moqRepositoryFactory.Object);

            var borrowerDataItems = await borrowerManager.GetBorrowersAsync(1, 0);

            Assert.NotEmpty(borrowerDataItems);
            Assert.Equal(10, borrowerDataItems.Length);

            var lastUserInList = borrowerDataItems.Last();
            var borrower = _fakeDbBorrowers.ToArray()[10 - 1];

            Assert.Equal(borrower.PhoneNo, lastUserInList.PhoneNo);
        }

        [Fact]
        [Trait(nameof(BorrowerManagerTests), nameof(BorrowerManager.GetBorrowersAsync))]
        public async Task GetBorrowers_ItemTwenty_ShouldReturnTwentyBorrowersAndLastItemBe20th()
        {
            var borrowerManager = new BorrowerManager(_moqRepositoryFactory.Object);

            var borrowerDataItems = await borrowerManager.GetBorrowersAsync(0, 20);

            Assert.NotEmpty(borrowerDataItems);
            Assert.Equal(20, borrowerDataItems.Length);

            var lastUserInList = borrowerDataItems.Last();
            var borrower = _fakeDbBorrowers.ToArray()[20 - 1];

            Assert.Equal(borrower.PhoneNo, lastUserInList.PhoneNo);
        }

        [Theory]
        [MemberData(nameof(DifferentArguments))]
        [Trait(nameof(BorrowerManagerTests), nameof(BorrowerManager.GetBorrowersAsync))]
        public async Task GetBorrowers_DifferentObjects_ShouldReturnExpectedItems(int page, int item, 
            int expectedReturnItems, int expectedPage, int expectedItemPP)
        {
            var borrowerManager = new BorrowerManager(_moqRepositoryFactory.Object);

            var borrowerDataItems = await borrowerManager.GetBorrowersAsync(page, item);
            
            Assert.Equal(expectedReturnItems, borrowerDataItems.Length);
            Assert.Equal(expectedPage, borrowerManager.CurrentPage);
            Assert.Equal(expectedItemPP, borrowerManager.CurrentItemsPerPage);
        }


        // ===========================================================================================
        // GetBook
        // ===========================================================================================


        [Fact]
        [Trait(nameof(BorrowerManagerTests), nameof(BorrowerManager.GetBorrowerAsync))]
        public async Task GetBorrower_EmailExists_ReturnABorrower()
        {
            var email = "A@mail.local";
            var dbBorrower = _fakeDbBorrowers.Single(s => s.Email == email);
            var borrowerManager = new BorrowerManager(_moqRepositoryFactory.Object);

            var borrowerData = await borrowerManager.GetBorrowerAsync(email);

            Assert.NotNull(borrowerData);
            Assert.Equal(dbBorrower.PhoneNo, borrowerData.PhoneNo);
            Assert.Equal(dbBorrower.AvatarLink, borrowerData.ImageLink);
            Assert.Equal(dbBorrower.Email, borrowerData.Email);
            Assert.Equal(dbBorrower.FirstName, borrowerData.FirstName);
            Assert.Equal(dbBorrower.MiddleName, borrowerData.MiddleName);
            Assert.Equal(dbBorrower.LastName, borrowerData.LastName);
            Assert.Equal(dbBorrower.Gender.Type, borrowerData.Gender);
            Assert.Equal(dbBorrower.GenderId,
                _fakeDatabaseGenders.Single(s => s.Type == borrowerData.Gender).Id);
            Assert.Equal(dbBorrower.RegistrationDate, borrowerData.RegistrationDate);
            Assert.Equal(dbBorrower.Username, borrowerData.Username);
        }

        [Fact]
        [Trait(nameof(BorrowerManagerTests), nameof(BorrowerManager.GetBorrowerAsync))]
        public async Task GetBorrower_EmailNotExist_throwNotFoundException()
        {
            var email = "@mail.local";
            var borrowerManager = new BorrowerManager(_moqRepositoryFactory.Object);

            var exception = await Assert.ThrowsAsync<NotFoundException>(
                async () => await borrowerManager.GetBorrowerAsync(email));
            Assert.Equal($"Email does not exists!", exception.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        [Trait(nameof(BorrowerManagerTests), nameof(BorrowerManager.GetBorrowerAsync))]
        public async Task GetBorrower_WrongIsbnFormat_ThrowNotFoundException(string email)
        {
            var borrowerManager = new BorrowerManager(_moqRepositoryFactory.Object);

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                async () => await borrowerManager.GetBorrowerAsync(email));
            Assert.Equal("Email cannot be null or empty!", exception.Message);
        }



        public static IEnumerable<object[]> DifferentArguments()
        {
            yield return new object[] { 0, 0, 10, 1, 10};
            yield return new object[] { 0, 1, 10, 1, 10};
            yield return new object[] { 0, 10, 10, 1, 10};
            yield return new object[] { 0, 15, 10, 1, 10};
            yield return new object[] { 0, 20, 20, 1, 20 };
            yield return new object[] { 0, 25, 10, 1, 10 };
            yield return new object[] { 0, 30, 21, 1, 30};
            yield return new object[] { 1, 0, 10, 1, 10 };
            yield return new object[] { 1, 20, 20, 1, 20 };
            yield return new object[] { 1, 30, 21, 1, 30 };
            yield return new object[] { 2, 0, 10, 2, 10 };
            yield return new object[] { 2, 1, 10, 2, 10 };
            yield return new object[] { 2, 20, 1, 2, 20 };
            yield return new object[] { 2, 30, 21 , 1, 30};
            yield return new object[] { 3, 0, 1 , 3, 10};
            yield return new object[] { 3, 1, 1, 3, 10 };
            yield return new object[] { 3, 20, 1, 2, 20 };
        }

        private IEnumerable<Borrower> FeedCategories()
        {
            var cats = new List<Borrower>();

            _fakeDatabaseGenders = new List<Gender>
            {
                new Gender { Id = 1, Type = "female" },
                new Gender { Id = 2, Type = "male" }
            };

            for (int i = 1; i <= 21; i++)
            {
                var gender = i % 2 != 0
                    ? _fakeDatabaseGenders.ToList()[i % 2 - 1]
                    : _fakeDatabaseGenders.ToList()[1];
                var letter = ((char)('A' + i - 1)).ToString();
                cats.Add(new Borrower
                {
                    Id = i,
                    FirstName = "F_" + letter,
                    MiddleName = "M_" + letter,
                    LastName = "L_" + letter,
                    Gender = gender,
                    PhoneNo = i.ToString(),
                    AvatarLink = "http://" + ((char)('A' + i - 1)) + ".jpg",
                    GenderId = gender.Id,
                    Password = i.ToString(), // TODO ake it MD5 hash code
                    RegistrationDate = DateTime.Now,
                    Email = $"{letter}@mail.local",
                    Username = letter + i // TODO
                });
            }



            return cats;
        }
    }


}
