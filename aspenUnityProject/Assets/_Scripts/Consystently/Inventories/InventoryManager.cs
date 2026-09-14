namespace Consystently.Inventories
{
  using UnityEngine;
  using Essentials;

  public class InventoryManager : Manager<InventoryManager>
  {
    /// <summary>
    /// Transfers an item at a quantity from one inventory (source) to another (target).
    /// </summary>
    /// <param name="sourceInventory">The source inventory.</param>
    /// <param name="targetInventory">The target inventory.</param>
    /// <param name="item">The item to be transferred.</param>
    /// <param name="quantity">The quantity of the item being transferred.</param>
    /// <returns>A boolean value determined by whether the transfer was successful.</returns>
    public bool TransferItem (Inventory sourceInventory, Inventory targetInventory, ItemData item, int quantity)
    {
      // Check if the source inventory contains the item in the required quantity
      if (sourceInventory.QuantityOf(item) < quantity)
      {
        Debug.LogError("Source inventory does not have enough of the item to transfer.");
        return false;
      }

      // Attempt to add the item to the target inventory
      targetInventory.AddItem(item, quantity, out bool addSuccessful);

      if (addSuccessful)
      {
        // Remove the item from the source inventory if successfully added to the target
        sourceInventory.RemoveItem(item, quantity, out bool removeSuccessful);

        if (removeSuccessful)
        {
          Debug.Log("Item(s) successfully transferred.");
          return true;
        }
        else
        {
          // If removal failed, roll back the addition
          targetInventory.RemoveItem(item, quantity, out _);
          Debug.LogError("Failed to remove item from source inventory.");
          return false;
        }
      }
      else
      {
        Debug.LogError("Target inventory cannot accommodate the item.");
        return false;
      }
    }

    /// <summary>
    /// Trades 2 items at respective quantities between two inventories.
    /// </summary>
    /// <param name="inventory1">The first inventory.</param>
    /// <param name="item1">The item from the first inventory being traded.</param>
    /// <param name="quantity1">The quantity of the first inventory's item being traded.</param>
    /// <param name="inventory2">The second inventory.</param>
    /// <param name="item2">The item from the second inventory being traded.</param>
    /// <param name="quantity2">The quantity of the second inventory's item being traded.</param>
    /// <returns>A boolean value determined by whether the trade was successful.</returns>
    public bool TradeItems(Inventory inventory1, ItemData item1, int quantity1, Inventory inventory2, ItemData item2, int quantity2)
    {
      // Ensure both inventories have the required quantities of the items to trade
      if (inventory1.QuantityOf(item1) < quantity1)
      {
        Debug.LogError($"{inventory1.InventoryName} does not have enough of {item1.ItemName} to trade.");
        return false;
      }

      if (inventory2.QuantityOf(item2) < quantity2)
      {
        Debug.LogError($"{inventory2.InventoryName} does not have enough of {item2.ItemName} to trade.");
        return false;
      }

      // Attempt to add the items to their respective target inventories
      inventory1.AddItem(item2, quantity2, out bool addSuccessful1);
      inventory2.AddItem(item1, quantity1, out bool addSuccessful2);

      if (addSuccessful1 && addSuccessful2)
      {
        // Remove the traded items from their respective source inventories
        inventory1.RemoveItem(item1, quantity1, out bool removeSuccessful1);
        inventory2.RemoveItem(item2, quantity2, out bool removeSuccessful2);

        if (removeSuccessful1 && removeSuccessful2)
        {
          Debug.Log("Trade successful.");
          return true;
        }
        else
        {
          // Rollback in case of failure
          if (removeSuccessful1) 
              inventory1.AddItem(item1, quantity1, out _);
          if (removeSuccessful2) 
              inventory2.AddItem(item2, quantity2, out _);

          Debug.LogError("Failed to remove items from source inventories.");
          return false;
        }
      }
      else
      {
        // Rollback in case of failure
        if (addSuccessful1) 
          inventory1.RemoveItem(item2, quantity2, out _);
        if (addSuccessful2) 
          inventory2.RemoveItem(item1, quantity1, out _);

        Debug.LogError("Failed to add items to target inventories.");
        return false;
      }
    }

    /// <summary>
    /// Transacts an item at a quantity from a seller's inventory to a buyer's inventory for a price that the buyer pays.
    /// </summary>
    /// <param name="sellingInventory">The seller's inventory.</param>
    /// <param name="buyingInventory">The buyer's inventory.</param>
    /// <param name="item">The item to be transacted.</param>
    /// <param name="quantity">The quantity of the item being transacted.</param>
    /// <param name="currency">The buyer's current available currency.</param>
    /// <returns>A boolean value determined by whether the transaction was successful.</returns>
    public bool TransactItem(Inventory sellingInventory, Inventory buyingInventory, ItemData item, int quantity, ref int currency)
    {
      int totalCost = item.Price.Value * quantity;

      // Check if the selling inventory contains the item in the required quantity.
      if (sellingInventory.QuantityOf(item) >= quantity)
      {
        // Check if the buyer has enough currency.
        if (currency >= totalCost)
        {
          // Remove the item from the selling inventory.
          sellingInventory.RemoveItem(item, quantity, out bool removeSuccessful);

          if (removeSuccessful)
          {
            // Add the item to the buying inventory.
            buyingInventory.AddItem(item, quantity, out bool addSuccessful);

            if (addSuccessful)
            {
              // Adjust the currency.
              currency -= totalCost;

              Debug.Log("Item(s) successfully transacted.");
              return true;
            }
            else
            {
              // If adding to the buying inventory fails, roll back the removal.
              sellingInventory.AddItem(item, quantity, out _);
              Debug.LogError("Failed to add item to buying inventory.");
              return false;
            }
          }
          else
          {
            Debug.LogError("Failed to remove item from selling inventory.");
            return false;
          }
        }
        else
        {
          Debug.LogError("Buyer does not have enough currency.");
          return false;
        }
      }
      else
      {
        Debug.LogError("Selling inventory does not have enough of the item to sell.");
        return false;
      }
    }
  }
}