namespace SupplierManagement.Web.Models.Revenue
{
   
    public class RevenueInsightsModel
    {
        public List<SupplierRevenueModel> TopSuppliers { get; set; } = new();
        public string AiAnswer{ get; set; }
        public string ErrorMessage { get; set; }
        public string Question { get; set; }
    }
}