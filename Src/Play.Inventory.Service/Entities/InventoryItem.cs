using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Play.Common;

namespace Play.Inventory.Service.Entities
{
    public class InventoryItem : IEntity
    {
        public Guid UserId { get; set; }
        public int quantity { get; set; }
        public Guid CatalogItemId { get; set; }

        public DateTimeOffset AcquiredDate { get; set; }
    }
}