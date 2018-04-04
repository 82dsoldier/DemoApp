using DemoApp.Data.Dtos;
using DemoApp.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DemoApp.Controllers {
	public class VehiclesController : Controller {
		IVehiclesService _dataService;

		public VehiclesController(IVehiclesService dataService) {
			_dataService = dataService;
		}

		[HttpGet]
		public IActionResult GetVehicles()
			=> Json(_dataService.GetList<ListDto>());

		[HttpGet]
		public IActionResult AddVehicle()
			=> View("AddVehicle", new VehicleDto());

		[HttpPost]
		public IActionResult AddVehicle(VehicleDto model) {
			_dataService.Create(model);
			return View("CloseCurrentView");
		}
	}
}
