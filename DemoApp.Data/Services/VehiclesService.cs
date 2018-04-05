using DemoApp.Data.Core;
using DemoApp.Data.Interfaces;
using DemoApp.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace DemoApp.Data.Services {
	public class VehiclesService : ServiceCore<Vehicle>, IVehiclesService {
		public VehiclesService(DemoAppContext context, ILogger<VehiclesService> logger) : base(context, logger) {
		}

		public override void Delete(int id) {
			try {
				var entity = _context.Vehicles.First(e => e.Id == id);
				_context.Vehicles.Remove(entity);
				_context.SaveChanges();
			} catch (Exception e) {
				_logger.LogError(e, $"An error occurred deleting a vehicle");
				throw;
			}
		}
	}
}
