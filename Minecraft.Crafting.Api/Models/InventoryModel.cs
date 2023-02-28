// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryController.cs" company="UCA Clermont-Ferrand">
//     Copyright (c) UCA Clermont-Ferrand All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Minecraft.Crafting.Api.Models
{
    /// <summary>
    /// The inventory model.
    /// </summary>
    public class InventoryModel
    {
        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets the number of items in a stack.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets the stack size of an item.
        /// </summary>
        public int StackSize { get; set; }
    }
}