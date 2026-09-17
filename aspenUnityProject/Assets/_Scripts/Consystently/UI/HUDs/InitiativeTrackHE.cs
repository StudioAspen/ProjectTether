using UnityEngine;

namespace Consystently.UI
{
  public class InitiativeTrackHE : HUDElement
  {
    [Header("Initiative Track")]
    [SerializeField] protected RectTransform _zeroPanel;
    [SerializeField] protected RectTransform _content;
    [SerializeField] protected InitiativeSlot _slotPrefab;

    public InitiativeSlot[] initiativeSlots { get; protected set; } = new InitiativeSlot[16];

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

      // Zero Slot needs to stretch to be larger than the rest
      initiativeSlots[0].RTransform.sizeDelta = Vector2.zero;
    }
  }
}
