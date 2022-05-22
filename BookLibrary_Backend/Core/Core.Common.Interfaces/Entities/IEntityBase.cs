namespace Core.Common.Interfaces.Entities
{
    /// <summary>
    /// Base interface to implement all other related Entity interfaces
    /// </summary>
    public interface IEntityBase : IIdentifiableEntity, IEntityVersion, IEntityEtag
    {
    }
}
