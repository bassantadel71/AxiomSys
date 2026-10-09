using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using System.Globalization;

namespace AxiomSysPortal
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			void ConfigureCookie(CookieAuthenticationOptions options)
			{
				options.LoginPath = "/Login";
			}

			// Add services to the container.
			builder.Services.AddRazorPages();

			builder.Services.AddHttpClient();
			builder.Services.AddHttpContextAccessor();

			builder.Services.AddSingleton<SharedLocalizer>();

			// Cookie login: users who are not logged in go to /Login
			builder.Services.AddAuthentication("Cookies").AddCookie("Cookies", ConfigureCookie);
			builder.Services.AddAuthorization();




			var app = builder.Build();

			

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseRouting();


			CultureInfo english = new CultureInfo("en");
			CultureInfo arabic = new CultureInfo("ar");
			arabic.NumberFormat.NativeDigits = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
			arabic.NumberFormat.DigitSubstitution = DigitShapes.None;

			RequestLocalizationOptions localization = new RequestLocalizationOptions();
			localization.DefaultRequestCulture = new RequestCulture(english);
			localization.SupportedCultures = new List<CultureInfo> { english, arabic };
			localization.SupportedUICultures = new List<CultureInfo> { english, arabic };

			// The culture comes from the address: /en/... or /ar/...
			RouteDataRequestCultureProvider provider = new RouteDataRequestCultureProvider();
			provider.Options = localization;
			localization.RequestCultureProviders.Insert(0, provider);
			app.UseRequestLocalization(localization);

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapRazorPages();
			app.Run();

			
		}
	}
}
