using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
namespace SupplierManagement.Api.Email;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;
    private readonly ILogger<EmailService> _logger;
    public EmailService(IOptions<EmailSettings> settings, ILogger<EmailService> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }
    public async Task SendOrderConfirmationAsync(string toEmail, string userName,
        string orderNumber, decimal totalAmount, DateTime orderDate)
    {
        try
        {
            var msg = new MimeMessage();
            msg.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            msg.To.Add(new MailboxAddress(userName, toEmail));
            msg.Subject = $"***Order Confirmed -- {orderNumber} | Supplier Hub***";
            msg.Body = new TextPart("plain")
            {
                Text = $@"
                Dear {userName},

                Your order has been confirmed.
                Order ID: {orderNumber}
                Amount: Rs.{totalAmount}

                Happy Shopping :)
                "
            };
            using var client = new SmtpClient();
            client.ServerCertificateValidationCallback =
    (s, c, h, e) => true;
    Console.WriteLine("Before connect");
            await client.ConnectAsync(_settings.SmtpHost, _settings.SmtpPort, SecureSocketOptions.Auto);
            Console.WriteLine("after connect");
            Console.WriteLine("Before auth");
            await client.AuthenticateAsync(_settings.SenderEmail, _settings.Password);
            Console.WriteLine("after auth");
            await client.SendAsync(msg);
            await client.DisconnectAsync(true);
            _logger.LogInformation("Order confirmation email sent to {Email} for order number {OrderNumber}", toEmail, orderNumber);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Email} for order {OrderNumber}",
                toEmail, orderNumber);
        }
    }
}