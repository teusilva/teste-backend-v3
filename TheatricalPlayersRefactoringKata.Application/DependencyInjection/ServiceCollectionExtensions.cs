using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using TheatricalPlayersRefactoringKata.Application.ExternalServices.RabbitMQ.Consumer;
using TheatricalPlayersRefactoringKata.Application.ExternalServices.RabbitMQ.Publisher;
using TheatricalPlayersRefactoringKata.Application.Mappings;
using TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.Create;
using TheatricalPlayersRefactoringKata.Infrastructure.DbContexts;
using TheatricalPlayersRefactoringKata.Infrastructure.DependencyInjection;

namespace TheatricalPlayersRefactoringKata.Application.DependencyInjection
{
    [ExcludeFromCodeCoverage]
	public static class ServiceCollectionExtensions
	{
		public static void AddDependencyApplicationSetup(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddApplicationLayer();
			services.AddValidation();
			services.AddInfrastructure(configuration);
			services.AddRabbitMQ();
		}

		private static void AddApplicationLayer(this IServiceCollection services)
		{
			services.AddAutoMapper(typeof(ManagerProfile));
			services.AddMediatR(c => c.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
		}

		private static void AddValidation(this IServiceCollection services)
		{
			services.AddFluentValidationAutoValidation();
			services.AddFluentValidationClientsideAdapters();
			services.AddValidatorsFromAssemblyContaining<CommandValidator>();
		}
        
        private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddPersistenceContexts(configuration);
			services.AddRepositories();
			services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();
		}

        private static void AddRabbitMQ(this IServiceCollection services)
        {
            services.AddSingleton<IInvoicePublisher, InvoicePublisher>();
            services.AddHostedService<InvoiceConsumer>();
        }
    }
}