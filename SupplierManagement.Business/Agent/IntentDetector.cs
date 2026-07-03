namespace SupplierManagement.Business.Agent
{
    public enum SkillSet
    {
        None, InventoryOnly, RevenueOnly, OrderOnly, InventoryAndRevenue, InventoryAndOrder, RevenueAndOrder, All
    }
    public static class IntentDetector
    {
        private static readonly string[] InventoryKeywords =
            { "stock", "inventory", "restock", "reorder", "out of stock",
              "stockout", "slow", "moving", "attention", "risk", "available" };

        private static readonly string[] RevenueKeywords =
            { "revenue", "profit", "sales", "performance", "earning",
              "income", "money", "losing", "margin", "contribute" };

        private static readonly string[] OrderKeywords =
            { "order", "orders", "purchase", "demand", "bought",
              "fulfill", "frequent", "priority", "customer" };

        private static readonly string[] AllKeywords =
            { "today", "focus", "biggest", "risk", "operational",
              "recommend", "action", "improve", "business" };
        public static SkillSet Detect(string question)
        {
            if (string.IsNullOrWhiteSpace(question)) return SkillSet.None;
            var q = question.ToLower();
            bool needsInventory = InventoryKeywords.Any(k => q.Contains(k));
            bool needsRevenue = RevenueKeywords.Any(k => q.Contains(k));
            bool needsOrder = OrderKeywords.Any(k => q.Contains(k));
            bool needsAll = AllKeywords.Any(k => q.Contains(k));
            if (needsInventory) return SkillSet.InventoryOnly;
            if (needsRevenue) return SkillSet.RevenueOnly;
            if (needsOrder) return SkillSet.OrderOnly;
            if (needsAll || needsInventory && needsRevenue && needsOrder) return SkillSet.All;
            if (needsInventory && needsRevenue) return SkillSet.InventoryAndRevenue;
            if (needsInventory && needsOrder) return SkillSet.InventoryAndOrder;
            if (needsRevenue && needsOrder) return SkillSet.RevenueAndOrder;
            return SkillSet.All;

        }
    }

}