namespace AxiomSysPortal.Models
{
	public class DeliveryTermModel
	{
		public int Code { get; set; }
		public string SName { get; set; } = "";
		public string BName { get; set; } = "";
		public int? Days { get; set; }
		public int? ActiveFlag { get; set; }
	}
}
