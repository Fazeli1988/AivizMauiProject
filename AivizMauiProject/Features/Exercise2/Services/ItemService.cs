using AivizMauiProject.Features.Exercise2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivizMauiProject.Features.Exercise2.Services
{
    public class ItemService : IItemService
    {
        private readonly List<ItemModel> _items = new();

        public ItemService()
        {
            // Initial data for display
            for (int i = 1; i <= 5; i++)
            {
                _items.Add(new ItemModel
                {
                    Title = $"Item {i}",
                    Description = $"Description {i}",
                    Date = DateTime.Now,
                    Image = "dotnet_bot.png"
                });
            }
        }

        public List<ItemModel> GetAll() => _items.ToList();

        public void Add(ItemModel item) => _items.Add(item);

        public void Update(ItemModel item)
        {
            
            var existing = _items.FirstOrDefault(i => ReferenceEquals(i, item));
            if (existing == null) return;

            existing.Title = item.Title;
            existing.Description = item.Description;
            existing.Date = item.Date;
            existing.Image = item.Image;
        }

        public void Delete(ItemModel item)
        {
            if (_items.Contains(item))
                _items.Remove(item);
        }
    }
}
