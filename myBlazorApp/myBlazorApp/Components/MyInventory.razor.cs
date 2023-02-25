/// IMPORTS ///
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Blazorise.DataGrid;
using Blazorise.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using myBlazorApp.Models;
using myBlazorApp.Pages;
using myBlazorApp.Services;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Grids;

namespace myBlazorApp.Components
{
    public partial class MyInventory
    {
        /// -------- INVENTORY PART -------- ///
       
        /// <summary>
        /// Get or Set the list of items of the inventory.
        /// </summary>
        [Parameter]
        public List<InventoryListItem> ItemsInventory { get; set; } = new List<InventoryListItem>();

        /// <summary>
        /// Get or Set the list of actions the user made.
        /// </summary>
        public ObservableCollection<InventoryAction> Actions { get; set; }

        /// <summary>
        /// Get or Set the item's name the user is dragging.
        /// </summary>
        public string CurrentDragItem { get; set; }

        /// <summary>
        /// Get or Set the item's index the user is dragging.
        /// </summary>
        public int CurrentDragIndex { get; set; }

        /// <summary>
        /// Get or Set the number of items the user is dragging.
        /// </summary>
        public int CurrentDragNumber { get; set; }

        /// <summary>
        /// Get or Set the item's stack size the user is dragging.
        /// </summary>
        public int CurrentDragStackSize { get; set; }

        /// <summary>
        /// Get or Set the InvntoryItem the user is dragging.
        /// </summary>
        public InventoryItem CurrentInventoryItem {get; set;} = new InventoryItem();

        ///-------- LIST PART -------- ///
        
        /*
         * items: my default list, containing alll the items printed 10 by 10
         * full: another list, loading all the items on one page
         * currentPage: current page of the list
         * pageSize: numper of items on the page
         * choix: list binded on the datagrid that will change when needed
         * isSorted: boolean turning true when we click on sort
         * searchText: String binded on the search bar input
         * totalItem : total number of items in the list
        */

        private List<Item> items = new List<Item>();

        private List<Item> full = new List<Item>();

        private List<Item> choix = new List<Item>();

        public string SearchText = "";

        private int totalItem;

        private int currentPage;

        private int pageSize;

        List<Item> Filtered = new List<Item>();

        List<Item> Sorted = new List<Item>();

        /// -------- DEPENDENCIES INJECTION -------- ///

        /// <summary>
        /// gets or sets the data service ( API or Local Stroage )
        /// </summary>
        [Inject]
        public IDataService DataService { get; set; }

        [Inject]
        public IWebHostEnvironment WebHostEnvironment { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private bool isSorted = false;

        [Inject]
        public IStringLocalizer<MyInventory> Localizer { get; set; }


        [Inject]
        internal IJSRuntime JavaScriptRuntime { get; set; }


        /// -------- METHODS -------- ///

        /// <summary>
        /// Constructor of the component.
        /// </summary>
        public MyInventory()
        {
            /// Creating new Actions list
            Actions = new ObservableCollection<InventoryAction>();
            /// subscribing the event "changement in the collection Actions" to the method OnActionsCollectionChanged.
            Actions.CollectionChanged += OnActionsCollectionChanged;
        }

        /// <summary>
        /// Method called when the collection "Actions" changes.
        /// Invoke the javascript script and call the API to save the inventory's data
        /// </summary>
        /// <param name="sender"> sender of the event </param>
        /// <param name="e"> event arguments </param>
        private async void OnActionsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            /// Invoke the js script
            JavaScriptRuntime.InvokeVoidAsync("MyInventory.AddActions", e.NewItems);

            /// call the dataservice if the action is "end", "drop" or "start"
            if (this.Actions.Last().Action == "End" || this.Actions.Last().Action == "Drop" || this.Actions.Last().Action == "Start")
            {
                string[] infos = new string[] { this.Actions.Last().ItemName, this.Actions.Last().Index.ToString(), this.Actions.Last().Number.ToString(), this.Actions.Last().StackSize.ToString() };
                await DataService.UpdateInventory(infos);
            }
        }

