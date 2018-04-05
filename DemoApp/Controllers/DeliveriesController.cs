using DemoApp.Data.Interfaces;
using DemoApp.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoApp.Controllers {
	public class DeliveriesController : Controller {
		IDeliveriesService _dataService;

		public DeliveriesController(IDeliveriesService dataService) {
			_dataService = dataService;
		}

		[HttpPost]
		public IActionResult SaveDelivery([FromBody]Delivery model) {
			if (ModelState.IsValid) {
				try {
					_dataService.Create(model);

					return Created(Url.Action("Deliveries", "GetDelivery", new { id = model.Id }), model);
				} catch {
					//In a more detailed app, I would catch the exception and check for the exception type
					//This would allow me to send a more detailed status message back to the client.
					//Not that it would matter to the client, but some of them are smart enough to send along screen shots
					//when reporting issues.
					return StatusCode(500);
				}
			}
			return StatusCode(400);
		}

		[HttpGet]
		public IActionResult GetDelivery(int id) {
			try {
				var model = _dataService.Get(id);
				if (model != null)
					return Json(model);
				return StatusCode(404);
			} catch {
				return StatusCode(500);
			}
		}

		[HttpGet]
		public IActionResult GetDeliveriesForVehicle(int vehicleId) {
			try {
				return Json(_dataService.GetDeliveriesForVehicle(vehicleId));
			} catch {
				return StatusCode(500);
			}
		}
	}
}
