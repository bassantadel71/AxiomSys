using AxiomSysBL.IServices;
using AxiomSysBL.Services;
using AxiomSysBL.Services.SalesService;
using AxiomSysDL.Context;
using AxiomSysDL.Repository;
using AxiomSysSL.Security;

namespace AxiomSysSL.Extensions
{
	public static class SalesModuleServiceCollectionExtensions
	{
		public static IServiceCollection AddSalesModule(this IServiceCollection services)
		{
			// Data layer
			services.AddScoped<IUnitOfWork<SupplyChainContext>, UnitOfWork<SupplyChainContext>>();

			// Shared
			services.AddScoped<ISharedService, SharedService>();
			services.AddScoped<ICurrentUserService, CurrentUserService>();

			// Sales
			services.AddScoped<ISaDeliveryTermService, SaDeliveryTermService>();

			return services;
		}
	}
}
