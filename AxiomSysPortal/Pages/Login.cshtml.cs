using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace AxiomSysPortal.Pages
{
    public class LoginModel : PageModel
    {
		private readonly IConfiguration _configuration;

		private readonly IHttpClientFactory _httpClientFactory;

		public LoginModel(
			IConfiguration configuration,
			IHttpClientFactory httpClientFactory)
		{
			_configuration = configuration;
			_httpClientFactory = httpClientFactory;
		}


		//public async Task<IActionResult> OnPost()
		//{
		//	// 1. Ask the API's dev endpoint for a token
		//	string url = _configuration.GetValue<string>("Uri") + "dev/token";
		//	HttpClient client = new HttpClient();
		//	string token = await client.GetStringAsync(url);

		//	// 2. Keep the token in the user's cookie as a claim called "token"
		//	List<Claim> claims = new List<Claim>();
		//	claims.Add(new Claim("token", token.Trim('"')));
		//	ClaimsIdentity identity = new ClaimsIdentity(claims, "Cookies");
		//	ClaimsPrincipal principal = new ClaimsPrincipal(identity);
		//	await HttpContext.SignInAsync("Cookies", principal);

		//	// 3. Go to the delivery terms page
		//	return Redirect("/en/SALES/DELIVERYTERMS/Index");
		//}


		//public void OnGet()
		//      {
		//      }


		public async Task<IActionResult> OnPost()
		{
			string baseUrl = "https://localhost:7075";
			string url = $"{baseUrl}/api/DevAuth?userId=1&company=1";

			var client = _httpClientFactory.CreateClient();

			string token = await client.GetStringAsync(url);

			List<Claim> claims = new List<Claim>
					{
						new Claim("token", token.Trim('"'))
					};

			ClaimsIdentity identity = new ClaimsIdentity(claims, "Cookies");
			ClaimsPrincipal principal = new ClaimsPrincipal(identity);

			await HttpContext.SignInAsync("Cookies", principal);

			return Redirect("/en/SALES/DELIVERYTERMS/Index");
		}



	}
}
