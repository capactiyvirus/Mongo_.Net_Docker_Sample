using System.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using Catalog.Dtos;
using Catalog.Entities;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;



namespace Catalog.Controllers
{

    // GET/items

    [ApiController]
    [Route("/api/items")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository repository; // calls the private class

        public ItemsController(IItemsRepository repository){
            //repository = new InMemItemsRepository(); // //creats a new instance of the repo of items we are using " this is bad since the id are gonna be differnt then what we see on swagger "
            this.repository = repository;
            //Rather than doing the above we need to use Dependency injection.
        }

        
        [HttpGet] // for the routing 
        public IEnumerable<ItemDto> GetItems(){
            var items = repository.GetItems().Select( item => item.AsDto());
            return items;
        }

        [HttpGet("{id}")] // routing and shows that we will be using a specific id 
        public ActionResult <ItemDto> GetItem(Guid id){ // we added the ActionResult to allow multiple types to be returned other than item object. so we can return the NotFound object which will be the 404 code and else the item
            var item = repository.GetItem(id);

            if(item is null){
                return NotFound(); //404 not found
            }


            return item.AsDto();
        }

        // POST /api/items
        [HttpPost]
        public ActionResult <ItemDto> CreateItem(CreateItemDto itemDto){
            //Item item = new Item {Id = Guid.NewGuid(), Name = name, Price = price, CreatedDate = createdDate};
            
            //creates new items based on the itemDto
            Item item = new(){
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow,
            };

            

            repository.CreateItem(item);

            return CreatedAtAction(nameof(GetItem), new {id = item.Id}, item.AsDto());
            

        }

        [HttpPut("{id}")]
        public ActionResult <ItemDto> UpdateItem(Guid id, UpdateItemDto itemDto){
            //Item item = new Item {Id = Guid.NewGuid(), Name = name, Price = price, CreatedDate = createdDate};

            //verify that item exists
            var existingItem = repository.GetItem(id);

            //returns not found if item is not found 
            if (existingItem is null) return NotFound();

            //create new instance of item with parameters taht we want to update so in this case id is the same items id, and we have a new name and price
            // in this case rather then writing like this to create a new instance of the item object
            // Item item = new(){
            //     Id = id,
            //     Name = itemDto.Name,
            //     Price = itemDto.Price,
            //     CreatedDate = DateTimeOffset.UtcNow,
            // };
            // we can do it like this
            Item updatedItem = existingItem with{
                Name = itemDto.Name,
                Price = itemDto.Price
            };

            

            repository.UpdateItem(updatedItem);
            
            return NoContent();
            //return CreatedAtAction(nameof(GetItem), new {id = item.Id}, item.AsDto());
            

        }
        [HttpDelete("{id}")]
        public ActionResult <ItemDto> DeleteItem(Guid id){
            var existingItem = repository.GetItem(id);
            if (existingItem is null) return NotFound();

            repository.DeleteItem(id);

            return NoContent();
        }

        

    }
}