using System.Collections.Generic;
using _Scripts.Runtime.Combat.Mechanics;
using _Scripts.Runtime.Entities.Scripts.Combat;
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
    private List<UnitController> units; 

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

    //doesn't seem to work because everything is instant. The enemy performs their actions instantly, 
    //returning the turn to the player immediately.
    private void UpdatePanels(List<UnitController> uc)
    {
      units = uc;
      for (int i = 0; i < uc.Count; i++)
      {
        initiativeSlots[i].SetText(uc[i].GetData().Name);
        initiativeSlots[i].SetPortrait(uc[i].GetData().Portrait);
      }
    } 
  }
}
