namespace AxiomSysPortal.Models
{
	public class ApiResponse
	{
		public bool Status { get; set; }
		public object? Data { get; set; }
		public string MessageAr { get; set; } = "";
		public string MessageEn { get; set; } = "";
	}
}
