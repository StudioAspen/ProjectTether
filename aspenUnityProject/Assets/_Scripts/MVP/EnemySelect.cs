using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Consystently.UI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemySelect : VisualElement
{
  public static EnemySelect Instance { get; private set; }
  [SerializeField] private GameObject _catalogButtonPrefab;
  [SerializeField] private GameObject _content;
  [SerializeField] private List<UnitSim> _enemySims;
  [SerializeField] private ButtonVE _addButton;

  public Stack<UnitSim> VacantEnemySims { get; private set; } = new Stack<UnitSim>();
  public List<UnitSim> ActiveEnemySims { get; private set; } = new List<UnitSim>();

  private UnitDataSO _hoveredEnemyData = null, _selectedEnemyData = null;
  private Dictionary <ButtonVE, EnemyUnitSO> _catalogButtons = new Dictionary<ButtonVE, EnemyUnitSO>();

  
  protected override void Awake()
  {
    base.Awake();
    
    Instance ??= this;

    // Alphabetize and reverse the order of the enemy buttons, then put them into a stack
    foreach(UnitSim sim in _enemySims.OrderByDescending(x => x.gameObject.name))
    {
      sim.gameObject.SetActive(false);
      VacantEnemySims.Push(sim);
    }

    Hide();
  }

  private void Start()
  {
    _addButton.Component.interactable = _selectedEnemyData;
    List<EnemyUnitSO> enemyUnits = EnemyCatalog.Instance.Objects;
    PopulateEnemyList(enemyUnits);
  }

  private void PopulateEnemyList(List<EnemyUnitSO> enemyUnits)
  {
    foreach (EnemyUnitSO enemyUnit in enemyUnits)
    {
      GameObject obj = Instantiate(_catalogButtonPrefab, _content.transform);
      ButtonVE button = obj.GetComponent<ButtonVE>();
      button.TextChild.text = enemyUnit.Name;

      // Subscribe the necessary events
      EventTrigger.Entry hover = new EventTrigger.Entry() { eventID = EventTriggerType.PointerEnter };
      hover.callback.AddListener((data) => HoverEnterData(button));
      button.Trigger.triggers.Add(hover);

      EventTrigger.Entry select = new EventTrigger.Entry() { eventID = EventTriggerType.PointerClick};
      select.callback.AddListener((data) => SelectData(button));
      button.Trigger.triggers.Add(select);

      // EventTrigger.Entry deselect = new EventTrigger.Entry() { eventID = EventTriggerType.Deselect};
      // deselect.callback.AddListener((data) => EmptyData(button));
      // button.Trigger.triggers.Add(deselect);

      EventTrigger.Entry exit = new EventTrigger.Entry() { eventID = EventTriggerType.PointerExit };
      exit.callback.AddListener((data) => HoverExitData());
      button.Trigger.triggers.Add(exit);
      
      _catalogButtons.Add(button, enemyUnit);
    }
  }

  public void HoverEnterData(ButtonVE button)
  {
    _hoveredEnemyData = _catalogButtons[button];
    SimDetails.Instance.DisplayUnitDetails(_hoveredEnemyData);
  }

  public void SelectData(ButtonVE button)
  {
    if(button)
    {
      _selectedEnemyData = _catalogButtons[button];
      SimDetails.Instance.DisplayUnitDetails(_selectedEnemyData);
      _addButton.Component.interactable = _selectedEnemyData;
    }
  }

  public void HoverExitData()
  {
    if (_selectedEnemyData)
      SimDetails.Instance.DisplayUnitDetails(_selectedEnemyData);
    else
      SimDetails.Instance.ClearDetails();

    _hoveredEnemyData = null;
    _addButton.Component.interactable = _selectedEnemyData;
  }

  public void EmptyData()
  {
    _hoveredEnemyData = null;
    _selectedEnemyData = null;
    SimDetails.Instance.ClearDetails();
  }

  public void AddEnemy()
  {
    if(!_selectedEnemyData)
      return;

    if(VacantEnemySims.TryPop(out UnitSim result))
    {
      ActiveEnemySims.Add(result);
      result.gameObject.SetActive(true);
      result.Data = _selectedEnemyData;
      result.UpdateDetails(_selectedEnemyData);
      BattleSimManager.Instance.ActivateSim(result); 
    }
  }

  public void DuplicateEnemy()
  {
    
  }

  public void RemoveEnemy(UnitSim sim)
  {
    if(ActiveEnemySims.Contains(sim))
    {
      UnitSim last = ActiveEnemySims.Last();
      // If this is somewhere in the middle of the stack
      if(last != sim)
      {
        int index = ActiveEnemySims.IndexOf(sim);

        for(int i = index; i < ActiveEnemySims.Count - 1; i++)
        {
          UnitSim current = ActiveEnemySims[i];
          UnitSim next = ActiveEnemySims[i + 1];

          current.Data = next.Data;
          current.UpdateDetails(current.Data);

          if(next.Piece.Slot == next.Slot)
            current.ReturnPiece();
          else
          {
            UnitPieceSlot storedSlot = next.Piece.Slot;
            next.ReturnPiece();

            if(storedSlot.Tile)
              current.Piece.Move(storedSlot.Tile);
          }
        }
      }

      last.ReturnPiece();
      last.ClearDetails();
      last.gameObject.SetActive(false);
      
      ActiveEnemySims.Remove(last);
      VacantEnemySims.Push(last);
    }
  }
}
