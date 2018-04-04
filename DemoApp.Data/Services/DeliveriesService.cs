using DemoApp.Data.Core;
using DemoApp.Data.Interfaces;
using DemoApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoApp.Data.Services {
	public class DeliveriesService : ServiceCore<Delivery>, IDeliveriesService {
		public DeliveriesService(DemoAppContext context) : base(context) {
		}

		public IEnumerable<Delivery> GetDeliveriesForVehicle(int vehicleId)
			=> _context.Deliveries.Where(d => d.VehicleId == vehicleId);
		public override void Create(Delivery entity) {
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
		}
		public override void Delete(int id) {
			var entity = _context.Deliveries.First(e => e.Id == id);
			_context.Deliveries.Remove(entity);
			_context.SaveChanges();
		}
	}
}
