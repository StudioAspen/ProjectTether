using UnityEngine;

namespace Consystently.UI
{
  [RequireComponent(typeof(CanvasGroup))]
  [DisallowMultipleComponent]
  public abstract class HUD : MonoBehaviour
  {
    [Header("HUD")]
    [SerializeField] protected string hudName;
    public string Name => hudName;

    
  }
}
