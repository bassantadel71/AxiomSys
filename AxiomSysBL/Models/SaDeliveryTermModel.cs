using System;
using System.Collections.Generic;
using System.Text;

namespace AxiomSysBL.Models
{
	public class SaDeliveryTermModel
	{
		public int Code { get; set; }
		public string SName { get; set; } = "";
		public string BName { get; set; } = "";
		public int? Days { get; set; }
		public int? ActiveFlag { get; set; }
	}
}
