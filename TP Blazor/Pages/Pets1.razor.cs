namespace TP_Blazor.Pages;

public partial class Pets1
{
    private List<Pet> pets = new()
    {
        new Pet { PetId = 2, Name = "Mr. Bigglesworth" },
        new Pet { PetId = 4, Name = "Salem Saberhagen" },
        new Pet { PetId = 7, Name = "K-9" }
    };

    private class Pet
    {
        public int PetId { get; set; }
        public string? Name { get; set; }
    }
}