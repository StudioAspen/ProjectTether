using System;
using UnityEngine;

namespace Consystently
{
  using Sieves;
  namespace Inventories
  {
    [CreateAssetMenu(fileName = "New Item Data", menuName = "Scriptable Objects/Inventories/Item Data")]
    public class ItemData : ScriptableObject
    {
      [Tooltip("The item's serial number.")]
      [SerializeField] private string serialNumber;

      [Tooltip("The item's name.")]
      [SerializeField] private string itemName;

      [TextArea]
      [Tooltip("The item's description.")]
      [SerializeField] private string description;

      [Tooltip("The category that the item belongs to.")]
      [SerializeField] private Category category;

      [Tooltip("The item's rarity.")]
      [SerializeField] private Rarity rarity;
      
      [Tooltip("The item's display icon.")]
      [SerializeField] private Sprite icon;

      [Range(0, 999999)]
      [Tooltip("The item's base price.")]
      [SerializeField] private int price;

      /// <summary>
      /// The item's serial number.
      /// </summary>
      public string SerialNumber => serialNumber;

      /// <summary>
      /// The item's name.
      /// </summary>
      public string ItemName => itemName;

      /// <summary>
      /// The item's description.
      /// </summary>
      public string Description => description;

      /// <summary>
      /// The category that the item belongs to.
      /// </summary>
      public Category ItemCategory => category;

      /// <summary>
      /// The item's rarity.
      /// </summary>
      public Rarity ItemRarity => rarity;

      /// <summary>
      /// The item's display icon.
      /// </summary>
      public Sprite Icon => icon;

      /// <summary>
      /// The item's base price.
      /// </summary>
      public Stock Price => new Stock($"{itemName}'s Price", price, 0, 999999);
    }
  }
}