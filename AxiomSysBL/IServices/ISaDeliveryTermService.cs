using AxiomSysBL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AxiomSysBL.IServices
{
	public interface ISaDeliveryTermService
	{
		List<SaDeliveryTermModel> GetAll();
		SaDeliveryTermModel? GetByCode(int code);
		ResponseModel Add(SaDeliveryTermModel model, int userId);
		ResponseModel Edit(SaDeliveryTermModel model, int userId);
		ResponseModel Delete(int code);
	}
}
