using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Threading;
using System.Threading.Tasks;
using BookLibrary.Business.Contracts.DataContracts;
using BookLibrary.Business.Contracts.ServiceContracts;
using BookLibrary.Business.Entities;
using BookLibrary.DataAccess.Dto;
using BookLibrary.DataAccess.Interfaces;
using Core.Common.Interfaces.Data;
using Timer = System.Timers.Timer;

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

            InitializePaging(page, item);

            var borrowerRepository = RepositoryFactory.GetEntityRepository<IBorrowerRepository>();
            PagingEntityDto<Borrower> filteredBorrowers = await borrowerRepository.GetFilteredBorrowersAsync(CurrentPage, CurrentItemsPerPage);

            SetHeaders(filteredBorrowers.TotalItems, CurrentPage, CurrentItemsPerPage);

            return MapBorrowersToBorrowersData(filteredBorrowers.Entities);
        }

        private BorrowerData[] MapBorrowersToBorrowersData(List<Borrower> borrowers)
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
                TotalBorrows = _rand.Next(1,20),
                RegistrationDate = borrower.RegistrationDate,
                Gender = borrower.Gender.Type,
                PhoneNo = borrower.PhoneNo
            };
        }
    }
}
