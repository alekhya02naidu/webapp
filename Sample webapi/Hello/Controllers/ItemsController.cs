using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Hello.DB.Models;

namespace Hello.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly ItemsContext _itemContext;

        public ItemsController(ItemsContext itemContext)
        {
            _itemContext = itemContext;
        }

        [HttpGet]
        public IActionResult GetAllItems()
        {
            var items = _itemContext.Items
                            .Include(i => i.CostNavigation) 
                            .ToList();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public IActionResult GetItemById(int id)
        {
            var item = _itemContext.Items.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public IActionResult CreateItem([FromBody] DB.Models.Item item)
        {
            _itemContext.Items.Add(item);
            _itemContext.SaveChanges();
            return CreatedAtAction(nameof(GetItemById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateItem(int id, [FromBody] Item updatedItem)
        {
            var existingItem = _itemContext.Items.Find(id);
            if (existingItem == null)
            {
                return NotFound();
            }
            existingItem.Name = updatedItem.Name;
            existingItem.Description = updatedItem.Description;
            _itemContext.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteItem(int id)
        {
            var existingItem = _itemContext.Items.Find(id);
            if (existingItem == null)
            {
                return NotFound();
            }
            _itemContext.Remove(existingItem);
            _itemContext.SaveChanges();
            return NoContent();
        }
    }
}