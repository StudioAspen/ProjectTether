using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Consystently 
{
  using Essentials;
  
  public class Catalog<T> : Singleton<Catalog<T>> where T : ScriptableObject
  {
    [Header("Data", order = 0)]
    [Space(10)]
    [Tooltip("Dataset containing every object of type T that exists in this Catalog.")]
    [SerializeField] protected List<T> objects = new List<T>();

    /// <summary>
    /// Dataset containing every object of type T that exists in this Catalog.
    /// </summary>
    public List<T> Objects => objects;

    protected override void Awake()
    {
      base.Awake();
      RemoveDuplicates();
    }

    /// <summary>
    /// Checks each object in the database for any repeating instances and removes them.
    /// </summary>
    private void RemoveDuplicates()
    {
      HashSet<T> uniqueObjects = new HashSet<T>();

      foreach (T obj in objects)
      {
        // Check if either the name or the serial number is unique
        if (!uniqueObjects.Contains(obj))
          uniqueObjects.Add(obj);
      }

      objects = uniqueObjects.ToList();
    }

    /// <summary>
    /// Rolls a random object instance  in the database.
    /// </summary>
    /// <returns>A Scriptable Object of type T.</returns>
    protected virtual T CreateRandom()
    {
      int index = Random.Range(0, objects.Count);
      T obj = objects[index];

      return Instantiate(obj);
    }

    // /// <summary>
    // /// Returns the item with the specified serial number in the catalog. Returns null if no such serial number is active.
    // /// </summary>
    // /// <param name="serialNumber">The item's serial number.</param>
    // /// <returns>An item from the catalog</returns>
    // public ItemData GetItemBySerialNumber(string serialNumber)
    // {
    //   ItemData itemToGet = itemDatabase.Find(item => serialNumber == item.SerialNumber);

    //   if (itemToGet != null)
    //     return itemToGet;
    //   else
    //     return null;
    // }

    // /// <summary>
    // /// Creates an instance of the item with the specified name in the catalog. Returns null if no such serial number is active.
    // /// </summary>
    // /// <param name="serialNumber">The item's serial number.</param>
    // /// <returns>An item object instance.</returns>
    // public ItemData CreateItemFromSerialNumber(string serialNumber)
    // {
    //   ItemData itemToCreate = itemDatabase.Find(item => item.SerialNumber == serialNumber);

    //   if (itemToCreate == null)
    //     return null;

    //   Debug.Log("Item of the serial number '" + itemToCreate.SerialNumber + "' created.");
    //   return Instantiate(itemToCreate);
    // }

    // /// <summary>
    // /// Creates an instance of the item with the specified name in the catalog. Returns null if no such item name exists.
    // /// </summary>
    // /// <param name="itemName">The item's name.</param>
    // /// <returns>An item object instance.</returns>
    // public ItemData CreateItemFromName(string itemName)
    // {
    //   ItemData itemToCreate = itemDatabase.Find(item => item.ItemName == itemName);

    //   if (itemToCreate == null)
    //     return null;

    //   Debug.Log("Item of the name '" + itemToCreate.ItemName + "' created.");
    //   return Instantiate(itemToCreate);
    // }
  }
}