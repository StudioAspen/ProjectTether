using UnityEngine;


namespace Consystently.UI
{
  [RequireComponent(typeof(CanvasGroup))]
  [DisallowMultipleComponent]
  public class HUDElement : MonoBehaviour
  {
    [Header("HUD Element")]
    [SerializeField] protected string hudName;
    public string Name => hudName;

    public HUD Hud { get; protected set; }

    void Awake()
    {
      // if (transform.root.gameObject.TryGetComponent(out HUD hud))
      // {
      //   Hud ??= hud;
      // }
    }
  }
}