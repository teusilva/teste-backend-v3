using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using TheatricalPlayersRefactoringKata.Domain.Repositories;
using TheatricalPlayersRefactoringKata.Infrastructure.DbContexts;
using TheatricalPlayersRefactoringKata.Infrastructure.Repositories;

namespace TheatricalPlayersRefactoringKata.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPersistenceContexts(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<IApplicationDbContext,ApplicationDbContext>();
            AddDbContext(services, configuration);
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IPlayRepository, PlayRepository>();
            services.AddTransient<IInvoiceRepository, InvoiceRepository>();
            services.AddTransient<IPerformanceRepository, PerformanceRepository>();
        }

        [ExcludeFromCodeCoverage]
        private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(
                     options => options.UseLazyLoadingProxies()
                     .UseInMemoryDatabase("ApplicationConnection")
                 );
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(
                     options => options.UseLazyLoadingProxies()
                     .UseNpgsql(configuration.GetConnectionString("ApplicationConnection"),
                     b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
                 );
            }
        }
    }
}
