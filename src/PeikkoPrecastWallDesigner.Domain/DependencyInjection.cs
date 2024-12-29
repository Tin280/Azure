using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using PeikkoPrecastWallDesigner.Domain.Service.Computations;
using PeikkoPrecastWallDesigner.Domain.Services.Computations;

namespace PeikkoPrecastWallDesigner.Domain
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddDomain(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddScoped<IComputingDService, ComputingDService>(); 
			return (services);
		}
	}
}
