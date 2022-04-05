﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLibrary.Business.Entities;
using BookLibrary.Business.Services.Managers;
using BookLibrary.DataAccess.Dto;
using BookLibrary.DataAccess.Interfaces;
using BookLibrary.DataAccess.SQLite;
using Core.Common.Interfaces.Data;
using Moq;
using Xunit;

namespace BookLibrary.Tests.UnitTests.WcfServices
{
    public class BorrowerManagerTests
    {
        private readonly Mock<IRepositoryFactory> _moqRepositoryFactory;
        private readonly Mock<IBorrowerRepository> _moqBorrowerRepository;
        private IEnumerable<Borrower> _fakeDbBorrowers;

        public BorrowerManagerTests()
        {
            _fakeDbBorrowers = FeedCategories();

            _moqBorrowerRepository = new Mock<IBorrowerRepository>();
            _moqBorrowerRepository.Setup(s => s.GetFilteredBorrowersAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((int page, int item) =>
               {
                   var newItem = item == -1 ? _fakeDbBorrowers.ToList().Count : item;
                   var pagingEntityDto = new PagingEntityDto<Borrower>
                   { TotalItems = _fakeDbBorrowers.ToList().Count,
                       Entities = _fakeDbBorrowers
                           .Skip(newItem * (page - 1))
                           .Take(newItem)
                           .ToList()
                   };

                   return pagingEntityDto;
               });

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
        public async Task GetBorrowers_DifferentObjects_ShouldReturnExpectedItems(int page, int item, int expected)
        {
            var borrowerManager = new BorrowerManager(_moqRepositoryFactory.Object);

            var borrowerDataItems = await borrowerManager.GetBorrowersAsync(page, item);

            Assert.Equal(expected, borrowerDataItems.Length);
        }

        public static IEnumerable<object[]> DifferentArguments()
        {
            yield return new object[] { 0, 0, 21 };
            yield return new object[] { 0, 1, 10 };
            yield return new object[] { 0, 10, 10 };
            yield return new object[] { 0, 15, 10 };
            yield return new object[] { 0, 20, 20 };
            yield return new object[] { 0, 25, 10 };
            yield return new object[] { 0, 30, 21 };
            yield return new object[] { 1, 0, 10 };
            yield return new object[] { 1, 20, 20 };
            yield return new object[] { 1, 30, 21 };
            yield return new object[] { 2, 0, 10 };
            yield return new object[] { 2, 1, 10 };
            yield return new object[] { 2, 20, 1 };
            yield return new object[] { 2, 30, 0 };
            yield return new object[] { 3, 0, 1 };
            yield return new object[] { 3, 1, 1 };
            yield return new object[] { 3, 20, 0 };
        }

        private IEnumerable<Borrower> FeedCategories()
        {
            var cats = new List<Borrower>();
            for (int i = 1; i <= 21; i++)
            {
                cats.Add(new Borrower
                {
                    Id = i,
                    FirstName = "F_" + ((char)('A' + i - 1)).ToString(),
                    MiddleName = "M_" + ((char)('A' + i - 1)).ToString(),
                    LastName = "L_" + ((char)('A' + i - 1)).ToString(),
                    Gender = new Gender(),// TODO
                    PhoneNo = i.ToString(),
                    AvatarLink = "http://" + ((char)('A' + i - 1)) + ".jpg",
                    GenderId = i,// TODO
                    Password = i.ToString(), // TODO
                    RegistrationDate = DateTime.Now,
                    Username = ((char)('A' + i - 1)).ToString() + i // TODO

                });
            }

            return cats;
        }
    }


}