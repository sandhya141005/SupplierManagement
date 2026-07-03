namespace SupplierManagement.Data.DTO
{
    public class ProductInventoryDTO
    {
        public string ProductName { get; set; }
        public string CompanyName { get; set; }
        public int StockQuantity { get; set; }
       public string Category{get;set;}
       public int TotalUnitsSold{get;set;}
    }
}