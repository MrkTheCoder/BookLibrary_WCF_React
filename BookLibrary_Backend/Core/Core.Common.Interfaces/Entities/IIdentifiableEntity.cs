namespace Core.Common.Interfaces.Entities
{
    /// <summary>
    /// Implementing on all Server side entities to return/set its Id property.
    /// </summary>
    public interface IIdentifiableEntity
    {
        int EntityId { get; set; }
    }
}
