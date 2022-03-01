﻿using BookLibrary.Business.AppConfigs;
using Core.Common.Interfaces.Data;
using DryIoc;

namespace BookLibrary.Business.Services
{
    public abstract class ManagerBase
    {
        private Container _container;
        
        protected IRepositoryFactory RepositoryFactory { get; set; }

        public ManagerBase()
        {
            RepositoryFactory = Container.Resolve<IRepositoryFactory>();
        }

        public ManagerBase(IRepositoryFactory repositoryFactory)
        {
            RepositoryFactory = repositoryFactory;
        }

        public Container Container => _container ?? 
                                      (_container = BootContainer.Builder = Bootstrapper.Bootstrapper.Bootstrap());


    }
}