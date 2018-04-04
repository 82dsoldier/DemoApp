using DemoApp.Data.Interfaces;
using System.Collections.Generic;

namespace DemoApp.Data.Models {
	public class Vehicle : IModel {
		public int Id { get; set; }
		public string Name { get; set; }
		public virtual ICollection<Delivery> Deliveries { get; set; }
	}
}
