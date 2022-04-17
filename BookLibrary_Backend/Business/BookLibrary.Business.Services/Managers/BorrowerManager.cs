using BookLibrary.Business.Contracts.DataContracts;
using BookLibrary.Business.Contracts.ServiceContracts;
using BookLibrary.Business.Entities;
using BookLibrary.DataAccess.Dto;
using BookLibrary.DataAccess.Interfaces;
using Core.Common.Exceptions;
using Core.Common.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Threading.Tasks;

namespace BookLibrary.Business.Services.Managers
{
    /// <summary>
    /// Borrower Service for managing library users in database and retrieve information based on API requests.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
        ConcurrencyMode = ConcurrencyMode.Multiple,
        IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class BorrowerManager : ManagerBase, IBorrowerService
    {
        public BorrowerManager() : base()
        { }
        public BorrowerManager(IRepositoryFactory resRepositoryFactory) : base(resRepositoryFactory)
        { }

        private readonly Random _rand = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<BorrowerData[]> GetBorrowersAsync(int page = 0, int item = 0)
        {
            if (page < 0 || item < 0)
                throw new ArgumentException("Page & Item arguments must be zero or a positive number");

            var borrowerRepository = RepositoryFactory.GetEntityRepository<IBorrowerRepository>();

            var totalItems = await borrowerRepository.GetCountAsync();

            InitializePaging(totalItems, page, item);

            var borrowers = await borrowerRepository.GetFilteredBorrowersAsync(CurrentPage, CurrentItemsPerPage);

            return MapBorrowersToBorrowersData(borrowers);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<BorrowerData> GetBorrowerAsync(string email)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email.Trim()))
                throw new ArgumentException("Email cannot be null or empty!");

            var borrowerRepository = RepositoryFactory.GetEntityRepository<IBorrowerRepository>();
            var borrower = await borrowerRepository.GetByExpressionAsync(b => b.Email == email);

            if (borrower == null)
                throw new NotFoundException("Email does not exists!");

            return MapBorrowerToBorrowerData(borrower);
        }



        private BorrowerData[] MapBorrowersToBorrowersData(IEnumerable<Borrower> borrowers)
        {
            var borrowerData = new List<BorrowerData>();

            foreach (var borrower in borrowers)
                borrowerData.Add(MapBorrowerToBorrowerData(borrower));

            return borrowerData.ToArray();
        }

        private BorrowerData MapBorrowerToBorrowerData(Borrower borrower)
        {


            return new BorrowerData
            {
                FirstName = borrower.FirstName,
                MiddleName = borrower.MiddleName,
                LastName = borrower.LastName,
                ImageLink = borrower.AvatarLink,
                TotalBorrows = _rand.Next(1, 20),
                RegistrationDate = borrower.RegistrationDate,
                Gender = borrower.Gender.Type,
                PhoneNo = borrower.PhoneNo,
                Email = borrower.Email,
                Username = borrower.Username,
            };
        }
    }
}
