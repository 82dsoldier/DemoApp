using DemoApp.Data.Models;
using System.Collections.Generic;

namespace DemoApp.Data.Interfaces {
	public interface IDeliveriesService : IServiceCore<Delivery> {
		IEnumerable<Delivery> GetDeliveriesForVehicle(int vehicleId);
	}
}
