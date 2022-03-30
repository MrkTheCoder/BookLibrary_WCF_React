using System;
using DryIoc;

namespace BookLibrary.Business.AppConfigs
{
    /// <summary>
    /// This is a simple static class/property to store instance of IoC container.
    /// </summary>
    public static class BootContainer
    {
        private static Container _builder;
        private static readonly object PadLock = new object();

        public static Container Builder
        {
            get => _builder ?? 
                   throw new NullReferenceException("Must instantiate container before use Builder!");
            set
            {
                // Double Check PadLock Pattern
                if (_builder != null) return;
                
                lock (PadLock)
                {
                    // .Net >= v8 : _builder ??= value;
                    if (_builder == null)
                        _builder = value;
                }
            }
        }
    }
}
