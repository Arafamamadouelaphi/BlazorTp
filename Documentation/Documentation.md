# Documentation for our Inventory page


<br/><br/>
## First part : The inventory grid (InventoryItem.razor.cs file)
<br/><br/>


###  internal void OnDragEnter()
#### Summary:
This method will create an action when the user will put an item in the grid. 
<br/>

###  internal void OnDragEnter()
#### Summary:
This method will create an action when the user will remove an item of the grid. 
<br/>

###  internal void OnDrop()
#### Summary:
This method will create an action when the user will put an item in onother one on the grid. 
<br/>

###  internal void OnDragEnd()
#### Summary:
This method will create an action when the user will drag and drop an item in the grid. 
<br/>

###  private void OnDragStart()
#### Summary:
This method will create an action when the user will start the drag and drop of an item in the grid. 
<br/>


<br/><br/>
## Second part : The inventory grid (MyInventory.razor.cs file)
<br/><br/>


###  public MyInventory()
#### Summary:
This method is the constructor of the component. It initializes the variables for the inventory grid part.
<br/>

### private async void OnActionsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
#### Parameters: 
- sender is the sender of the event
- e is a class of event arguments
#### Summary:
This method is called when our collection 'Actions' is changed.
It will call the Js script, and if needed, the dataService.


<br/><br/>
## Last part : The items list
<br/><br/>


### private async Task OnReadData(DataGridReadDataEventArgs<Item> e)
#### Parameters: 
- e is a class of event arguments
#### Summary:
This method will initialize a lot of variables used for the list.
<br/>

### private List<Item> choseList()
#### Returns: 
- a list of items corresponding to what we want
#### Summary:
This method will check if our search bar is empty. 
If it is, it will set our Datagrid list to the default one: items
Else, it will take the texte in the Search bar, and search in all the items, those that correspond.
<br/>

### private void inputValue()
#### Summary:
When we type something in the search bar, this method is called.
It will initialize the 'Filtered' list with the items corresponding to what we are typing.
Then, it will call the choseList() method.
<br/>

### private void OnPress()
#### Summary:
This method will be called when we will press the 'Sort' button.
It will set a boolean either to true if the list isn't already sorted, or to false if it is.
Then, it will call the SortList() method.
<br/>

### private List<Item> SortList()
#### Returns: 
- a list of items corresponding to what we want
#### Summary:
This method will check if our isSorted boolean is false or true.
If it's false, then it will set our list to the default one: the items won't be sorted.
Else, it will set it with another list, that we sort by name.
<br/>