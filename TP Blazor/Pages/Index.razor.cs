using TP_Blazor.Components;
using TP_Blazor.Models;

namespace TP_Blazor.Pages;

public partial class Index
{
   /* private Cake CakeItem = new Cake
    {
        Id = 1,
        Name = "Black Forest",
        Cost = 50
    };
    
    public List<Cake> Cakes { get; set; }

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        LoadCakes();
        StateHasChanged();
        return base.OnAfterRenderAsync(firstRender);
    }

    public void LoadCakes()
    {
        Cakes = new List<Cake>
        {
            new Cake
            {
                Id = 1,
                Name = "Red Velvet",
                Cost = 60
            },
            new Cake
            {
                Id = 2,
                Name = "Chocolate",
                Cost = 70
            },
            new Cake
            {
                Id = 3,
                Name = "Vanilla",
                Cost = 80
            },
            new Cake
            {
                Id = 4,
                Name = "Strawberry",
                Cost = 90
            },
            new Cake
            {
                Id = 5,
                Name = "Blueberry",
                Cost = 100
            }
        };
        
        
    }*/
   
   [Inject]
   public IDataService DataService { get; set; }

   public List<Item> Items { get; set; } = new List<Item>();

   private List<CraftingRecipe> Recipes { get; set; } = new List<CraftingRecipe>();

   protected override async Task OnAfterRenderAsync(bool firstRender)
   {
       base.OnAfterRenderAsync(firstRender);

       if (!firstRender)
       {
           return;
       }

       Items = await DataService.List(0, await DataService.Count());
       Recipes = await DataService.GetRecipes();

       StateHasChanged();
   }
}