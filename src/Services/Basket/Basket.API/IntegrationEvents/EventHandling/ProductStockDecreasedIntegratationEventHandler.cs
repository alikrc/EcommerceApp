using EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using Services.Basket.API.IntegrationEvents.Events;
using Services.Basket.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Basket.API.IntegrationEvents.EventHandling
{
    public class ProductStockDecreasedIntegratationEventHandler : IIntegrationEventHandler<ProductStockDecreasedIntegratationEvent>
    {
        private readonly ILogger<ProductStockDecreasedIntegratationEventHandler> _logger;
        private readonly IBasketRepository _repository;
        public ProductStockDecreasedIntegratationEventHandler(
            IBasketRepository repository,
            ILogger<ProductStockDecreasedIntegratationEventHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(ProductStockDecreasedIntegratationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

                var userIds = _repository.GetUsers();

                foreach (var id in userIds)
                {
                    var basket = await _repository.GetBasketAsync(id);

                    await UpdateStockInBasketItems(@event.ProductId, @event.NewStock, @event.OldStock, basket);
                }
            }
        }

        private async Task UpdateStockInBasketItems(int productId, int newStock, int oldStock, CustomerBasket basket)
        {
            var itemsToUpdate = basket?.Items?.Where(x => x.ProductId == productId).ToList();

            if (itemsToUpdate != null)
            {
                _logger.LogInformation($"----- {nameof(ProductStockDecreasedIntegratationEventHandler)} - Updating items in basket for user: {basket.BuyerId} ({ itemsToUpdate})");

                foreach (var item in itemsToUpdate)
                {
                    if (item.Quantity >= newStock)
                    {
                        item.Quantity = newStock;
                    }
                }
                await _repository.UpdateBasketAsync(basket);
            }
        }
    }
}
