using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Json;
using System;

namespace DemoApp {
	public class Program {
		public static void Main(string[] args) {
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
				.Enrich.WithExceptionDetails()
				.WriteTo.RollingFile(new JsonFormatter(renderMessage: true), @"C:\logs\log-{Date}.log")
				.CreateLogger();

			try {
				BuildWebHost(args).Run();
			} catch (Exception e) {
				Log.Fatal(e, "Host terminated unexpectedly");
			} finally {
				Log.CloseAndFlush();
			}
		}

		public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.UseSerilog()
				.Build();
	}
}
