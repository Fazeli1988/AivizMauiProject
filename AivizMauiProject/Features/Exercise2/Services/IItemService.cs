using AivizMauiProject.Features.Exercise2.Models;
using System.Collections.ObjectModel;

namespace AivizMauiProject.Features.Exercise2.Services
{
    public interface IItemService
    {
        List<ItemModel> GetAll();
        void Add(ItemModel item);
        void Update(ItemModel item);
        void Delete(ItemModel item);
    }
}
