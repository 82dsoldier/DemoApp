using DemoApp.Data.Interfaces;
using DemoApp.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;

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
				} catch (Exception e) {
					//Log the exception
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
			} catch (Exception e) {
				//log the exception
				return StatusCode(500);
			}
		}

		[HttpGet]
		public IActionResult GetDeliveriesForVehicle(int vehicleId) {
			try {
				return Json(_dataService.GetDeliveriesForVehicle(vehicleId));
			} catch (Exception e) {
				//log exception
				return StatusCode(500);
			}
		}
	}
}
