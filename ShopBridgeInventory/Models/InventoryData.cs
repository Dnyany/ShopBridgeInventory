using System.ComponentModel.DataAnnotations;

namespace ShopBridgeInventory.Models
{
    public class InventoryData
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Inventory Name is Required")]
        public string InventoryName { get; set; }
        [Required(ErrorMessage = "Inventory Description is Required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Inventory Price is Required")]
        public decimal Price { get; set; }

    };
    public class dInventoryData
    {
        [Required(ErrorMessage = "Input ID is not Valid.")]
        public int ID { get; set; }
    };
    public class uInventoryData
    {
        [Required(ErrorMessage = "ID is Required")]
        public int ID { get; set; }

        [Required(ErrorMessage = "Inventory Name is Required")]
        public string InventoryName { get; set; }

        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
