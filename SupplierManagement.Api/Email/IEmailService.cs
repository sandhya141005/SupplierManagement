namespace SupplierManagement.Api.Email;
public interface IEmailService
{
    Task SendOrderConfirmationAsync(string toEmail, string userName,
        string orderNumber, decimal totalAmount, DateTime orderDate);
}