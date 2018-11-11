using BL;
using Cache;
using DAL;
using Interface;
using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;

namespace PhoneBook
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IContactsDetailsDAL, ContactsDAL>();
            container.RegisterType<IContactDetailsCache, ContactDetailsCache>();
            container.RegisterType<IContactDetailsBL, ContactDetailsBL>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}