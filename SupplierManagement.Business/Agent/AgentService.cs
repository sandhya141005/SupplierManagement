using SupplierManagement.Api.AI;
using SupplierManagement.Business.Interfaces;
using SupplierManagement.Data.DTO;

namespace SupplierManagement.Business.Agent
{
    public interface IAgentService
    {
        Task<string> AskAsync(string question);
    }

    public class AgentService : IAgentService
    {
        private readonly IRevenueSkill _revenueSkill;
        private readonly IInventorySkill _inventorySkill;
        private readonly IOrderSkill _orderSkill;
        private readonly IAiService _aiService;

        public AgentService(
            IRevenueSkill revenueSkill,
            IInventorySkill inventorySkill,
            IOrderSkill orderSkill,
            IAiService aiService)
        {
            _revenueSkill = revenueSkill;
            _inventorySkill = inventorySkill;
            _orderSkill = orderSkill;
            _aiService = aiService;
        }

        public async Task<string> AskAsync(string question)
        {
            var skillSet = IntentDetector.Detect(question);
            var ctx = new AgentContext { UserQuestion = question };
            bool fetchInventory = skillSet is SkillSet.InventoryOnly
                or SkillSet.InventoryAndRevenue
                or SkillSet.InventoryAndOrder
                or SkillSet.All;

            bool fetchRevenue = skillSet is SkillSet.RevenueOnly
                or SkillSet.InventoryAndRevenue
                or SkillSet.RevenueAndOrder
                or SkillSet.All;

            bool fetchOrder = skillSet is SkillSet.OrderOnly
                or SkillSet.InventoryAndOrder
                or SkillSet.RevenueAndOrder
                or SkillSet.All;

            var tasks = new List<Task>();
            Task<List<ProductInventoryDTO>> inventoryTask = null;
            Task<List<SupplierRevenueDTO>> revenueTask = null;
            Task<List<OrderSummaryDTO>> orderTask = null;

            if (fetchInventory) inventoryTask = _inventorySkill.GetStocksAsync();
            if (fetchRevenue) revenueTask = _revenueSkill.GetSuppliersAsync();
            if (fetchOrder) orderTask = _orderSkill.GetOrderSummaryAsync();
            if (inventoryTask != null) tasks.Add(inventoryTask);
            if (revenueTask != null) tasks.Add(revenueTask);
            if (orderTask != null) tasks.Add(orderTask);
            await Task.WhenAll(tasks);
            if (inventoryTask != null) ctx.InventoryData = await inventoryTask;
            if (revenueTask != null) ctx.RevenueData = await revenueTask;
            if (orderTask != null) ctx.OrderData = await orderTask;
           /* foreach (var p in ctx.InventoryData)
            {
                Console.WriteLine($"{p.ProductName} {p.TotalUnitsSold}");
            }*/
            if (ctx.InventoryData.Any())

                ctx.CalculatedInsights = BusinessCalc.Calculate(ctx.InventoryData, ctx.OrderData);

            if (!ctx.InventoryData.Any() && !ctx.RevenueData.Any() && !ctx.OrderData.Any())
                return "No business data is available to answer your question.";

            var prompt = AgentPromptBuild.Build(ctx);
            return await _aiService.GetCompletionAsync(prompt);
        }
    }
}