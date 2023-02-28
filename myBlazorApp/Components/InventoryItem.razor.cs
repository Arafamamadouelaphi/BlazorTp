/// IMPORTS ///
using System;
using System.Reflection.Metadata;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using myBlazorApp.Models;

namespace myBlazorApp.Components;

public partial class InventoryItem
{
    /// -------- PARAMETERS -------- ///

    /// <summary>
    /// gets or sets the item name 
    /// </summary>
    [Parameter]
    public string Item { get; set; }

    /// <summary>
    /// gets or sets the index 
    /// </summary>
    [Parameter]
    public int Index { get; set; }

    /// <summary>
    /// gets or sets the number of items
    /// </summary>
    [Parameter]
    public int Number { get; set; }

    /// <summary>
    /// gets or sets the stack size 
    /// </summary>
    [Parameter]
    public int StackSize { get; set; }

    /// <summary>
    /// gets or sets the capability to drop the item
    /// </summary>
    [Parameter]
    public bool NoDrop { get; set; }

    /// <summary>
    /// gets or sets the image of the item 
    /// </summary>
    [Parameter]
    public string Image { get; set; }


    /// -------- DEPENDENCIES INJECTION -------- ///

    /// <summary>
    /// Gets or sets the localizer
    /// </summary>
    [Inject]
    public IStringLocalizer<InventoryItem> Localizer { get; set; }


    /// -------- CASCADING PARAMETER -------- ///

    /// <summary>
    /// Gets or sets the cascading parameter Parent ( MyInventory ).
    /// </summary>
    [CascadingParameter]
    public MyInventory Parent { get; set; }


    /// -------- DRAG AND DROP METHODS -------- ///

    /// <summary>
    /// Creating a new action when the user enter in an item of the Inventory items
    /// </summary>
    internal void OnDragEnter()
    {
        if (NoDrop)
        {
            return;
        }
        Parent.Actions.Add(new InventoryAction { Action = "Drag Enter", ItemName = this.Item, Index = this.Index, Number = this.Number, StackSize = this.StackSize });
    }

    /// <summary>
    /// Creating a new action when the user removes an item of the Inventory items
    /// </summary>
    internal void OnDragLeave()
    {
        if (NoDrop)
        {
            return;
        }
        Parent.Actions.Add(new InventoryAction { Action = "Drag Leave", ItemName = this.Item, Index = this.Index, Number = this.Number, StackSize = this.StackSize });
    }

    /// <summary>
    /// Creating a new action when the user drops an item in an Inventory item
    /// </summary>
    internal void OnDrop()
    {
        if (Parent.CurrentDragItem != "null")
        {
            /// Case when the item comes from the list of items
            if(Parent.CurrentDragIndex == 0)
            {
                if (this.Item == Parent.CurrentDragItem)
                {
                    this.Number = this.Number + Parent.CurrentDragNumber;
                }
                else
                {
                    this.Number = Parent.CurrentDragNumber;
                }
                this.Item = Parent.CurrentDragItem;
                this.StackSize = Parent.CurrentDragStackSize;
                Parent.ItemsInventory[this.Index - 1] = new InventoryListItem(this.Item, this.Index, this.Number, this.StackSize);
            }
            /// Case when the item dropped is the same than the item inside the Inventory item
            else if (this.Item == Parent.CurrentDragItem)
            {
                this.Number = this.Number + Parent.CurrentDragNumber;

                Parent.CurrentInventoryItem.Number = 0;
                Parent.CurrentInventoryItem.Item = "null";
                Parent.CurrentInventoryItem.StackSize = 0;

                Parent.ItemsInventory[Parent.CurrentInventoryItem.Index - 1] = new InventoryListItem(Parent.CurrentInventoryItem.Item, Parent.CurrentInventoryItem.Index, Parent.CurrentInventoryItem.Number, Parent.CurrentInventoryItem.StackSize);
            }
            /// Case when the item must be exchanged with another or just be inserted.
            else
            {
                string tmpItem = this.Item;
                int tmpNumber = this.Number;
                int tmpStackSize = this.StackSize;

                Parent.ItemsInventory[Parent.CurrentDragIndex - 1] = new InventoryListItem(tmpItem, Parent.CurrentDragIndex, tmpNumber, tmpStackSize);

                this.Item = Parent.CurrentDragItem;
                this.Number = Parent.CurrentDragNumber;
                this.StackSize = Parent.CurrentDragStackSize;

                Parent.CurrentInventoryItem.Item = tmpItem;
                Parent.CurrentInventoryItem.Number = tmpNumber;
                Parent.CurrentInventoryItem.StackSize = tmpStackSize;
            }

        }
        Parent.Actions.Add(new InventoryAction { Action = "Drop", ItemName = this.Item, Index = this.Index, Number = this.Number, StackSize = this.StackSize });
        
    }

    /// <summary>
    /// Creating a new action when the user end the drag and dropped the item in an Inventory item
    /// </summary>
    internal void OnDragEnd()
    {
        if(NoDrop || Parent.CurrentDragItem == "null")
        {
            return;
        }

        if (Parent.CurrentDragIndex != this.Index)
        {
            Parent.ItemsInventory[this.Index-1] = new InventoryListItem(Parent.CurrentDragItem, Parent.CurrentDragIndex, Parent.CurrentDragNumber, Parent.CurrentDragStackSize);
            this.Item = Parent.CurrentDragItem;
            this.Number = Parent.CurrentDragNumber;
            this.StackSize = Parent.CurrentDragStackSize;
        }

        Parent.Actions.Add(new InventoryAction { Action = "End", ItemName = this.Item, Index = this.Index, Number = this.Number, StackSize = this.StackSize });
    }


    /// <summary>
    /// Creating a new action when the user starts dragging an item
    /// </summary>
    private void OnDragStart()
    {
        Parent.CurrentInventoryItem = this;
        Parent.CurrentDragIndex = this.Index;
        Parent.CurrentDragItem = this.Item;
        Parent.CurrentDragNumber = this.Number;
        Parent.CurrentDragStackSize = this.StackSize;

        // remove the element if it's not in the list of items
        if (this.Index != 0)
        {
            this.Item = "null";
            this.Number = 0;
            this.StackSize = 0;
        }
        
        Parent.Actions.Add(new InventoryAction { Action = "Drag Start", ItemName = this.Item, Index = this.Index, Number = this.Number, StackSize = this.StackSize});

    }
}

