using System;
using myBlazorApp.Models;

namespace myBlazorApp.Components
{
    public class CraftingRecipe
    {
        public Item Give { get; set; }
        public List<List<string>> Have{ get; set; }
    }
}

