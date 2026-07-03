
using SupplierManagement.Data.DTO;
namespace SupplierManagement.Business.Agent
{
    public class AgentContext
    {
        public string UserQuestion{get;set;}
        public List<SupplierRevenueDTO> RevenueData{get;set;}=new();
        public List<ProductInventoryDTO> InventoryData{get;set;}=new();
        public List<ProductInsightsDTO> CalculatedInsights{get;set;}=new();
        public List<OrderSummaryDTO> OrderData{get;set;}=new();
    }
}