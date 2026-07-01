namespace SupplierManagement.Api.AI
{
    public enum SkillType
    {
        Revenue,
        Inventory,
        Unknown
    }
    public static class QuestionRouter
    {
        private static readonly string[] RevenueKeywords =
            { "revenue", "profit", "sales", "supplier performance" };
        private static readonly string[] InventoryKeywords =
            { "stock", "inventory", "restock","products","counts" };
        public static SkillType Route(string question)
        {
            if (string.IsNullOrWhiteSpace(question))
                return SkillType.Unknown;

            var q = question.ToLower();

            if (RevenueKeywords.Any(k => q.Contains(k)))
                return SkillType.Revenue;

            if (InventoryKeywords.Any(k => q.Contains(k)))
                return SkillType.Inventory;
            return SkillType.Unknown;
        }
    }
}