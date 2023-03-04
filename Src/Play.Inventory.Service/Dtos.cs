using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Inventory.Service
{
    public class Dtos
    {
        public record GrantItemsDto(Guid UserId, Guid CatalogItemId, int quantity);
        public record InventoryItemDto(Guid CatalogItemId, string Name, string Description, int quantity, DateTimeOffset AcquiredDate);
        public record CatalogItemDto(Guid Id, string Name, string Description);
    }
}