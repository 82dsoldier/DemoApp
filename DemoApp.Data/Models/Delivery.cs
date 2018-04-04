using DemoApp.Data.Interfaces;
using System;

namespace DemoApp.Data.Models {
	public class Delivery : IModel {
		public int Id { get; set; }
		public int VehicleId { get; set; }
		public string Name { get; set; }
		public string Origin { get; set; }
		public string Destination { get; set; }
		public DateTime DepartureTime { get; set; }
		public string TrackingNumber { get; set; }
		public virtual Vehicle Vehicle { get; set; }
	}
}
