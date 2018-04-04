using DemoApp.Data.Core;
using DemoApp.Data.Interfaces;
using DemoApp.Data.Models;
using System.Linq;

namespace DemoApp.Data.Services {
	public class VehiclesService : ServiceCore<Vehicle>, IVehiclesService {
		public VehiclesService(DemoAppContext context) : base(context) {
		}

		public override void Delete(int id) {
			var entity = _context.Vehicles.First(e => e.Id == id);
			_context.Vehicles.Remove(entity);
			_context.SaveChanges();
		}
	}
}
