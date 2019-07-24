using Microsoft.Practices.Unity;
using Upload.Infra.Persistence.Repository;
using prmToolkit.NotificationPattern;
using System.Data.Entity;
using Upload.Domain.Interfaces.Repositories;
using Upload.Domain.Interfaces.Repositories.Base;
using Upload.Domain.Interfaces.Services;
using Upload.Domain.Services;
using Upload.Infra.Persistence;
using Upload.Infra.Persistence.Repository.Base;
using Upload.Infra.Transaction;

namespace Upload.IoC.Unity
{
    public static class DependencyResolver
    {
        public static void Resolve(UnityContainer container)
        {
            container.RegisterType<DbContext, UploadContext>(new HierarchicalLifetimeManager());
            container.RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<INotifiable, Notifiable>(new HierarchicalLifetimeManager());
            container.RegisterType(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));
            container.RegisterType<IRepositoryUser, RepositoryUser>(new HierarchicalLifetimeManager());
            container.RegisterType<IRepositoryFile, RepositoryFile>(new HierarchicalLifetimeManager());
            container.RegisterType<IServiceUser, ServiceUser>(new HierarchicalLifetimeManager());
            container.RegisterType<IServiceFile, ServiceFile>(new HierarchicalLifetimeManager());
        }
    }
}
