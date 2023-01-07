using ShopBridgeInventory.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopBridgeInventory.Repository
{
    public interface IRepository
    {
        Task<List<InventoryData>> GetAllDetails();
    }
}
