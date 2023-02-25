using System;
using System.Xml.Linq;
using myBlazorApp.Models;
namespace myBlazorApp.Components
{
	public class InventoryListItem
	{
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="itemName"> name of the item </param>
        /// <param name="position"> position of the item in the Inventory </param>
        /// <param name="number"> number of items at a certain position </param>
        /// <param name="stackSize"> max number of the same item at one position </param>
        public InventoryListItem(string itemName, int position, int number, int stackSize)
        {
            this.itemName = itemName;
            this.position = position;
            this.number = number;
            this.stackSize = stackSize;
        }

        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        public string itemName { get; set; }

        /// <summary>
        /// Gets and sets the position of the item in the Inventory.
        /// </summary>
        public int position { get; set; }

        /// <summary>
        /// Gets and sets the number of items at a certain position.
        /// </summary>
        public int number { get; set; }

        /// <summary>
        /// Gets and sets the max number of the same item at one position.
        /// </summary>
        public int stackSize { get; set; }
	}
	
}

