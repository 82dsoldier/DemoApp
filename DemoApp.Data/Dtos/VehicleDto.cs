using DemoApp.Data.Interfaces;

namespace DemoApp.Data.Dtos {

	public class VehicleDto : IModel {
		public int Id { get; set; }
		public string Name { get; set; }
	}
}
