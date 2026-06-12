namespace SupplierManagement.Web.Models;

public static class PaymentMethodsList
{
    public static List<string> All => new()
    {
        "UPI",
        "Net Banking",
        "Credit Card",
        "Debit Card",
        "Cash on Delivery",
        "Bank Transfer",
        "Wallet"
    };
}