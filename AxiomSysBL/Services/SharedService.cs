using AxiomSysBL.IServices;
using AxiomSysBL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AxiomSysBL.Services
{
	public class SharedService : ISharedService
	{
		public ResponseModel HandleException(Exception ex) => new()
		{
			status = false,
			messageAr = "حدث خطأ غير متوقع",
			messageEn = "An unexpected error occurred."
		};
	}
}
