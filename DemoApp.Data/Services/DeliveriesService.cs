using DemoApp.Data.Core;
using DemoApp.Data.Interfaces;
using DemoApp.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoApp.Data.Services {
	public class DeliveriesService : ServiceCore<Delivery>, IDeliveriesService {
		public DeliveriesService(DemoAppContext context, ILogger<DeliveriesService> logger) : base(context, logger) {
		}

		public IEnumerable<Delivery> GetDeliveriesForVehicle(int vehicleId) {
			try {
				return _context.Deliveries.Where(d => d.VehicleId == vehicleId);
			} catch (Exception e) {
				_logger.LogError(e, $"An error occurred obtaining a list of deliveries for the specified vehicle");
				throw;
			}
		}

		public override void Create(Delivery entity) {
			try {
				var trackingPrefix = $"{DateTime.Now.ToString("yyyyMMdd")}{entity.VehicleId:d00}";
				var lastEntry = _context.Deliveries.Where(e => e.TrackingNumber.StartsWith(trackingPrefix, StringComparison.CurrentCulture)).OrderByDescending(e => e.Id).FirstOrDefault();
				if (lastEntry == null) {
					entity.TrackingNumber = $"{trackingPrefix}001";
				} else {
					var trackingNumber = Convert.ToInt32(lastEntry.TrackingNumber.Substring(lastEntry.TrackingNumber.Length - 3, 3)) + 1;
					entity.TrackingNumber = $"{trackingPrefix}{string.Format("{0:D3}", trackingNumber)}";
				}
				_context.Deliveries.Add(entity);
				_context.SaveChanges();
			} catch (Exception e) {
				_logger.LogError(e, $"An error occurred creating a delivery");
				throw;
			}
		}
		public override void Delete(int id) {
			try {
				var entity = _context.Deliveries.First(e => e.Id == id);
				_context.Deliveries.Remove(entity);
				_context.SaveChanges();
			} catch (Exception e) {
				_logger.LogError(e, $"An error occurred deleting a delivery");
				throw;
			}
		}
	}
}
