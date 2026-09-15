using System;
using UnityEngine;

namespace Consystently
{
  namespace Inventories
  {
    public class Stock
    {
      private string stockName;
      private int value;
      private int minValue;
      private int maxValue;

      /// <summary>
      ///  Invokes when the stock's [value, minValue, or maxValue] adjusts. (int oldValue, int newValue, int adjustment)
      /// </summary>
      public event Action<int, int, int> onValueAdjusted, onMinValueAdjusted, onMaxValueAdjusted;

      /// <summary>
      ///  Invokes when the stock's [value, minValue, or maxValue] updates. (int oldValue, int newValue)
      /// </summary>
      public event Action<int, int> onValueUpdated, onMinValueUpdated, onMaxValueUpdated;

      /// <summary>
      /// The stock's name.
      /// </summary>
      public string StockName => stockName;

      /// <summary>
      /// The stock's current value.
      /// </summary>
      public int Value => value;

      /// <summary>
      /// The stock's minimum value.
      /// </summary>
      public int MinValue => minValue;

      /// <summary>
      /// The stock's maximum value.
      /// </summary>
      public int MaxValue => maxValue;

      /// <summary>
      /// Constructor for a stock that automatically determines all settings based on the provided stock value.
      /// </summary>
      /// <param name="stockName">The stock's name.</param>
      /// <param name="value">The stock's initial value.</param>
      public Stock(string stockName, int value)
      {
        this.stockName = stockName;
        this.value = value < 0 ? 0 : value;

        // The minimum and maximum value are automatically set.
        this.minValue = 0;
        this.maxValue = int.MaxValue;

        Regulate();
      }

      /// <summary>
      /// Constructor for a stock that automatically determines settings based on the provided values.
      /// </summary>
      /// <param name="stockName">The stock's name.</param>
      /// <param name="value">The stock's current value.</param>
      /// <param name="minValue">The stock's minimum value.</param>
      /// <param name="maxValue">The stock's maximum value.</param>
      public Stock(string stockName, int value, int minValue, int maxValue)
      {
        this.stockName = stockName;
        this.value = value;
        this.minValue = minValue;
        this.maxValue = maxValue;

        Regulate();
      }

      /// <summary>
      /// Regulates the stock's values to ensure that its rules are being followed.
      /// </summary>
      public void Regulate()
      {
        // Debug.Log($"({stockName}) Before Regulation: minValue = {minValue}, maxValue = {maxValue}, value = {value}");

        if (this.minValue < 0) 
            this.minValue = 0;

        if (this.maxValue < 1) 
            this.maxValue = 1;

        // Clamp minValue and maxValue.
        minValue = Mathf.Clamp(minValue, 0, int.MaxValue);
        maxValue = Mathf.Clamp(maxValue, 1, int.MaxValue);

        // Ensure maxValue is not less than minValue.
        if (maxValue < minValue)
            maxValue = minValue;

        // Clamp value between minValue and maxValue.
        value = Mathf.Clamp(value, minValue, maxValue);

        // Debug.Log($"({stockName}) After Regulation: minValue={minValue}, maxValue={maxValue}, value={value}");
      }

      /// <summary>
      /// Adds/Subtracts a number to/from the current minimum value.
      /// </summary>
      /// <param name="adjustment">The number to be added/subtracted.</param>
      public void AdjustMinValue(int adjustment)
      {
        int oldMinValue = minValue;

        minValue += adjustment;
        Regulate();

        onMinValueAdjusted?.Invoke(oldMinValue, minValue, minValue - adjustment);
      }

      /// <summary>
      /// Replaces the current minimum value with a new one.
      /// </summary>
      /// <param name="newMinValue">The new minimum value.</param>
      public void SetMinValue(int newMinValue)
      {
        int oldMinValue = minValue;

        minValue = newMinValue;
        Regulate();

        onMinValueUpdated?.Invoke(oldMinValue, minValue);
      }

      /// <summary>
      /// Adds/Subtracts a number to/from the current maximum value.
      /// </summary>
      /// <param name="adjustment">The number to be added/subtracted.</param>
      public void AdjustMaxValue(int adjustment)
      {
        int oldMaxValue = maxValue;

        maxValue += adjustment; 
        Regulate();

        onMaxValueAdjusted?.Invoke(oldMaxValue, maxValue, maxValue - adjustment);
      }

      /// <summary>
      /// Replaces the current maximum value with a new one.
      /// </summary>
      /// <param name="newMaxValue">The new maximum value.</param>
      public void SetMaxValue(int newMaxValue)
      { 
        int oldMaxValue = maxValue;

        maxValue = newMaxValue;
        Regulate();

        onMaxValueUpdated?.Invoke(oldMaxValue, maxValue);
      }

      /// <summary>
      /// Adds/Subtracts a number to/from the current stock value.
      /// </summary>
      /// <param name="adjustment">The number to be added/subtracted.</param>
      public void AdjustCurrentValue(int adjustment)
      {
        int oldValue = value;

        value += adjustment;
        Regulate();

        onValueAdjusted?.Invoke(oldValue, value, value - adjustment);
      }

      /// <summary>
      /// Replaces the current stock value with a new one.
      /// </summary>
      /// <param name="newCurrentValue">The new stock value.</param>
      public void SetCurrentValue(int newCurrentValue)
      {
        int oldValue = value;

        value = newCurrentValue;
        Regulate();

        onValueUpdated?.Invoke(oldValue, value);
      }
    }
  }
}
