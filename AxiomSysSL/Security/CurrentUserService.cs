namespace AxiomSysSL.Security;

public class CurrentUserService : ICurrentUserService
{
	private readonly IHttpContextAccessor _http;

	public CurrentUserService(IHttpContextAccessor http)
	{
		_http = http;
	}

	public int? UserId
	{
		get
		{
			// 1. Get the current request
			var request = _http.HttpContext;
			if (request == null)
			{
				return null;
			}

			// 2. Look inside the token for the item named "Id"
			var claim = request.User.FindFirst("Id");
			if (claim == null)
			{
				return null;
			}

			// 3. The token stores text, so turn it into a number
			int id;
			bool worked = int.TryParse(claim.Value, out id);
			if (worked)
			{
				return id;
			}

			return null;
		}
	}
}