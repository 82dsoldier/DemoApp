using DemoApp.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DemoApp {
	/// <summary>
	/// As this is a bare-bones application, I have forgone several things such as strict data checking (I do some in the Javascript and some in the controllers)
	/// and error logging.  Normally I would create and register some sort of logging functionality such as Serilog (I hate the built-in logging) and use it
	/// for logging errors/warnings.  I would also normally create or already have created a base controller type that would automatically inject the logging object
	/// into each controller and a base service class that would do the same for the services.
	/// </summary>
	public class Startup {
		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }
		public void ConfigureServices(IServiceCollection services) {
			services.AddMvc();
			services.AddDemoAppData();
		}
		public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
			if (env.IsDevelopment()) {
				app.UseBrowserLink();
				app.UseDeveloperExceptionPage();
			} else {
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();

			app.UseMvc(routes => {
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
