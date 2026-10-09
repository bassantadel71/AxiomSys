using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace AxiomSysPortal
{
	public static class Helper
	{
		// Reads one value from the logged-in user's claims
		public static string GetClaimValue(ClaimsPrincipal user, string claimType)
		{
			var claim = user.FindFirst(claimType);
			if (claim == null)
			{
				return "";
			}
			return claim.Value;
		}

		public static async Task<HttpResponseMessage> GetRequest(string url, string token)
		{
			HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			HttpResponseMessage response = await client.GetAsync(url);
			return response;
		}
		public static async Task<HttpResponseMessage> PostRequest(string url, string json, string token)
		{
			HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await client.PostAsync(url, content);
			return response;
		}
	}
}
