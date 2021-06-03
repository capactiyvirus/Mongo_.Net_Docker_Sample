using System;
using System.Collections.Generic;
using Catalog.Entities;
using System.Linq;


namespace Catalog.Repositories{
    

    public class InMemItemsRepository : IItemsRepository
    {
        private readonly List<Item> items = new()
        {
            new Item { Id = Guid.NewGuid(), Name = "Potion", Price = 9, CreatedDate = DateTimeOffset.UtcNow },
            new Item { Id = Guid.NewGuid(), Name = "Iron Sword", Price = 52, CreatedDate = DateTimeOffset.UtcNow },
            new Item { Id = Guid.NewGuid(), Name = "Bronze Shield", Price = 32, CreatedDate = DateTimeOffset.UtcNow }
        };

        public IEnumerable<Item> GetItems()
        {
            return items;
        }

        public Item GetItem(Guid id)
        {
            return items.Where(item => item.Id == id).SingleOrDefault();
        }

        public void CreateItem(Item item){
            items.Add(item);
            
            
            // //we can check to see if that item doesnt exist
            // var existCheck = items.Where(item => item.Name == name).SingleOrDefault();
            // Console.WriteLine(existCheck);

            // Item newItem = new Item {Id = Guid.NewGuid(), Name = name, Price = price, CreatedDate = createdDate};

            // items.Add(newItem);
            // return newItem;
            // //then if not we can add the item
            

        }

        public void UpdateItem(Item item){
            //find the item
            var index = items.FindIndex(existingItem => existingItem.Id == item.Id);
            items[index] = item;


            //update the item
            //items.Add(item);

        }

        public void DeleteItem(Guid id)
        {
            var index = items.FindIndex(existingItem => existingItem.Id == id);
            items.RemoveAt(index);
        }
    }
}