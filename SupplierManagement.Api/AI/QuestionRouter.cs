namespace SupplierManagement.Api.AI
{
    public enum SkillType
    {
        Revenue,
        Inventory,
        Order,
        Unknown
    }
    public static class QuestionRouter
    {
        private static readonly string[] RevenueKeywords =
            { " Supplier","revenue", "profit", "sales", "supplier performance" };
        private static readonly string[] InventoryKeywords =
            { "stock", "inventory", "products","counts" };
        private static readonly string[] OrderKeywords =
{ "order", "orders", "purchase", "purchases", "bought", "demand","restock"};
        public static SkillType Route(string question)
        {
            if (string.IsNullOrWhiteSpace(question))
                return SkillType.Unknown;

            var q = question.ToLower();

            if (RevenueKeywords.Any(k => q.Contains(k)))
                return SkillType.Revenue;

            if (InventoryKeywords.Any(k => q.Contains(k)))
                return SkillType.Inventory;
            if (OrderKeywords.Any(k => q.Contains(k)))
                return SkillType.Order;
            return SkillType.Unknown;
        }
    }
}