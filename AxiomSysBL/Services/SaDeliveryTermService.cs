using AxiomSysBL.IServices;

using AxiomSysBL.Models;

using AxiomSysDL.Context;
using AxiomSysDL.Models.SupplyChain;
using AxiomSysDL.Repository;

namespace AxiomSysBL.Services.SalesService;

public class SaDeliveryTermService : ISaDeliveryTermService
{
	private readonly IUnitOfWork<SupplyChainContext> _uowSC;
	private readonly ISharedService _sharedService;

	public SaDeliveryTermService(IUnitOfWork<SupplyChainContext> uowSC, ISharedService sharedService)
	{
		_uowSC = uowSC;
		_sharedService = sharedService;
	}

	// Turns a database row into the model we send out
	private SaDeliveryTermModel ToModel(SaDeliveryTerm row)
	{
		SaDeliveryTermModel model = new SaDeliveryTermModel();
		model.Code = row.Code;
		model.SName = row.SName;
		model.BName = row.BName;
		model.Days = row.Days;
		model.ActiveFlag = row.ActiveFlag;
		return model;
	}

	// Finds one row by code, or returns null if it does not exist
	private SaDeliveryTerm? FindRow(int code)
	{
		var repo = _uowSC.GetRepository<SaDeliveryTerm>();

		var query = from t in repo.GetAll()
					where t.Code == code
					select t;

		return query.FirstOrDefault();
	}

	private ResponseModel NotFoundResponse()
	{
		ResponseModel response = new ResponseModel();
		response.status = false;
		response.messageAr = "السجل غير موجود";
		response.messageEn = "Record not found.";
		return response;
	}

	public List<SaDeliveryTermModel> GetAll()
	{
		var repo = _uowSC.GetRepository<SaDeliveryTerm>();

		var query = from t in repo.GetAll()
					orderby t.Code
					select t;

		List<SaDeliveryTermModel> result = new List<SaDeliveryTermModel>();
		foreach (var row in query.ToList())
		{
			result.Add(ToModel(row));
		}
		return result;
	}

	public SaDeliveryTermModel? GetByCode(int code)
	{
		var row = FindRow(code);
		if (row == null)
		{
			return null;
		}
		return ToModel(row);
	}

	public ResponseModel Add(SaDeliveryTermModel model, int userId)
	{
		try
		{
			// 1. Check: does this code already exist?
			var existing = FindRow(model.Code);
			if (existing != null)
			{
				ResponseModel duplicate = new ResponseModel();
				duplicate.status = false;
				duplicate.messageAr = "الكود موجود مسبقاً";
				duplicate.messageEn = "Code already exists.";
				return duplicate;
			}

			// 2. Do the work: build the new row and stamp the audit fields
			SaDeliveryTerm row = new SaDeliveryTerm();
			row.Code = model.Code;
			row.SName = model.SName;
			row.BName = model.BName;
			row.Days = model.Days;
			row.ActiveFlag = model.ActiveFlag ?? 1;
			row.EntryUser = userId;
			row.EntryDate = DateTime.Now;

			// 3. Save
			_uowSC.GetRepository<SaDeliveryTerm>().Add(row);
			_uowSC.Commit();

			// 4. Return a message
			ResponseModel ok = new ResponseModel();
			ok.status = true;
			ok.data = row.Code;
			ok.messageAr = "تمت الإضافة بنجاح";
			ok.messageEn = "Added successfully.";
			return ok;
		}
		catch (Exception ex)
		{
			return _sharedService.HandleException(ex);
		}
	}

	public ResponseModel Edit(SaDeliveryTermModel model, int userId)
	{
		try
		{
			var row = FindRow(model.Code);
			if (row == null)
			{
				return NotFoundResponse();
			}

			row.SName = model.SName;
			row.BName = model.BName;
			row.Days = model.Days;
			row.ActiveFlag = model.ActiveFlag;
			row.ChangeUser = userId;
			row.ChangeDate = DateTime.Now;

			_uowSC.Commit();

			ResponseModel ok = new ResponseModel();
			ok.status = true;
			ok.data = row.Code;
			ok.messageAr = "تم التعديل بنجاح";
			ok.messageEn = "Updated successfully.";
			return ok;
		}
		catch (Exception ex)
		{
			return _sharedService.HandleException(ex);
		}
	}

	public ResponseModel Delete(int code)
	{
		try
		{
			var row = FindRow(code);
			if (row == null)
			{
				return NotFoundResponse();
			}

			_uowSC.GetRepository<SaDeliveryTerm>().Remove(row);
			_uowSC.Commit();

			ResponseModel ok = new ResponseModel();
			ok.status = true;
			ok.messageAr = "تم الحذف بنجاح";
			ok.messageEn = "Deleted successfully.";
			return ok;
		}
		catch (Exception ex)
		{
			return _sharedService.HandleException(ex);
		}
	}
}