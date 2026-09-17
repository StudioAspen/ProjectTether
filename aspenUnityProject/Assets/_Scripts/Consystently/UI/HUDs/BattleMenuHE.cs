using UnityEngine;

namespace Consystently.UI
{
  public class BattleMenuHE : HUDElement
  {
    [Header("Battle Menu")]
    [SerializeField] private MeshFilter focusedEntity;
    protected GameMenu _menu;

    protected void Awake()
    {
      _menu ??= GetComponent<GameMenu>();
    }

    protected override void Start()
    {
      base.Start();
      Debug.Log(focusedEntity.mesh.bounds.size);
      
    }

    public void Update()
    {
      if (Input.GetKeyDown(KeyCode.Space))
        MenuManager.Instance.OpenMenu(_menu);
    }
  }
}
