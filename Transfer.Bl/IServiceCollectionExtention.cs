using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Transfer.Bl
{
    public static class IServiceCollectionExtention
    {
        public static void TransferBlConfigue(this IServiceCollection serviceCollection)
        {
            Assembly currentAssem = Assembly.GetExecutingAssembly();
            //services.AddAutoMapper(ServiceCollectionExtension.GetAllAssemblies());
            serviceCollection.AddAutoMapper(currentAssem);
        }
    }
}
