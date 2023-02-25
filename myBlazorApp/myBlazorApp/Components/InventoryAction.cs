using System;
using myBlazorApp.Models;
namespace myBlazorApp.Components
{
	public class InventoryAction
    {
        /// <summary>
        /// Gets and sets the action
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Gets and sets the inde where the action affected
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets and sets the item name the action affected
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// Gets and sets the number of items the action affected
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Gets and sets the stack size of the item the action affected
        /// </summary>
        public int StackSize { get; set; }
    }
}

