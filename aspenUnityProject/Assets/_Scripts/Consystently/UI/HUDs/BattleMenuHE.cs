using _Scripts.Runtime.Combat;
using _Scripts.Runtime.UI;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Consystently.UI
{
  public class BattleMenuHE : HUDElement
  {
    [Header("Battle Menu")]
    [SerializeField] private MeshFilter focusedEntity;
    protected GameMenu _menu;

    [Header("Action Ring Buttons")]
    [SerializeField] private Button _attackButton;
    [SerializeField] private Button _defendButton;
    [SerializeField] private Button _moveButton;

    protected void Awake()
    {
      _menu ??= GetComponent<GameMenu>();
    }

    protected override void Start()
    {
      base.Start();
      if (focusedEntity != null && focusedEntity.mesh != null)
        Debug.Log(focusedEntity.mesh.bounds.size);
      
      if (_attackButton != null) 
        _attackButton.onClick.AddListener(() => OnActionButtonClicked(CombatActions.Attack));

      if (_defendButton != null)
        _defendButton.onClick.AddListener(() => OnActionButtonClicked(CombatActions.Defend));

      if (_moveButton != null)
        _moveButton.onClick.AddListener(() => OnActionButtonClicked(CombatActions.Move));
    }

    private void OnActionButtonClicked(CombatActions action)
    {
      Debug.Log($"[BattleMenuHE] Player confirmed action: {action}");

      Hide();

      CombatUI.PlayerAction?.Invoke(action);
    }

    protected virtual void OnDestroy() {

      if (_attackButton != null) _attackButton.onClick.RemoveAllListeners();
      if (_defendButton != null) _defendButton.onClick.RemoveAllListeners();
      if (_moveButton != null) _moveButton.onClick.RemoveAllListeners();
    }
    public void Update()
    {
      if (Input.GetKeyDown(KeyCode.Space))
        MenuManager.Instance.OpenMenu(_menu);
    }

  }
}
