namespace Core.Common.Interfaces.Entities
{
    /// <summary>
    /// Get a unique field + Version field. To return unique string for each entity.
    /// </summary>
    public interface IEntityEtag
    {
        string ETag { get; }
    }
}
