namespace Core.Common.Interfaces.Entities
{
    /// <summary>
    /// Get ONLY Entity RowVersion field in string
    /// </summary>
    public interface IEntityVersion 
    {
        string Version { get; }
    }
}
