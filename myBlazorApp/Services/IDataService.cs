using System;
using myBlazorApp.Components;
using myBlazorApp.Models;

namespace myBlazorApp.Services
{
    public interface IDataService
    {
        Task Add(ItemModel model);
        Task<int> Count();
        Task<List<Item>> List(int currentPage, int pageSize);
        Task<Item> GetById(int id);
        Task Update(int id, ItemModel model);
        Task Delete(int id);
        Task<List<CraftingRecipe>> GetRecipes();
        Task<List<InventoryListItem>> GetInventoryItems();
        Task UpdateInventory(string[] infos);
    }
}

