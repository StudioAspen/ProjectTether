using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Consystently
{
  using Sieves;
  
  namespace Inventories
  {
    [DisallowMultipleComponent]
    public class Inventory : MonoBehaviour
    {
      [Header("Inventory")]
      [Tooltip("The inventory's name.")]
      [SerializeField] protected string inventoryName = "Inventory";

      [Tooltip("If the inventory needs to be prepopulated with items, add them to this list. Add multiple copies of an item to the list if the inventory requires more than one of an item.")]
      [SerializeField] protected List<ItemData> itemsSupply;

      private Dictionary<ItemData, Stock> inventoryItems = new Dictionary<ItemData, Stock>();

      public const int MaxQuantity = 9;

      /// <summary>
      /// The inventory's name.
      /// </summary>
      public string InventoryName => inventoryName;

      /// <summary>
      /// The items in this inventory and their respective quantities.
      /// </summary>
      public Dictionary<ItemData, Stock> InventoryItems => inventoryItems;

      protected virtual void Awake()
      {
        // Prepopulates inventory with any items in the itemsToPrepopulate list, then removes the item from that list to show what was not added.
        List<ItemData> itemsNotAdded = new List<ItemData>();

        foreach(ItemData item in itemsSupply)
        {
          AddItem(item, 1, out bool successful);

          if (!successful)
            itemsNotAdded.Add(item);
        }

        itemsSupply = itemsNotAdded;
      }

      /// <summary>
      /// Adds an item to the inventory at the specified quantity.
      /// </summary>
      /// <param name="item">The item to add.</param>
      /// <param name="quantity">The quantity of the item to add.</param>
      /// <param name="successful">Returns true if the item at the specified quantity was successfully added.</param>
      public virtual void AddItem(ItemData item, int quantity, out bool successful)
      {   
        if (inventoryItems.ContainsKey(item))
        {   
          // If the player is trying to add more copies of the item than the remaining available space, reject the request.
          int availableSpace = MaxQuantity - inventoryItems[item].Value;
          if(quantity > availableSpace)
          {
            Debug.LogError("You are trying to add a quantity of an item that exceeds its maximum quantity. Addition of item canceled.");
            successful = false;
            return;
          }
          
          // If the inventory already contains a copy of the item, we only want to adjust the quantity of that item.
          inventoryItems[item].AdjustCurrentValue(quantity);
        }
        else
        {
          // If the player is trying to add more copies of the item than the max quantity allowed, reject the request.
          if(quantity > MaxQuantity)
          {
            Debug.LogError("You are trying to add a quantity of an item that exceeds its maximum quantity. Addition of item canceled.");
            successful = false;
            return;
          }

          // If the inventory does not contain a copy of the item, we're going to add that item and set its quantity.
          inventoryItems.Add(item, new Stock(item.ItemName, quantity, 0, MaxQuantity));
        }

        successful = true;
      }

      /// <summary>
      /// Gets the quantity of an item in the inventory if it exists.
      /// </summary>
      /// <param name="item">The item to find the quantity of.</param>
      /// <returns>The item's quantity.</returns>
      public int QuantityOf(ItemData item)
      {
          if (inventoryItems.ContainsKey(item))
              return inventoryItems[item].Value;
          else
              return 0;
      }

      /// <summary>
      /// Removes an item from the inventory at the specified quantity if it exists.
      /// </summary>
      /// <param name="item">The item to remove.</param>
      /// <param name="quantity">The quantity of the item to remove.</param>
      /// <param name="successful">Returns true if the item at the specified quantity was successfully removed.</param>
      public virtual void RemoveItem(ItemData item, int quantity, out bool approved)
      {
        if (inventoryItems.ContainsKey(item))
        { 
          // If the player is trying to remove more copies of the item than the remaining quantity, reject the request.
          if(quantity > inventoryItems[item].Value)
          {
            Debug.LogError("You are trying to remove a quantity of an item that exceeds its current quantity. Removal of item canceled.");
            approved = false;
            return;
          }

          // If the player is requesting to remove a quantity that is less than the remaining available quantity, we only want to adjust the item's quantity.
          inventoryItems[item].AdjustCurrentValue(-Mathf.Abs(quantity));

          // If the item's quantity is 0, we remove it's entry from the inventory.
          if (inventoryItems[item].Value == 0)
            inventoryItems.Remove(item);
        }
        else
        {
          // If the player is trying to remove an item that isn't present in the inventory, reject the request.
          Debug.LogError("You are trying to remove a quantity of an item that is not present in this inventory. Removal of item canceled.");
          approved = false;
          return;
        }

        approved = true;
      }

      /// <summary>
      /// Sorts the inventory based on the specified sort type.
      /// </summary>
      /// <param name="sortType">The assortment type.</param>
      /// <returns>A sorted copy of the inventory.</return>
      public Dictionary<ItemData, Stock> Sort(Assortment sortType)
      {
        Dictionary<ItemData, Stock> sortedInventory;

        switch (sortType)
        {
          case Assortment.AtoZ:

            // Sort the inventory alphabetically by item names from A to Z
            sortedInventory = inventoryItems.OrderBy(x => x.Key.ItemName).ToDictionary(x => x.Key, x => x.Value);

            break;

          case Assortment.ZtoA:

            // Sort the dictionary alphabetically by item names from Z to A
            sortedInventory = inventoryItems.OrderByDescending(x => x.Key.ItemName).ToDictionary(x => x.Key, x => x.Value);

            break;

          case Assortment.LowestQuantity:

            // Sort the inventory from lowest quantity to highest quantity
            sortedInventory = inventoryItems.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            break;

          case Assortment.HighestQuantity:

            // Sort the inventory from highest quantity to lowest quantity
            sortedInventory = inventoryItems.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            break;

          default:

            sortedInventory = inventoryItems;

            break;
        }

        return sortedInventory;
      }

      /// <summary>
      /// Filters the inventory based on the specified category.
      /// </summary>
      /// <param name="category">The category filter.</param>
      /// <returns>A filtered copy of the inventory.</returns>
      public Dictionary<ItemData, Stock> FilterByCategory(Category category)
      {
        Dictionary<ItemData, Stock> filteredInventory = inventoryItems.Where(x => x.Key.ItemCategory == category).ToDictionary(x => x.Key, x => x.Value);

        return filteredInventory;
      }

      /// <summary>
      /// Filters the inventory based on the specified rarity.
      /// </summary>
      /// <param name="rarity">The rarity filter.</param>
      /// <returns>A filtered copy of the inventory.</returns>
      public Dictionary<ItemData, Stock> FilterByRarity(Rarity rarity)
      {
        Dictionary<ItemData, Stock> filteredInventory = inventoryItems.Where(x => x.Key.ItemRarity == rarity).ToDictionary(x => x.Key, x => x.Value);

        return filteredInventory;
      }
    }
  }
}