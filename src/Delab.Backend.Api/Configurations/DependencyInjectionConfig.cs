using Delab.AccessData.Context;
using Delab.AccessData.Data;
using Delab.AccessData.Repositories;
using Delab.Backend.Api.BusinessServices;
using Delab.Backend.Api.Notifications;
using Delab.Shared.Interfaces.BusinessServices;
using Delab.Shared.Interfaces.Repositories;

namespace Delab.Backend.Api.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static void ResolveDependencies(this IServiceCollection services)
        {
            services.AddTransient<SeedDb>();

            services.AddScoped<DBContext>();            

            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IStateRepository, StateRepository>();
            services.AddScoped<ICityRepository, CityRepository>();

            services.AddScoped<INotifier, Notifier>();
            services.AddScoped<ICountryBusinessService, CountryBusinessService>();
            services.AddScoped<IStateBusinessService, StateBusinessService>();
            services.AddScoped<ICityBusinessService, CityBusinessService>();
        }
    }
}
