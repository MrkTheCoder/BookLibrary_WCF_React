using System.ServiceModel;

namespace BookLibrary.Business.Contracts
{
    /// <summary>
    /// Use this at second Constructor of ManagerBase only for Unit Testing purpose.
    /// </summary>
    [ServiceContract]
    public interface IFakeService
    {
        [OperationContract]
        void FakeMethod();
    }
}
