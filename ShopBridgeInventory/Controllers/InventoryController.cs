using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopBridgeInventory.Managers;
using ShopBridgeInventory.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopBridgeInventory.Controllers
{
    [ApiController]
    [Route("api/Inventory")]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryManager inventoryManager;
        public InventoryController(InventoryManager param)
        {
            inventoryManager = param ?? throw new ArgumentNullException(nameof(param));
        }

        [HttpGet]
        [HttpGet("InventoryList")]//api/Inventory/InventoryList
        public async Task<ActionResult<List<InventoryData>>> getInventory()
        {
            return await inventoryManager.GetAllDetails();
        }

        [HttpGet("{Id:int}")]
        [HttpGet("InventoryList/{Id:int}")]//api/Inventory/InventoryList/1
        public async Task<ActionResult<List<InventoryData>>> getInventory(int Id)
        {
            return await inventoryManager.GetDetailsByID(Id);
        }

        [Authorize]
        [HttpPost]
        [HttpPost("add")]
        public async Task<ActionResult> addInventory(InventoryData param)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(await inventoryManager.AddInventory(param));
        }

        [Authorize]
        [HttpPut]
        [HttpPut("update")]
        public async Task<ActionResult> updateInventory(uInventoryData param)
        {
            if (!ModelState.IsValid || param.ID == 0)
            {
                return BadRequest();
            }
            return Ok(await inventoryManager.UpdateInventory(param));
        }

        [Authorize]
        [HttpDelete]
        [HttpDelete("delete")]
        public async Task<ActionResult> deleteInventory(dInventoryData param)
        {
            if (!ModelState.IsValid || param.ID == 0)
            {
                return BadRequest();
            }
            return Ok(await inventoryManager.DeleteInventory(param));
        }
    }
}
