using DemoApp.Data.Dtos;
using DemoApp.Data.Interfaces;
using DemoApp.Data.Models;
using DemoApp.Data.Services;
using ExpressMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DemoApp.Data {
	public static class ServiceExtensions {
		/// <summary>
		/// When creating a .Net core library, I always include a ServiceExtensions class that, at a minimum, has an Add method.
		/// This allows me to register all services for this library in one place without cluttering up the Startup.cs file.  It doesn't
		/// work as well if you're attempting to use loose coupling, but it's possible, using reflection, to load and execute the proper
		/// function.
		/// </summary>
		/// <param name="services">The services.</param>
		/// <returns>IServiceCollection.</returns>
		public static IServiceCollection AddDemoAppData(this IServiceCollection services) {

			services.AddDbContext<DemoAppContext>(options => options.UseInMemoryDatabase(databaseName: "DemoData"));
			services.AddTransient<IDeliveriesService, DeliveriesService>()
				.AddTransient<IVehiclesService, VehiclesService>();

			Mapper.Register<Vehicle, ListDto>();
			Mapper.Register<Vehicle, VehicleDto>();
			return services;
		}
	}
}
