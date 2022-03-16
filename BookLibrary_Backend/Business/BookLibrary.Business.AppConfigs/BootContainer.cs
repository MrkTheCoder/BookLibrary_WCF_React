using DryIoc;

namespace BookLibrary.Business.AppConfigs
{
    /// <summary>
    /// This is a simple static property to store IoC container.
    /// </summary>
    public static class BootContainer
    {
        public static Container Builder { get; set; }
    }
}
