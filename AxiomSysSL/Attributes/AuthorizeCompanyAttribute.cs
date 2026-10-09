using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AxiomSysSL.Attributes
{
	public class AuthorizeCompanyAttribute : Attribute, IAuthorizationFilter
	{
		public void OnAuthorization(AuthorizationFilterContext context)
		{
			var claim = context.HttpContext.User.FindFirst("CompanyCode");
			if (claim == null)
			{
				context.Result = new ForbidResult();
			}
		}
	}
}
