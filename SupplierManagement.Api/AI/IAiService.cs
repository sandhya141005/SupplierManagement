namespace SupplierManagement.Api.AI
{
    public interface IAiService
    {
        Task<String> GetCompletionAsync(string userMessage);
    }
}