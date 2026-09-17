using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Consystently.UI
{
  [DisallowMultipleComponent]
  public class InitiativeSlot : MonoBehaviour
  {
    [Range(0, 19)]
    public int TrackPlacement = 0;
    public TextMeshProUGUI PlacementText { get; private set; }
    public RectTransform RTransform { get; private set; }

    public void Initialize(int i)
    {
      PlacementText ??= GetComponentInChildren<TextMeshProUGUI>();
      RTransform ??= GetComponent<RectTransform>();
      
      TrackPlacement = i;
      PlacementText.maxVisibleCharacters = 2;
      PlacementText.text = i.ToString();
    }
  }
}
