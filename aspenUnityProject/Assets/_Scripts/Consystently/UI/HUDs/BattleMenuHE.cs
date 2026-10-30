using System;
using _Scripts.Runtime.Combat;
using _Scripts.Runtime.Combat.States.CombatManagerStates;
using _Scripts.Runtime.Entities.Scripts.Combat;
using _Scripts.Runtime.Managers;
using _Scripts.Runtime.Tile_System.Scripts;
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

    [SerializeField] private GameObject cursor;
    private bool _designerMode;
    
    [Header("Action Ring Buttons")]
    [SerializeField] private Button _attackButton;
    [SerializeField] private Button _defendButton;
    [SerializeField] private Button _moveButton;


    public static event Action<CombatActions> PlayerAction;
    public static event Action<CombatActions, int> PlayerSelectiveAction;
    public static event Action<bool> ToggledManualEnemy; 
    
    protected void Awake()
    {
      _menu ??= GetComponent<GameMenu>();
    }
    
    protected override void Start()
    {
      base.Start();
      CombatManager.battlePhaseChanged += HandleUserActions;
      CombatManager.examinedTile += ExamineTile;
      CombatManager.exitedExamine += ExitExamine;
      CombatManager.examinedUnit += ExamineUnit; 
    
      if (focusedEntity != null && focusedEntity.mesh != null)
        Debug.Log(focusedEntity.mesh.bounds.size);
      
      if (_attackButton != null) 
        _attackButton.onClick.AddListener(() => OnActionButtonClicked(CombatActions.Attack));

      if (_defendButton != null)
        _defendButton.onClick.AddListener(() => OnActionButtonClicked(CombatActions.Defend));

      if (_moveButton != null)
        _moveButton.onClick.AddListener(() => OnActionButtonClicked(CombatActions.Move));

      /*
      if (abilityButtons == null || abilityButtons.Length == 0)
      {
        abilityButtons = abilitiesPanel.GetComponentsInChildren<Button>();
        for (int ability = 0; ability < abilityButtons.Length; ability++)
        {
          int i = ability;
          abilityButtons[ability].onClick.RemoveAllListeners();
          abilityButtons[ability].onClick.AddListener(() => SendSelectedAction(3, i));
        }

      }
      */
    }
    
    protected virtual void OnDestroy() {
      CombatManager.battlePhaseChanged += HandleUserActions;
      CombatManager.examinedTile += ExamineTile;
      CombatManager.exitedExamine += ExitExamine;
      CombatManager.examinedUnit += ExamineUnit;
      if (_attackButton != null) _attackButton.onClick.RemoveAllListeners();
      if (_defendButton != null) _defendButton.onClick.RemoveAllListeners();
      if (_moveButton != null) _moveButton.onClick.RemoveAllListeners();
      
   }
    public void Update()
    {
      if (Input.GetKeyDown(KeyCode.Space))
        MenuManager.Instance.OpenMenu(_menu);
    }


    private void OnActionButtonClicked(CombatActions action)
    {
      Debug.Log($"[BattleMenuHE] Player confirmed action: {action}");

      //Hide();
      if (action != CombatActions.Defend)
      {
        Hide(); 
        InputManager.Instance.Actions.UI.Disable();
        cursor.SetActive(true);
      }

      PlayerAction?.Invoke(action);
    }

    private void HandleUserActions(BattlePhase bs, UnitController unitController)
    {
      //abilitiesPanel.SetActive(false);
      cursor.SetActive(false);
      if (bs is PlayerPhase)
        Show();
      else
        Hide();

      if (_canvasGroup.interactable)
        InputManager.Instance.Actions.UI.Enable();
      else
        InputManager.Instance.Actions.UI.Disable();

      /*
      int currMove = 0;
      foreach (Transform button in abilitiesPanel.transform)
      {
        if(currMove > unitController.GetData().Moves.Count-1)
          button.gameObject.SetActive(false);
        else
        {
          button.gameObject.SetActive(true);
          button.GetComponentInChildren<TextMeshProUGUI>().text = unitController.GetData().Moves[currMove].Name;
        }
        currMove++;
      }
    */
    }


    private void ExamineTile(TileController tc)
    {
      cursor.SetActive(false);
      //TODO: more stuff here later for info panels
    }

    private void ExitExamine()
    {
      cursor.SetActive(true);
    }

    private void ExamineUnit(UnitController uc)
    {
      Debug.Log(uc.GetData().Name);
      //TODO: more stuff here later for info panels  
    }
    
    
  }
}
