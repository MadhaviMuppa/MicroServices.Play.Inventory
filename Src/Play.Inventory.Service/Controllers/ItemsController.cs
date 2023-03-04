using Microsoft.AspNetCore.Mvc;
using Play.Common;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.Entities;
using static Play.Inventory.Service.Dtos;

namespace Play.Inventory.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<InventoryItem> _repository;
        private readonly CatalogClient _catalogClient;
        public ItemsController(CatalogClient catalogClient, IRepository<InventoryItem> repository)
        {
            _catalogClient = catalogClient;
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAllAsync(Guid UserId)
        {
            if (UserId == Guid.Empty)
            {
                return BadRequest();
            }

            var catalogItems = await _catalogClient.GetCatalogItemsAsync();
            var inventoryItems = await _repository.GetAllAsync(item => item.UserId == UserId);
            var items = inventoryItems.Select(item =>
            {
                var catalogItem = catalogItems.Single(citem => citem.Id == item.CatalogItemId);
                return item.AsDto(catalogItem.Name, catalogItem.Description);
            });

            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(GrantItemsDto item)
        {
            var inventoryItems = await _repository.GetAsync(i => i.CatalogItemId == item.CatalogItemId && i.UserId == item.UserId);
            if (inventoryItems == null)
            {
                inventoryItems = new InventoryItem
                {
                    UserId = item.UserId,
                    CatalogItemId = item.CatalogItemId,
                    quantity = item.quantity,
                    AcquiredDate = DateTimeOffset.UtcNow
                };
                await _repository.CreateAsync(inventoryItems);
            }
            else
            {
                inventoryItems.quantity += item.quantity;
                await _repository.UpdateAsync(inventoryItems);

            }

            return Ok();
        }
    }
}