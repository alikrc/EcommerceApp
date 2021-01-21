
using EventBus.Events;

namespace Services.Basket.API.IntegrationEvents.Events
{
    public class ProductStockDecreasedIntegratationEvent : IntegrationEvent
    {
        public int ProductId { get; private set; }

        public int NewStock { get; private set; }

        public int OldStock { get; private set; }
        public ProductStockDecreasedIntegratationEvent(int productId, int newStock, int oldStock)
        {
            ProductId = productId;
            NewStock = newStock;
            OldStock = oldStock;
        }
    }
}
