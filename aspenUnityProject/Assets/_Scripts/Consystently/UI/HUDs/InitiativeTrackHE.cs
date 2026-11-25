using System.Collections.Generic;
using System.Globalization;
using _Scripts.Runtime.Combat.Mechanics;
using _Scripts.Runtime.Entities.Scripts.Combat;
using _Scripts.Runtime.Utilities;
using UnityEngine;

namespace _Scripts.Consystently.UI
{
  public class InitiativeTrackHE : HUDElement
  {
    [Header("Initiative Track")]
    [SerializeField] protected RectTransform _zeroPanel;
    [SerializeField] protected RectTransform _content;
    [SerializeField] protected InitiativeSlot _slotPrefab;

    public InitiativeSlot[] initiativeSlots { get; protected set; } = new InitiativeSlot[16];
    private PriorityQueue<TurnQueueItem, float> _unitTurns; 
    
    protected override void Start()
    {
      base.Start();
      for (int i = 0; i < initiativeSlots.Length; i++)
      {
        initiativeSlots[i] = Instantiate(_slotPrefab, i == 0 ? _zeroPanel : _content);
        initiativeSlots[i].Initialize(i);

        initiativeSlots[i].RTransform.anchorMin = Vector2.zero;
        initiativeSlots[i].RTransform.anchorMax = Vector2.one;
        initiativeSlots[i].RTransform.pivot = new Vector2(0.5f, 0.5f);
      }

      // Active Slot needs to stretch to be larger than the rest
      initiativeSlots[0].RTransform.sizeDelta = Vector2.zero;
      TurnQueue.OnShuffled += UpdatePanels;
    }

    protected virtual void OnDestroy() 
    {
     TurnQueue.OnShuffled -= UpdatePanels; 
    }

    private void UpdatePanels(SortedSet<TurnQueueItem> turnQueue)
    {
      int i = 0;
      foreach(TurnQueueItem tqi in turnQueue)
      {
        if (i > initiativeSlots.Length-1)
          break;
        initiativeSlots[i].SetText((tqi.Initiative*100).ToString(CultureInfo.CurrentCulture));
        initiativeSlots[i].SetPortrait(tqi.GetUnitData().Portrait);
        i++;
      }
    } 
  }
}
