using System.Globalization;
using AxiomSysPortal.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace AxiomSysPortal.Pages.SALES.DELIVERYTERMS
{
	[Authorize]
	public class IndexModel : PageModel
	{
		private readonly IConfiguration _configuration;

		public IndexModel(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		[BindProperty]
		public DeliveryTermModel Term { get; set; } = new DeliveryTermModel();

		public List<DeliveryTermModel> Terms { get; set; } = new List<DeliveryTermModel>();

		private string ApiUrl(string path)
		{
			return _configuration.GetValue<string>("Uri") + "Sales/SaDeliveryTerm/" + path;
		}

		private bool IsArabic()
		{
			return CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ar";
		}

		// Token expired or missing: sign out and send the user to the login page
		private async Task<IActionResult> SignOutAndGoToLogin()
		{
			await HttpContext.SignOutAsync("Cookies");
			return Redirect("/Login");
		}

		public async Task<IActionResult> OnGet()
		{
			string token = Helper.GetClaimValue(User, "token");
			HttpResponseMessage response = await Helper.GetRequest(ApiUrl("GetAll"), token);

			if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
			{
				return await SignOutAndGoToLogin();
			}

			if (response.IsSuccessStatusCode)
			{
				string json = await response.Content.ReadAsStringAsync();
				List<DeliveryTermModel>? list = JsonConvert.DeserializeObject<List<DeliveryTermModel>>(json);
				if (list != null)
				{
					Terms = list;
				}
			}
			else
			{
				TempData["ErrorMessage"] = IsArabic() ? "تعذر تحميل البيانات" : "Could not load the data.";
			}

			return Page();
		}

		// AJAX handler: /...?handler=Details&code=1 (fills the Edit modal)
		public async Task<IActionResult> OnGetDetails(int code)
		{
			string token = Helper.GetClaimValue(User, "token");
			HttpResponseMessage response = await Helper.GetRequest(ApiUrl("GetByCode?code=" + code), token);

			if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
			{
				return Unauthorized();
			}

			if (!response.IsSuccessStatusCode)
			{
				return NotFound();
			}

			string json = await response.Content.ReadAsStringAsync();
			return Content(json, "application/json");
		}

		// The button name picks the branch, same idea as the pack's snippet C1
		public async Task<IActionResult> OnPost()
		{
			string token = Helper.GetClaimValue(User, "token");
			HttpResponseMessage? response = null;

			if (Request.Form.ContainsKey("AddDetail"))
			{
				response = await Helper.PostRequest(
					ApiUrl("AddSaDeliveryTerm"), JsonConvert.SerializeObject(Term), token);
			}
			else if (Request.Form.ContainsKey("EditDetail"))
			{
				response = await Helper.PostRequest(
					ApiUrl("EditSaDeliveryTerm"), JsonConvert.SerializeObject(Term), token);
			}
			else if (Request.Form.ContainsKey("DeleteDetail"))
			{
				response = await Helper.PostRequest(
					ApiUrl("DeleteSaDeliveryTerm?code=" + Term.Code), "", token);
			}

			if (response != null)
			{
				if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
				{
					return await SignOutAndGoToLogin();
				}

				await SetResponseMessage(response);
			}

			return RedirectToPage();
		}

		// Success goes to SuccessMessage, anything else to ErrorMessage
		private async Task SetResponseMessage(HttpResponseMessage response)
		{
			bool arabic = IsArabic();

			if (!response.IsSuccessStatusCode)
			{
				TempData["ErrorMessage"] = arabic ? "حدث خطأ أثناء الاتصال بالخادم" : "The server returned an error.";
				return;
			}

			string json = await response.Content.ReadAsStringAsync();
			ApiResponse? result = JsonConvert.DeserializeObject<ApiResponse>(json);

			if (result == null)
			{
				TempData["ErrorMessage"] = arabic ? "رد غير صالح" : "Invalid response.";
				return;
			}

			string message = arabic ? result.MessageAr : result.MessageEn;
			if (result.Status)
			{
				TempData["SuccessMessage"] = message;
			}
			else
			{
				TempData["ErrorMessage"] = message;
			}
		}
	}
}