using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AxiomSysSL.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DevAuthController : ControllerBase
	{
		private readonly IConfiguration _config;

		public DevAuthController(IConfiguration config)
		{
			_config = config;
		}

		[HttpGet]
		public IActionResult Get(int userId = 1, int company = 1)
		{
			string secret = _config["Jwt:Key"]!;
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			List<Claim> claims = new List<Claim>();
			claims.Add(new Claim("Id", userId.ToString()));
			claims.Add(new Claim("CompanyCode", company.ToString()));

			var token = new JwtSecurityToken(
				claims: claims,
				expires: DateTime.UtcNow.AddHours(8),
				signingCredentials: credentials);

			string tokenText = new JwtSecurityTokenHandler().WriteToken(token);
			return Ok(tokenText);
		}
	}
}
