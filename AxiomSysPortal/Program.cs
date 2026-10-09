using Microsoft.AspNetCore.Localization.Routing;

namespace AxiomSysPortal
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddRazorPages();

			builder.Services.AddHttpContextAccessor();
			builder.Services.AddHttpClient();

			// Cookie login: users who are not logged in go to /Login
			builder.Services.AddAuthentication("Cookies").AddCookie("Cookies", ConfigureCookie);
			builder.Services.AddAuthorization();




			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseRouting();


			// Culture comes from the address: /en/... or /ar/...
			RequestLocalizationOptions localization = new RequestLocalizationOptions();
			localization.SetDefaultCulture("en");
			localization.AddSupportedCultures("en", "ar");
			localization.AddSupportedUICultures("en", "ar");
			RouteDataRequestCultureProvider provider = new RouteDataRequestCultureProvider();
			provider.Options = localization;
			localization.RequestCultureProviders.Insert(0, provider);
			app.UseRequestLocalization(localization);



			app.UseAuthentication();
			app.UseAuthorization();

			//app.MapStaticAssets();
			app.MapRazorPages();

			app.Run();

			void ConfigureCookie(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions options)
			{
				options.LoginPath = "/Login";
			}
		}
	}
}
