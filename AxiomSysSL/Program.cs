using AxiomSysDL.Context;
using AxiomSysSL.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
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

			OpenApiSecurityRequirement CreateRequirement(OpenApiDocument document)
			{
				OpenApiSecurityRequirement requirement = new OpenApiSecurityRequirement();
				OpenApiSecuritySchemeReference reference = new OpenApiSecuritySchemeReference("Bearer", document);
				requirement.Add(reference, new List<string>());
				return requirement;
			}

			IList<string> GetTags(ApiDescription api)
			{
				string path = api.RelativePath ?? "";
				string[] parts = path.Split('/');

				// Any route that starts with api/Sales goes under the "Sales" group
				if (parts.Length > 1 && parts[0] == "api" && parts[1] == "Sales")
				{
					return new List<string> { "Sales" };
				}

				string controller = api.ActionDescriptor.RouteValues["controller"] ?? "Other";
				return new List<string> { controller };
			}

			void ConfigureSwagger(SwaggerGenOptions options)
			{
				// The Authorize button
				OpenApiSecurityScheme scheme = new OpenApiSecurityScheme();
				scheme.Type = SecuritySchemeType.Http;
				scheme.Scheme = "bearer";
				scheme.BearerFormat = "JWT";
				options.AddSecurityDefinition("Bearer", scheme);
				options.AddSecurityRequirement(CreateRequirement);

				// The Sales group
				options.TagActionsBy(GetTags);
			}

			// Add services to the container.

			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(ConfigureSwagger);
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