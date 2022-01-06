using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
