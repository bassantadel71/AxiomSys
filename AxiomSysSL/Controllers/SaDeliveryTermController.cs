using AxiomSysBL.IServices;
using AxiomSysBL.Models;
using AxiomSysSL.Attributes;
using AxiomSysSL.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AxiomSysSL.Controllers
{
	[Authorize]
	[Route("api/Sales/[controller]")]
	[ApiController]
	[AuthorizeCompany]
	public class SaDeliveryTermController : ControllerBase
	{
		private readonly ISaDeliveryTermService _service;
		private readonly ICurrentUserService _currentUserService;

		public SaDeliveryTermController(ISaDeliveryTermService service, ICurrentUserService currentUserService)
		{
			_service = service;
			_currentUserService = currentUserService;
		}
		[HttpGet("GetAll")]
		public IActionResult GetAll()
		{
			List<SaDeliveryTermModel> list = _service.GetAll();
			return Ok(list);
		}

		[HttpGet("GetByCode")]
		public IActionResult GetByCode(int code)
		{
			SaDeliveryTermModel? item = _service.GetByCode(code);
			if (item == null)
			{
				return NotFound();
			}
			return Ok(item);
		}
		[HttpPost("AddSaDeliveryTerm")]
		public IActionResult AddSaDeliveryTerm(SaDeliveryTermModel model)
		{
			int userId = _currentUserService.UserId ?? 0;
			if (userId == 0)
			{
				return Unauthorized();
			}

			ResponseModel result = _service.Add(model, userId);
			return Ok(result);
		}

		[HttpPost("EditSaDeliveryTerm")]
		public IActionResult EditSaDeliveryTerm(SaDeliveryTermModel model)
		{
			int userId = _currentUserService.UserId ?? 0;
			if (userId == 0)
			{
				return Unauthorized();
			}

			ResponseModel result = _service.Edit(model, userId);
			return Ok(result);
		}

		[HttpPost("DeleteSaDeliveryTerm")]
		public IActionResult DeleteSaDeliveryTerm(int code)
		{
			int userId = _currentUserService.UserId ?? 0;
			if (userId == 0)
			{
				return Unauthorized();
			}

			ResponseModel result = _service.Delete(code);
			return Ok(result);
		}
	}
}
