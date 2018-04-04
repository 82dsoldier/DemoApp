using DemoApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoApp.Data {
	public class DemoAppContext : DbContext {
		public DbSet<Delivery> Deliveries { get; set; }
		public DbSet<Vehicle> Vehicles { get; set; }
		public DemoAppContext(DbContextOptions<DemoAppContext> options) : base(options) {

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {

			modelBuilder.Entity<Vehicle>(entity => {
				entity.HasKey(a => a.Id);
				entity.Property(a => a.Id).IsRequired().ValueGeneratedOnAdd();
				entity.Property(a => a.Name).IsRequired().HasMaxLength(256);
			});

			modelBuilder.Entity<Delivery>(entity => {
				entity.HasKey(a => a.Id);
				entity.Property(a => a.Id).IsRequired().ValueGeneratedOnAdd();
				entity.Property(a => a.Destination).IsRequired().HasMaxLength(512);
				entity.Property(a => a.Name).IsRequired().HasMaxLength(256);
				entity.Property(a => a.Origin).IsRequired().HasMaxLength(256);
				entity.Property(a => a.TrackingNumber).IsRequired().HasMaxLength(32);
				entity.Property(a => a.VehicleId).IsRequired();
				entity.HasOne(a => a.Vehicle).WithMany(b => b.Deliveries).HasForeignKey(c => c.VehicleId).OnDelete(DeleteBehavior.Restrict);
			});
		}
	}
}