        /// <summary>
        /// Read data from the data service to display them in the inventory and in the data grid
        /// Set up all the variables depending on what we want.
        /// </summary>
        /// <param name="e"> Datagrid of items </param>
        /// <returns> Task </returns>
        private async Task OnReadData(DataGridReadDataEventArgs<Item> e)
        {

            if (e.CancellationToken.IsCancellationRequested)
            {
                return;
            }

            if (!e.CancellationToken.IsCancellationRequested)
            {
                /// Read the items of the inventory
                ItemsInventory = await DataService.GetInventoryItems();
                totalItem = await DataService.Count();
                items = await DataService.List(e.Page, e.PageSize);
                full = await DataService.List(e.Page, totalItem);
                currentPage = e.Page;
                pageSize = e.PageSize;
                if (isSorted == false)
                {
                    choix = items;
                }
                else
                {
                    choix = SortList();
                    return;
                }
                if (SearchText.IsNullOrEmpty())
                {
                    choix = items;
                }
                else
                {
                    choix = choseList();
                    return;
                }
                StateHasChanged();
            }
        }


        /*
         * Name: choseList
         * Function: change the list choix when we type something in the search bar
        */
        private List<Item> choseList()
        {
            // If the search bar is empty, we don't need to use a special list, we use the default one
            if (SearchText.IsNullOrEmpty())
            {
                choix = items;
                totalItem = full.Count();
                NavigationManager.NavigateTo("inventory", false);
            }

            // Else, we change the list choix and we filter the items using the Filtered list
            else
            {
                // This line allows us to adapt pageSize depending on the number of items on it. 
                // If there is only 7 items, we don't want to print 10 on our page
                if (Filtered.Count() < (currentPage - 1) * 10 + pageSize)
                {
                    pageSize = Filtered.Count() - (currentPage - 1) * 10;
                }
                choix = Filtered.GetRange((currentPage - 1) * 10, pageSize);
                totalItem = Filtered.Count();
            }
            StateHasChanged();
            NavigationManager.NavigateTo("inventory", false);
            return choix;
        }

        /*
         * Name: inputValue
         * Function: When we type on the search bar, this function is called to initialize the Filtered list and call our function
        */
        private void inputValue()
        {
            // We filter the items with the search bar if their name contain what we are typing
            Filtered = full.Where(
               itm => itm.DisplayName.ToLower().Contains(SearchText.ToLower())).ToList();
            choseList();
        }


        /*
         * Name: OnPress
         * Function: This function is binded on the Sort button. It changes our boolean value and call our SortList function
        */
        private void OnPress()
        {
            if (isSorted == true)
            {
                isSorted = false;
            }
            else isSorted = true;
            SortList();
        }


        /*
         * Name: SortList
         * Function: When we press the sort button, this function will hange the items in the list choix
        */
        private List<Item> SortList()
        {
            // If our boolean is false, it means that the list was sorted and we pressed the button to unsort it.
            // So, it takes the default list for our dataGrid
            // The navigateTo allows to be on the first page of the list when we unsort it
            if (isSorted == false)
            {
                choix = items;
                NavigationManager.NavigateTo("inventory", true);
            }

            // Else, we put the full list into the Sorted one, and we sort the items by name. 
            // We check for the number of items in the page and we give the Sorted list to choix with the pageSize we want
            else
            {
                if (Sorted.IsNullOrEmpty())
                {
                    Sorted = full;
                }
                Sorted.Sort((x, y) => string.Compare(x.DisplayName, y.DisplayName));
                if (Sorted.Count() < (currentPage - 1) * 10 + pageSize)
                {
                    pageSize = Sorted.Count() - (currentPage - 1) * 10;
                }
                choix = Sorted.GetRange((currentPage - 1) * 10, pageSize);
            }
            return choix;
        }
    }
}

