
using AxiomSysDL.Context;
using AxiomSysSL.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AxiomSysSL
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			void ConfigureDatabase(DbContextOptionsBuilder options)
			{
				string connection = builder.Configuration.GetConnectionString("SupplyChain")!;
				options.UseOracle(connection);
			}

			void ConfigureJwt(JwtBearerOptions options)
			{
				string secret = builder.Configuration["Jwt:Key"]!;
				var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

				options.TokenValidationParameters = new TokenValidationParameters();
				options.TokenValidationParameters.ValidateIssuer = false;
				options.TokenValidationParameters.ValidateAudience = false;
				options.TokenValidationParameters.IssuerSigningKey = key;
			}

			// Add services to the container.

			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddHttpContextAccessor();

			builder.Services.AddDbContext<SupplyChainContext>(ConfigureDatabase);
			builder.Services.AddSalesModule();



			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(ConfigureJwt);
			builder.Services.AddAuthorization();

			var app = builder.Build();

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthentication();   // reads the token
			app.UseAuthorization();    // checks the token is allowed


			app.MapControllers();

			app.Run();
		}
	}
}
